using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class desC : MonoBehaviour
{
    [Header("UI")]
    public GameObject PhoneMessage;
    public GameObject CloseButton;
    [Space]
    public GameObject TrainFront;
    public GameObject TrainBack;
    public GameObject Train;
    [Tooltip("起始位置（畫面外）")] public Transform trainStartPoint;
    [Tooltip("停下來的位置（月台前）")] public Transform trainStopPoint;
    [Tooltip("火車移動速度")] public float trainSpeed = 5f;
    [Space]
    public GameObject DesPanel;
    public GameObject BlackPanel;//黑色遮罩

    [Header("角色")]
    public GameObject Player;
    Animator PlayerAnimator;
    [Tooltip("主角位置")] public Transform playerTransform;
    [Tooltip("主角走路速度")] public float walkSpeed = 5f;
    [Tooltip("走到的「中間」位置")] public Transform middlePoint;


    [Header("腳本")]
    public Animation animationScript;
    public CControll cControllScript;

    private void Awake()
    {
        animationScript = GetComponent<Animation>();
        cControllScript = Player.GetComponent<CControll>();
        

    }
    private void Start()
    {
        if (cControllScript == null)
        {
            cControllScript = Player.GetComponent<CControll>();
        }

        PlayerAnimator = cControllScript != null ? cControllScript.animator : null;

        if (PlayerAnimator == null)
        {
            // 保險：自己找一次
            PlayerAnimator = Player.GetComponentInChildren<Animator>();
        }

        if (PlayerAnimator == null)
        {
            Debug.LogError("[desC] 找不到 Player 的 Animator，請檢查 Player 階層。", this);
        }
        BlackPanel.SetActive(false);
        PhoneMessage.SetActive(false);
        DesPanel.SetActive(false);
        StartCoroutine(SceneFlow());
    }
    public void StartButton()
    {
        StartCoroutine(animationScript.FadeOutAndChangeScene(BlackPanel.GetComponent<CanvasGroup>(), 1.5f, "01"));
    }
    public void BackButton()
    {
        StartCoroutine(animationScript.FadeOutAndChangeScene(BlackPanel.GetComponent<CanvasGroup>(), 1.5f, "menu"));
    }

    IEnumerator SceneFlow()
    {
        // 1. 
        Debug.Log("主角走進場景");
        cControllScript.Target = new Vector2(11.1000004f, -9.359639f);
        cControllScript.StartAutoMoveTo(cControllScript.Target);

        // 等他走到指定 X
        yield return new WaitUntil(() => cControllScript.autoMoveFinished);
        PlayerAnimator.SetBool("walk",false);


        // 2. 
        Debug.Log("播放拿手機的動畫");
        PlayerAnimator.SetBool("phone", true);
        yield return new WaitForSeconds(cControllScript.phone.length);

        // 3. 
        Debug.Log("顯示圖片 Panel");
        PhoneMessage.SetActive(true);

        // 4. 
        Debug.Log("過幾秒後出現叉叉");
        yield return new WaitForSeconds(5f);
        CloseButton.SetActive(true);

        // 5. 
        Debug.Log("等玩家按關閉");
        bool closed = false;
        CloseButton.GetComponent<Button>().onClick.AddListener(() => closed = true);
        yield return new WaitUntil(() => closed);
        PlayerAnimator.SetBool("phone", false);

        //// 6. 黑幕淡入（0→1）
        //yield return StartCoroutine(animationScript.FadeIn(BlackPanel.GetComponent<CanvasGroup>(), 1f));

        // 7. 
        Debug.Log("火車進站（用移動而不是 Animator）");
        Train.transform.position = trainStartPoint.position; // 先放到起始點
        yield return StartCoroutine(MoveToPoint(Train.transform, trainStopPoint.position, trainSpeed));
        yield return new WaitUntil(() =>
    Vector3.Distance(Train.transform.position, trainStopPoint.position) < 0.01f);


        // 8. 
        Debug.Log("主角走向火車（被前景 sprite 遮住，看起來像上車）");
        cControllScript.Target = new Vector2(-4.1f, 4f);
        cControllScript.StartAutoMoveTo(cControllScript.Target);
        // 等他走到指定 X
        yield return new WaitUntil(() => cControllScript.autoMoveFinished);
        PlayerAnimator.SetBool("walk", false);
        cControllScript.rig.bodyType = RigidbodyType2D.Kinematic;

        // 9. 
        Debug.Log("畫面再次淡出黑（可省略）");
        BlackPanel.SetActive(true);
        yield return StartCoroutine(animationScript.FadeOutAndChangeScene(BlackPanel.GetComponent<CanvasGroup>(), 1f,"00"));

        // 劇情全部跑完
        Debug.Log("劇情全部跑完");
        cControllScript.EnablePlayerControl();
        // 10. 切換場景
        SceneManager.LoadScene("00");
    }
    //移動位置
    IEnumerator MoveToPoint(Transform obj, Vector3 targetPos, float speed)
    {
        targetPos.z = obj.position.z; // 2D 鎖 Z 軸，避免跑飛

        while (Vector3.Distance(obj.position, targetPos) > 0.01f)
        {
            obj.position = Vector3.MoveTowards(
                obj.position,
                targetPos,
                speed * Time.deltaTime
            );

            yield return null;
        }
        obj.position = targetPos; // 收尾精準貼到目標
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

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
    public Animator PlayerAnimation;
    [Tooltip("主角位置")] public Transform playerTransform;
    [Tooltip("主角走路速度")] public float walkSpeed = 5f;
    [Tooltip("走到的「中間」位置")] public Transform middlePoint;


    [Header("腳本")]
    public Animation animationScript;
    public CControll cControllScript;

    private void Awake()
    {
        animationScript = GetComponent<Animation>();
        cControllScript = Player.GetComponentInChildren<CControll>();
        PlayerAnimation = Player.GetComponentInChildren<Animator>();

    }
    private void Start()
    {
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
        cControllScript.Target = new Vector2(11.1000004f, -8.89999962f);
        cControllScript.StartAutoMoveTo(cControllScript.Target);

        // 等他走到指定 X
        yield return new WaitUntil(() => cControllScript.autoMoveFinished);
        PlayerAnimation.SetBool("walk",false);


        // 2. 
        Debug.Log("播放拿手機的動畫");
        PlayerAnimation.SetBool("Phone", true);
        yield return new WaitForSeconds(PlayerAnimation.GetCurrentAnimatorStateInfo(0).length);

        // 3. 
        Debug.Log("顯示圖片 Panel");
        PhoneMessage.SetActive(true);

        // 4. 
        Debug.Log("過幾秒後出現叉叉");
        yield return new WaitForSeconds(2f);
        CloseButton.SetActive(true);

        // 5. 
        Debug.Log("等玩家按關閉");
        bool closed = false;
        CloseButton.GetComponent<Button>().onClick.AddListener(() => closed = true);
        yield return new WaitUntil(() => closed);
        PlayerAnimation.SetBool("Phone", false);

        //// 6. 黑幕淡入（0→1）
        //yield return StartCoroutine(animationScript.FadeIn(BlackPanel.GetComponent<CanvasGroup>(), 1f));

        // 7. 
        Debug.Log("火車進站（用移動而不是 Animator）");
        Train.transform.position = trainStartPoint.position; // 先放到起始點
        yield return StartCoroutine(MoveToPoint(Train.transform, trainStopPoint.position, trainSpeed));

        yield return new WaitForSeconds(5f);

        // 8. 
        Debug.Log("主角走向火車（被前景 sprite 遮住，看起來像上車）");
        cControllScript.isAutoMoving = true;
        cControllScript.Target = new Vector2(-3.6f, 0);
        PlayerAnimation.SetBool("walk",true);
        yield return new WaitForSeconds(5f);

        // 9. 
        Debug.Log("畫面再次淡出黑（可省略）");
        yield return StartCoroutine(animationScript.FadeIn(BlackPanel.GetComponent<CanvasGroup>(), 1f));

        // 劇情全部跑完
        Debug.Log("劇情全部跑完");
        cControllScript.EnablePlayerControl();
        // 10. 切換場景
        SceneManager.LoadScene("01");
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
    }

}

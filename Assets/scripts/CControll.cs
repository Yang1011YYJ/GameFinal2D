using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CControll : MonoBehaviour
{
    [Header("動畫")]
    public Animator animator;
    public AnimationClip idle;
    [Tooltip("看手機")]public AnimationClip phone;
    [Tooltip("走路")] public AnimationClip walk;
    [Tooltip("轉身")] public AnimationClip turn;
    [Tooltip("遊戲失敗")] public AnimationClip die;
    [Tooltip("坐著睡著")] public AnimationClip sitsleep;
    [Tooltip("抓頭")] public AnimationClip Catch;

    [Header("角色外表")]
    public SpriteRenderer spriteRenderer;

    public Rigidbody2D rig;

    [Header("移動參數")]
    public float moveSpeed = 5f;
    public float maxSpeed = 10f;
    [Tooltip("true = 玩家可以用方向鍵控制")]public bool playerControlEnabled = true;
    [Tooltip("是否正在自動移動")]public bool isAutoMoving = false;
    [Tooltip("自動移動的目標座標")] public Vector2 Target;
    [Tooltip("判定抵達目標的容許誤差")]public float arriveThreshold = 0.05f;
    [Tooltip("自動移動是否結束（給外部查詢用）")]public bool autoMoveFinished = false;
    float x; // 最後實際拿去移動用的輸入值

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        rig = GetComponent<Rigidbody2D>();
        isAutoMoving = false;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 預設為 0，避免殘留
        x = 0f;

        if (isAutoMoving)
        {
            float diff = Target.x - transform.position.x;

            // 如果離目標很近，就算抵達
            if (Mathf.Abs(diff) <= arriveThreshold)
            {
                Debug.Log("離目標很近，算抵達");
                // 停下來
                x = 0f;
                isAutoMoving = false;
                autoMoveFinished = true;

                // 把座標修正到目標（避免物理晃一下）
                Vector2 pos = transform.position;
                pos= Target;
                transform.position = pos;
            }
            else
            {
                // 按方向決定往左(-1)還是往右(1)
                x = Mathf.Sign(diff);
                autoMoveFinished = false;
            }
        }
        else if (playerControlEnabled)// 2. 平常玩家控制
        {
            x = Input.GetAxis("Horizontal");
            
        }
        else // 3. 其他情況（例如劇情中不給動）
        {
            x = 0f;
        }

        //動畫設定(判斷是否在走路)
        if (Mathf.Abs(x) > 0.01f)
        {
            animator.SetBool("walk", true);
            // 走路時使用側面圖
            //spriteRenderer.sprite = image_1;
            //if (x > 0)
            //{
            //    //sprite.flipX = true;
            //    //角色面向右邊
            //    spriteRenderer.flipX = true;
            //}
            //else
            //{
            //    spriteRenderer.flipX = false;
            //}
            spriteRenderer.flipX = x > 0;
        }
        else
        {
            animator.SetBool("walk", false);
            //spriteRenderer.sprite = image_0;
            // 沒在移動，用正面圖
            spriteRenderer.flipX = false;

        }
    }

    void FixedUpdate()
    {
        // 角色移動 & 限速
        rig.AddForce(new Vector2(x * moveSpeed, 0f));

        rig.velocity = new Vector2(
            Mathf.Clamp(rig.velocity.x, -maxSpeed, maxSpeed),
            rig.velocity.y
        );
    }

    /// <summary>
    /// 給外部呼叫，開始自動走到某個 X 位置
    /// </summary>
    public void StartAutoMoveTo(Vector2 Target)
    {
        this.Target = Target;
        Debug.Log("1");
        isAutoMoving = true;
        Debug.Log("2");
        autoMoveFinished = false;

        // 劇情時通常會關掉玩家控制，避免亂動
        playerControlEnabled = false;

        // 順便開啟走路動畫（Update 也會處理，不過這樣比較即時）
        animator.SetBool("walk", true);
    }

    /// <summary>
    /// 劇情結束後，恢復玩家控制
    /// </summary>
    public void EnablePlayerControl()
    {
        playerControlEnabled = true;
        isAutoMoving = false;
    }
}

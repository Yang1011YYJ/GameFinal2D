using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CControll : MonoBehaviour
{
    public Animator animator;

    [Header("角色外表")]
    public SpriteRenderer spriteRenderer;
    [Tooltip("面正面")]
    public Sprite image_0;
    [Tooltip("面左邊")]
    public Sprite image_1;
    [Tooltip("面背面")]
    public Sprite image_2;

    public Rigidbody2D rig;

    [Header("移動參數")]
    public float moveSpeed = 5f;
    public float maxSpeed = 10f;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rig = GetComponent<Rigidbody2D>();
    }

    float x; // 宣告成成員變數
    // Update is called once per frame
    void Update()
    {
        x = Input.GetAxis("Horizontal");

        //動畫設定(判斷是否在走路)
        if (Mathf.Abs(x) > 0.01f)
        {
            animator.SetBool("walk", true);
            // 走路時使用側面圖
            spriteRenderer.sprite = image_1;
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
            spriteRenderer.sprite = image_0;
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
}

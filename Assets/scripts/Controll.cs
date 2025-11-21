using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controll : MonoBehaviour
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

    public Rigidbody rig;
   
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        rig = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        // 角色移動
        rig.AddForce(new Vector2(x * 5f, 0f));

        //限制速度最大值
        rig.velocity = new Vector2(
            Mathf.Clamp(rig.velocity.x, -10f, 10f),
            rig.velocity.y
        );
        print(rig.velocity);

        //動畫設定(判斷是否在走路)
        if (x != 0)
        {
            animator.SetBool("walk", true);
            // 走路時使用側面圖
            spriteRenderer.sprite = image_1;
            if (x > 0)
            {
                //sprite.flipX = true;
                //角色面向右邊
                spriteRenderer.flipX = true;
            }
            else
            {
                spriteRenderer.flipX = false;
            }
        }
        else
        {
            animator.SetBool("walk", false);
            // 沒在移動，用正面圖
            spriteRenderer.flipX = false;      // 正面圖通常不用翻
            spriteRenderer.sprite = image_0;
        }
    }
}

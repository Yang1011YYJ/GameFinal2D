using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class First : MonoBehaviour
{
    [Header("腳本")]
    public Animation animationScript;

    [Header("計算圈圈數量")]


    [Header("其他")]
    public GameObject BlackPanel;//黑色遮罩

    private void Awake()
    {
        animationScript = GetComponent<Animation>();
        BlackPanel = GameObject.Find("BlackPanel");
    }
    private void Start()
    {
        BlackPanel.SetActive(false);
        StartCoroutine(animationScript.FadeIn(BlackPanel.GetComponent<CanvasGroup>(), 1.5f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

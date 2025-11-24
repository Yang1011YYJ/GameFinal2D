using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMenu : MonoBehaviour
{
    [Header("腳本")]
    public Animation animationScript;

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
    }
    public void SceneChange()
    {
        StartCoroutine(animationScript.FadeOutAndChangeScene(BlackPanel.GetComponent<CanvasGroup>(), 1.5f, "des"));
    }
}

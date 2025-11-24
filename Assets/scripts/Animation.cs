using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Animation : MonoBehaviour
{
    void Start()
    {
    }

    public IEnumerator FadeOutAndChangeScene(CanvasGroup panel, float fadeDuration, string nextScene)
    {
        panel.gameObject.SetActive(true);
        float timer = 0f;
        panel.alpha = 0f;

        // 淡入
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            panel.alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            yield return null;
        }

        panel.alpha = 1f;

        // 淡出結束 → 換場景
        SceneManager.LoadScene(nextScene);
    }
    public IEnumerator FadeIn(CanvasGroup panel, float fadeDuration)
    {
        panel.gameObject.SetActive(true);
        float timer = 0f;
        panel.alpha = 1f;

        // 淡入
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            panel.alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            yield return null;
        }

        panel.alpha = 0f;
        panel.gameObject.SetActive(false);
    }
}

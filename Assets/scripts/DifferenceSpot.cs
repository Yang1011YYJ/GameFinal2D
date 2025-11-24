using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifferenceSpot : MonoBehaviour
{
    [Header("圈圈預置物")]
    public GameObject circlePrefab;      // 指到 CircleMark.prefab

    [Header("圈圈要放在哪個 UI 爸爸下面")]
    public RectTransform canvasRect;     // 通常拖 Canvas 或 CircleLayer
    bool found = false;

    public void OnClickSpot()
    {
        if (found) return;
        found = true;

        // 生成圈圈在點的位置
        GameObject circle = Instantiate(circlePrefab, canvasRect);
        // 把圈圈的位置對齊這個 Spot
        var spotRect = GetComponent<RectTransform>();
        var circleRect = circle.GetComponent<RectTransform>();
        circleRect.anchoredPosition = spotRect.anchoredPosition;

        // 播圈圈填滿動畫
        var filler = circle.GetComponent<CircleFill>();
        if (filler != null)
        {
            filler.Play();
        }
    }
}


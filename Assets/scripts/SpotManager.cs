using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpotManager : MonoBehaviour
{
    public DifferenceSpot[] spots;  // 自動抓，不用手動填

    public int totalCount;          // 總共有幾個 spot
    public int foundCount;          // 已經找到幾個
                                    // Start is called before the first frame update
                                    // 自動抓場景中所有 DifferenceSpot（包含沒啟用的，用 true）
    [Header("UI")]
    public TextMeshProUGUI text;//計算數量

    void Awake()
    {
        spots = FindObjectsOfType<DifferenceSpot>(true);

        totalCount = spots.Length;
        foundCount = 0;
        text.text = foundCount + " / " + totalCount;//更新初始狀態

        // 順便幫每個 spot 設定 manager
        foreach (var s in spots)
        {
            s.manager = this;
        }

        Debug.Log($"總共有 {totalCount} 個可以找的地方");
    }

    // 被某個 Spot 呼叫
    public void OnSpotFound(DifferenceSpot spot)
    {
        foundCount++;

        Debug.Log($"找到第 {foundCount} 個，進度：{foundCount} / {totalCount}");

        // 這裡之後可以加：
        text.text = foundCount+" / "+totalCount;
        // - 如果 foundCount == totalCount → 顯示「全部找完」畫面
        if (foundCount >= totalCount)
        {
            Debug.Log("全部找完啦！");
            // TODO：彈出完成畫面、下一關按鈕等等
        }
    }
}

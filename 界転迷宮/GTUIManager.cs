using UnityEngine;

[System.Serializable]
public class StageUISet
{
    [Header("このステージで表示するUIたち")]
    public GameObject ui1;
    public GameObject ui2;
}

public class GTUIManager : MonoBehaviour
{
    [Header("ステージごとのUIセット (1〜7)")]
    [SerializeField] private StageUISet[] stageUISets = new StageUISet[7];

    private void Start()
    {
        int stageIndex = StageData.SelectedStageIndex; // ← StageManagerと同じ仕組みを利用
        ShowStageUI(stageIndex);
    }

    public void ShowStageUI(int stageIndex)
    {
        // まず全部非表示にする
        for (int i = 0; i < stageUISets.Length; i++)
        {
            if (stageUISets[i].ui1 != null) stageUISets[i].ui1.SetActive(false);
            if (stageUISets[i].ui2 != null) stageUISets[i].ui2.SetActive(false);
        }

        // ステージ番号に対応するUIだけ表示
        if (stageIndex > 0 && stageIndex <= stageUISets.Length)
        {
            StageUISet set = stageUISets[stageIndex - 1];
            if (set.ui1 != null) set.ui1.SetActive(true);
            if (set.ui2 != null) set.ui2.SetActive(true);
        }
        else
        {
            Debug.LogWarning($"ステージ番号 {stageIndex} は範囲外です");
        }
    }
}

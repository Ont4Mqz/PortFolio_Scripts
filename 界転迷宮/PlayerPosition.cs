using UnityEngine;

/// <summary>
/// ステージ4と6のときのみプレイヤーの初期位置を設定するスクリプト
/// </summary>
public class PlayerPosition : MonoBehaviour
{
    [Header("プレイヤーオブジェクト")]
    [SerializeField] private GameObject player;

    [Header("ステージごとの初期位置")]
    [SerializeField] private Vector3 stage4Position;
    [SerializeField] private Vector3 stage6Position;

    private void Start()
    {
        // 選択されたステージ番号を取得（StageData から）
        int stageNum = StageData.SelectedStageIndex;

        // 初期位置を決定
        Vector3? initPos = GetInitialPosition(stageNum);

        // 該当するステージなら移動、それ以外は何もしない
        if (initPos.HasValue && player != null)
        {
            player.transform.position = initPos.Value;
        }
    }

    /// <summary>
    /// ステージ番号に応じて初期位置を返す（4と6のみ）
    /// </summary>
    private Vector3? GetInitialPosition(int stageNum)
    {
        switch (stageNum)
        {
            case 4: return stage4Position;
            case 6: return stage6Position;
            default:
                return null; // 何もしない
        }
    }
}

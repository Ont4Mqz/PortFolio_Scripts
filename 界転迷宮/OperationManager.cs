using UnityEngine;

public class OperationManager : MonoBehaviour
{
    [Header("2Dモードで表示するUI")]
    public GameObject howtoUI2D;       // 2Dモード時に表示するUI

    [Header("3Dモードで表示するUI")]
    public GameObject howtoUI3D;       // 3Dモード時に表示するUI

    [Header("ゲームオーバー時に表示するUIパネル")]
    public GameObject gameOverPanel;   // ゲームオーバー判定に使うパネル（Active状態を監視）

    [Header("ゲームオーバー時に表示する説明UI")]
    public GameObject gameOverUI;      // ゲームオーバー時に表示する説明UI

    private bool lastIsCamera2D;       // 前回のカメラ状態

    private void Start()
    {
        if (IsGameOverActive())             // ゲームオーバーパネルが既に表示中なら
        {
            ShowGameOverUI();                // ゲームオーバー説明UIだけ表示
            return;                          // 他のUI初期化は行わない
        }

        lastIsCamera2D = CameraSwitcher.IsCamera2D; // 現在のカメラ状態を保存
        ApplyUIState(lastIsCamera2D);               // 初期UI反映
    }

    private void Update()
    {
        if (IsGameOverActive())               // ゲームオーバーパネルがアクティブなら
        {
            ShowGameOverUI();                  // ゲームオーバーUI表示
            return;                            // 2D/3D切替はしない
        }

        // 通常時の2D/3D切替処理
        if (CameraSwitcher.IsCamera2D != lastIsCamera2D)
        {
            lastIsCamera2D = CameraSwitcher.IsCamera2D;
            ApplyUIState(lastIsCamera2D);
        }
    }

    private bool IsGameOverActive()
    {
        return gameOverPanel != null && gameOverPanel.activeSelf; // ゲームオーバーパネルが表示中か
    }

    private void ShowGameOverUI()
    {
        if (gameOverUI != null) gameOverUI.SetActive(true);     // ゲームオーバー説明UIを表示
        if (howtoUI2D != null) howtoUI2D.SetActive(false);      // 2D用UIは非表示
        if (howtoUI3D != null) howtoUI3D.SetActive(false);      // 3D用UIも非表示
    }

    private void ApplyUIState(bool is2D)
    {
        if (howtoUI2D != null) howtoUI2D.SetActive(is2D);       // 2DモードUIの表示切替
        if (howtoUI3D != null) howtoUI3D.SetActive(!is2D);      // 3DモードUIの表示切替
        if (gameOverUI != null) gameOverUI.SetActive(false);    // 通常時はゲームオーバーUIを非表示
    }
}

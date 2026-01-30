using UnityEngine;

public class FPSManager : MonoBehaviour
{
    [SerializeField] private int targetFPS = 60; // 目標FPS

    private void Awake()
    {
        QualitySettings.vSyncCount = 0; // VSyncを無効
        Application.targetFrameRate = targetFPS; // FPSを設定
    }

    private void Update()
    {
        if (Application.targetFrameRate != targetFPS)
        {
            Application.targetFrameRate = targetFPS; // 常に60FPSに戻す
        }
    }
}

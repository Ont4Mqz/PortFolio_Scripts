using UnityEngine;

public class FrameRateLimiter : MonoBehaviour
{
    void Start()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0; // VSync を無効にする（必要に応じて）
    }
}

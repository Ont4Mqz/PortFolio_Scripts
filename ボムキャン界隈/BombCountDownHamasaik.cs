using UnityEngine;
using TMPro;
using System.Collections;

public class BombCountDownHamasaki : MonoBehaviour
{
    public TextMeshProUGUI timerText1;     // 1つ目のタイマー表示用のTextMeshPro
    public TextMeshProUGUI timerText2;     // 2つ目のタイマー表示用のTextMeshPro
    public float startDelay = 5f;          // タイマー開始までの遅延時間
    public float countdownTime = 10f;      // カウントダウンの長さ（秒）

    private bool isCountingDown = false;
    private float currentTime;

    void Start()
    {
        // ゲーム開始から指定秒数後にカウントダウンを開始
        StartCoroutine(StartCountdownAfterDelay());
    }

    IEnumerator StartCountdownAfterDelay()
    {
        // 指定された遅延時間を待機
        yield return new WaitForSeconds(startDelay);

        // カウントダウンを開始
        isCountingDown = true;
        currentTime = countdownTime;
    }

    void Update()
    {
        if (isCountingDown)
        {
            // 時間を減らす
            currentTime -= Time.deltaTime;

            // 小数点第1位まで表示
            string formattedTime = currentTime.ToString("F1");
            if (timerText1 != null) timerText1.text = formattedTime;
            if (timerText2 != null) timerText2.text = formattedTime;

            // カウントダウンが終了したら停止
            if (currentTime <= 0)
            {
                currentTime = 0;
                isCountingDown = false;
                if (timerText1 != null) timerText1.text = "0.0";
                if (timerText2 != null) timerText2.text = "0.0";
            }
        }
    }
}

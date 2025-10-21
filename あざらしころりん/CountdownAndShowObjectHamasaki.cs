using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CountdownAndShowObjectHamasaki : MonoBehaviour // クラス名をファイル名と一致させる
{
    public Button startButton;         // カウントダウンを開始するボタン
    public Text countdownText;         // カウントダウン表示用のテキストUI
    public GameObject objectToShow;    // カウントダウン後に表示するオブジェクト
    public float countdownTime = 3f;   // カウントダウン時間
    public float objectVisibleDuration = 1f; // オブジェクトを表示する時間

    void Start()
    {
        objectToShow.SetActive(false);   // 初めにオブジェクトを非表示にする
        countdownText.text = "";         // カウントダウン表示を空にする
        startButton.onClick.AddListener(StartCountdown); // ボタンのクリックイベントを設定
    }

    void StartCountdown()
    {
        startButton.gameObject.SetActive(false); // ボタンを非表示にする
        StartCoroutine(CountdownCoroutine());
    }

    IEnumerator CountdownCoroutine()
    {
        float currentTime = countdownTime;

        // カウントダウン処理
        while (currentTime > 0)
        {
            countdownText.text = Mathf.Ceil(currentTime).ToString(); // カウントダウンの整数部分を表示
            yield return new WaitForSeconds(1f);  // 1秒待つ
            currentTime--;
        }

        // 0秒の時点では何も表示しない
        countdownText.text = "";

        // オブジェクトを表示する
        objectToShow.SetActive(true);

        // 指定した時間だけオブジェクトを表示する
        yield return new WaitForSeconds(objectVisibleDuration);

        // オブジェクトを非表示にする
        objectToShow.SetActive(false);
    }
}

using UnityEngine;
using UnityEngine.UI;

public class StaticImageTimer : MonoBehaviour
{
    [SerializeField] private Sprite[] numberSprites; // 0から9までのイメージ
    [SerializeField] private Image[] minuteImages;   // 分を表示するImageコンポーネント（2桁用）
    [SerializeField] private Image colonImage;       // コロン（:）を表示するImageコンポーネント
    [SerializeField] private Image[] secondImages;   // 秒を表示するImageコンポーネント（2桁用）

    //[SerializeField] private int minutes = 5;        // 初期分（例: 5分）
    //[SerializeField] private int seconds = 49;       // 初期秒（例: 49秒）

    void Start()
    {
        //変更した(中村)
        // 初期分と秒を表示
        UpdateNumberImages((int)TimerManager.Min, minuteImages);
        UpdateNumberImages((int)TimerManager.Sec, secondImages);
    }

    private void UpdateNumberImages(int number, Image[] digitImages)
    {
        // 数値を文字列化して桁ごとに分解（常に2桁で表示）
        string numberStr = number.ToString("D2"); // "D2" で2桁表示を強制

        // 各桁に対応するイメージを更新
        for (int i = 0; i < digitImages.Length; i++)
        {
            int digit = int.Parse(numberStr[i].ToString());
            digitImages[i].sprite = numberSprites[digit];
            digitImages[i].enabled = true;
        }
    }
}

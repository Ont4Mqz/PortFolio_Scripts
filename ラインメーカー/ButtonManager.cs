using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    // 正しい選択肢のボタン
    public Button[] choiceButtons; // 8つのボタン（インスペクターで設定可能）
    public GameObject[] objectsToDestroy; // 消すオブジェクト（複数選択可能）
    public int correctButtonIndex; // 正しいボタンのインデックス

    private void Start()
    {
        // ボタンにリスナーを追加
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            int index = i; // ローカル変数に保存
            choiceButtons[i].onClick.AddListener(() => OnButtonClicked(index));
        }
    }

    private void OnButtonClicked(int buttonIndex)
    {
        if (buttonIndex == correctButtonIndex)
        {
            // 正しい選択肢が押された場合
            foreach (GameObject obj in objectsToDestroy)
            {
                Destroy(obj); // オブジェクトを消す
            }
            Debug.Log("正しい選択肢が選ばれました！");
        }
        else
        {
            // 間違った選択肢が押された場合
            Debug.Log("間違った選択肢が選ばれました。GameOverシーンに遷移します。");
            SceneManager.LoadScene("GameOverHamasaki"); // GameOverシーンに遷移
        }
    }
}

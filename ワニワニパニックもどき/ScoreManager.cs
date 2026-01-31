using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [Header("スコア設定")]
    [SerializeField] private int hitScore = 100;

    [Header("表示")]
    [SerializeField] private TMP_Text scoreText;

    private int score = 0;

    void Start()
    {
        UpdateScoreText();
    }

    /// <summary>
    /// ワニを叩いたときに呼ぶ
    /// </summary>
    public void AddScore()
    {
        score += hitScore;
        UpdateScoreText();
    }

    /// <summary>
    /// 任意の値を加算したい場合用（拡張用）
    /// </summary>
    public void AddScore(int value)
    {
        score += value;
        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        if (scoreText == null) return;

        scoreText.text =
            "Score\n" +
            score.ToString();
    }

    /// <summary>
    /// ゲームリスタート用
    /// </summary>
    public void ResetScore()
    {
        score = 0;
        UpdateScoreText();
    }
}

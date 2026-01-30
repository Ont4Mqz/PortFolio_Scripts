using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [Header("UI参照")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private int playerID = 1;

    public static int Player1Score { get; private set; } = 0;
    public static int Player2Score { get; private set; } = 0;

    private void Awake() // ゲームシーンに入るたびにスコア初期化
    {
        Player1Score = 0;
        Player2Score = 0;
        DifficultyLevel.score1 = 0;
        DifficultyLevel.score2 = 0;
    }

    private void Start()
    {
        UpdateScoreUI();
    }

    public void AddScore(int points)
    {
        AddScore(playerID, points);
    }

    public void AddScore(int targetPlayerId, int points)
    {
        if (targetPlayerId == 1)
          { Player1Score += points;
            DifficultyLevel.score1 += points;
        }
        else if (targetPlayerId == 2)
           { Player2Score += points;
            DifficultyLevel.score2 += points;
        }

        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText == null) return;

        int currentScore = playerID == 2 ? Player2Score : Player1Score;
        scoreText.text = $"{currentScore}";
    }

    public static void ResetAll()
    {
        Player1Score = 0;
        Player2Score = 0;
    }
}

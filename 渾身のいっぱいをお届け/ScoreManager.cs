using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public float Score { get; private set; }

    [SerializeField] private GameObject gameOverPanel; // ゲームオーバーパネル
    [SerializeField] private string gameOverTag = "Enemy"; // ゲームオーバー用コライダーのタグ

    private bool isGameOver = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        // Playerのスコアを毎フレーム取得
        Score = Player.GetScore();

        // スコアが0以下になったらゲームオーバー
        if (!isGameOver && Score <= 0)
        {
            TriggerGameOver();
        }


    }

    private void TriggerGameOver()
    {
        isGameOver = true;
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            Invoke(nameof(ToTitle), 3.5f);
        }
    }
    void ToTitle()
    {
        SceneManager.LoadScene("TitleScene");

    }

    // コライダーに当たったらゲームオーバー（このスクリプトがアタッチされたオブジェクトにCollider2Dが必要）
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isGameOver && collision.CompareTag(gameOverTag))
        {
            TriggerGameOver();
        }
    }
    public bool IsGameOver()
    {
        return isGameOver;
    }

}
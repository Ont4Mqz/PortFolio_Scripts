using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class TimerManager : MonoBehaviour
{
    [Header("ゲーム制限時間")]
    [SerializeField] private float gameTimeLimit = 60f;

    [Header("レベルアップ設定")]
    [SerializeField] private float levelDuration = 10f;

    [Header("ゲーム開始カウントダウン")]
    [SerializeField] private float startCountDownTime = 3f;

    [Header("リザルト")]
    [SerializeField] private float finishWaitTime = 2f;
    [SerializeField] private string resultSceneName = "ResultScene";

    [Header("表示")]
    [SerializeField] private TMP_Text gameTimeText;
    [SerializeField] private TMP_Text levelTimerText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text startCountText;
    [SerializeField] private TMP_Text finishText;

    private float remainingGameTime;
    private float remainingLevelTime;
    private int speedLevel = 1;

    private bool isGameStarted = false;
    private bool isGameOver = false;

    public int SpeedLevel => speedLevel;
    public bool IsGameStarted => isGameStarted;
    public bool IsGameOver => isGameOver;

    void Start()
    {
        if (finishText != null)
            finishText.text = "";
    }

    // CameraMoverから呼ぶ
    public void StartGameCountdown()
    {
        StartCoroutine(GameStartCountDown());
    }

    IEnumerator GameStartCountDown()
    {
        float time = startCountDownTime;

        while (time > 0f)
        {
            startCountText.text = Mathf.CeilToInt(time).ToString();
            time -= Time.deltaTime;
            yield return null;
        }

        startCountText.text = "GO!";
        yield return new WaitForSeconds(0.5f);
        startCountText.text = "";

        StartGame();
    }

    void StartGame()
    {
        isGameStarted = true;
        remainingGameTime = gameTimeLimit;
        remainingLevelTime = levelDuration;
        UpdateUI();
    }

    void Update()
    {
        if (!isGameStarted || isGameOver) return;

        // 制限時間
        remainingGameTime -= Time.deltaTime;
        if (remainingGameTime <= 0f)
        {
            remainingGameTime = 0f;
            StartCoroutine(GameFinish());
            return;
        }

        // レベル管理
        remainingLevelTime -= Time.deltaTime;
        if (remainingLevelTime <= 0f)
        {
            speedLevel++;
            remainingLevelTime = levelDuration;
        }

        UpdateUI();
    }

    IEnumerator GameFinish()
    {
        isGameOver = true;

        if (finishText != null)
        {
            finishText.text = "Finish!";
        }

        yield return new WaitForSeconds(finishWaitTime);

        SceneManager.LoadScene(resultSceneName);
    }

    void UpdateUI()
    {
        if (gameTimeText != null)
            gameTimeText.text = Mathf.CeilToInt(remainingGameTime).ToString();

        if (levelTimerText != null)
            levelTimerText.text = Mathf.CeilToInt(remainingLevelTime).ToString();

        if (levelText != null)
            levelText.text = $"LV {speedLevel}";
    }
}

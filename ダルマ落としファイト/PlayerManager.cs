using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;

public class PlayerManager : MonoBehaviour
{
    [Header("参照スクリプト")]
    [SerializeField] private TimerCount timerCount; // タイマー参照
    [SerializeField] private DarumaManager player1Manager; // プレイヤー1管理
    [SerializeField] private DarumaManager player2Manager; // プレイヤー2管理
  
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI player1ReadyText; // プレイヤー1準備完了テキスト
    [SerializeField] private TextMeshProUGUI player2ReadyText; // プレイヤー2準備完了テキスト
    [SerializeField] private List<GameObject> gameUIs = new List<GameObject>(); // 準備完了まで非表示にするUIリスト

    [Header("アニメーター")]
    [SerializeField] private Animator CountDownAnimation; // カウントダウンアニメーション

    [SerializeField] private AudioClip SECountDown;
    [SerializeField] private AudioClip Decide;
    [SerializeField] private AudioSource seaudioManager;
    [SerializeField] private AudioSource countDownManager;

    AudioSource audioSource;

    private Gamepad pad1;
    private Gamepad pad2;
    private bool player1Ready = false; //準備完了のBool
    private bool player2Ready = false;
    private bool coroutineStarted = false; //一度だけコルーチンを走らせるためのフラグ

    private void Start()
    {
        seaudioManager = GameObject.Find("BGMManager").GetComponent<AudioSource>();
        countDownManager = GameObject.Find("SEManager").GetComponent<AudioSource>();
        timerCount.gameFlag = true; 
        var pads = Gamepad.all; //今ついてるコントローラーを取得
        if (pads.Count >= 1) pad1 = pads[0];
        if (pads.Count >= 2) pad2 = pads[1];

        if (player1Manager != null && pad1 != null) player1Manager.SetGamepad(pad1); //各プレイヤーのDarumaManagerに紐付け
        if (player2Manager != null && pad2 != null) player2Manager.SetGamepad(pad2);

        if (player1ReadyText != null) player1ReadyText.text = "Push ○!"; //準備完了のテキスト
        if (player2ReadyText != null) player2ReadyText.text = "Push ○!";

        foreach (GameObject ui in gameUIs)  // 準備完了まで非表示にするUI
        {
            if (ui != null)
                ui.SetActive(false);
        }
    }

    private void Update()
    {
        if (!timerCount.gameFlag) return;
        if (pad1 != null && pad1.buttonEast.wasPressedThisFrame) //プレイヤー1の準備入力
        {
            seaudioManager.PlayOneShot(Decide);
            player1Ready = !player1Ready;
            if (player1ReadyText != null)
                player1ReadyText.text = player1Ready ? "Ready!" : "Not Ready";
        }

        if (pad2 != null && pad2.buttonEast.wasPressedThisFrame) //プレイヤー2の準備入力
        {
            seaudioManager.PlayOneShot(Decide);
            player2Ready = !player2Ready;
            if (player2ReadyText != null)
                player2ReadyText.text = player2Ready ? "Ready!" : "Not Ready";
        }

        // キーボード操作対応(デバッグ用)
        #if UNITY_EDITOR //エディタ上でのみ有効
        var keyboard = Keyboard.current;
        if (keyboard != null)
        {
            if (keyboard.digit1Key.wasPressedThisFrame && !player1Ready) // ← 1キー
            {
                seaudioManager.PlayOneShot(Decide);
                player1Ready = true;
                player1ReadyText.text = "Ready!";
                Debug.Log("Player1 Keyboard Ready!");
            }

            if (keyboard.digit2Key.wasPressedThisFrame && !player2Ready) // ← 2キー
            {
                seaudioManager.PlayOneShot(Decide);
                player2Ready = true;
                player2ReadyText.text = "Ready!";
                Debug.Log("Player2 Keyboard Ready!");
            }
        }
        #endif

        if (player1Ready && player2Ready && !coroutineStarted) //両方とも準備オッケーだったら
        {
            timerCount.gameFlag = false;
            coroutineStarted = true; //1回だけ実行されるようにした
            StartCoroutine(AfterP1P2Ready());
        }
    }

    private IEnumerator AfterP1P2Ready() //両方準備オッケーの時用のコルーチン
    {
        yield return new WaitForSeconds(1f); //両方準備オッケーになってからカウントダウンが始まるまでの時間
        player1ReadyText.gameObject.SetActive(false);
        player2ReadyText.gameObject.SetActive(false);

        // UIを表示
        foreach (GameObject ui in gameUIs)
        {
            if (ui != null)
                ui.SetActive(true);
        }

        Debug.Log("カウントダウンスタート");
        CountDownAnimation.SetTrigger("LetsCount"); //カウントダウンアニメーションのトリガー
        countDownManager.PlayOneShot(SECountDown);
    }
}
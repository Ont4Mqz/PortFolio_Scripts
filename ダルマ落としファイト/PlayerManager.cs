using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;

public class PlayerManager : MonoBehaviour
{
    [Header("参照スクリプト")]
    [SerializeField] private TimerCount timerCount;
    [SerializeField] private DarumaManager player1Manager;
    [SerializeField] private DarumaManager player2Manager;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI player1ReadyText;
    [SerializeField] private TextMeshProUGUI player2ReadyText;

    [Header("アニメーター")]
    [SerializeField] private Animator CountDownAnimation;

    private Gamepad pad1;
    private Gamepad pad2;

    private bool player1Ready = false; //準備完了のBool
    private bool player2Ready = false;
    private bool coroutineStarted = false; // ← ★一度だけコルーチンを走らせるためのフラグ

    private void Start()
    {
        var pads = Gamepad.all; //今ついてるコントローラーを取得
        if (pads.Count >= 1) pad1 = pads[0];
        if (pads.Count >= 2) pad2 = pads[1];

        if (player1Manager != null && pad1 != null) player1Manager.SetGamepad(pad1); //各プレイヤーのDarumaManagerに紐付け
        if (player2Manager != null && pad2 != null) player2Manager.SetGamepad(pad2);

        if (player1ReadyText != null) player1ReadyText.text = "Push ○!"; //準備完了のテキスト
        if (player2ReadyText != null) player2ReadyText.text = "Push ○!";
    }

    private void Update()
    {
        if (pad1 != null && pad1.buttonEast.wasPressedThisFrame) //プレイヤー1の準備入力
        {
            player1Ready = !player1Ready;
            if (player1ReadyText != null)
                player1ReadyText.text = player1Ready ? "Ready!" : "Not Ready";
        }

        if (pad2 != null && pad2.buttonEast.wasPressedThisFrame) //プレイヤー2の準備入力
        {
            player2Ready = !player2Ready;
            if (player2ReadyText != null)
                player2ReadyText.text = player2Ready ? "Ready!" : "Not Ready";
        }

        if (player1Ready && player2Ready && !coroutineStarted) // 両方とも準備オッケーだったら
        {
            coroutineStarted = true; //1回だけ実行されるようにした
            StartCoroutine(AfterP1P2Ready());
        }
    }

    private IEnumerator AfterP1P2Ready() //両方準備オッケーの時用のコルーチン
    {
        yield return new WaitForSeconds(1f); //両方準備オッケーになってからカウントダウンが始まるまでの時間
        player1ReadyText.gameObject.SetActive(false);
        player2ReadyText.gameObject.SetActive(false);

        Debug.Log("カウントダウンスタート");
        CountDownAnimation.SetTrigger("LetsCount"); //カウントダウンアニメーションのトリガー
    }
}


// --------------------------- このスクリプトの流れ ---------------------------
//
// ▼ 概要
// 2人プレイヤー用の「レディ画面（準備確認）」を管理するスクリプト。
// 各プレイヤーのコントローラー入力を受け取り、両者が準備完了したら
// カウントダウン演出を開始する。
//
// ▼ Start()
// ・接続されているコントローラー(Gamepad.all)を取得。
// ・pad1, pad2 に順番で登録。
// ・それぞれの DarumaManager に該当コントローラーを紐付け。
// ・「Push ○!」という初期メッセージを表示。
//
// ▼ Update()
// ・プレイヤー1が ○ボタン（buttonEast）を押す → player1Ready をトグル。
// 　　→ テキストを「Ready! / Not Ready」で切り替え。
// ・プレイヤー2も同様に ○ボタンでトグル。
// ・両方が Ready 状態になった瞬間に、
// 　　coroutineStarted が false なら一度だけコルーチンを開始。
// 　　→ 2回目以降は起動しないようガード。
//
// ▼ AfterP1P2Ready()
// ・両者準備完了後に呼ばれるコルーチン。
// 　1秒待機してから Ready テキストを非表示。
// 　カウントダウンアニメーションをトリガー（"LetsCount"）。
// 　→ 実際のゲーム開始前の演出を担う部分。
//
// ▼ 補足
// ・コントローラーが1つしか無い場合は1Pのみ操作可能。
// ・TimerCountやDarumaManagerとは連携して、実際のプレイ開始を制御予定。
// ・coroutineStarted フラグでコルーチンの多重起動を防いでいる。
//
// ---------------------------------------------------------------------------

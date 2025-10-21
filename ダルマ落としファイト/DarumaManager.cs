using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro; // ← 追加：テキスト表示用

public class DarumaManager : MonoBehaviour
{
    [Header("ほかのスクリプト参照")]
    [SerializeField] private DarumaSpawner spawner;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private TimerCount timecount;
    [SerializeField] private BuffController buffController; //バフ参照

    [Header("各段プレハブ登録")]
    public GameObject CirclePrefab;
    public GameObject CrossPrefab;
    public GameObject TrianglePrefab;
    public GameObject SquarePrefab;

    [Header("今の段リスト")]　//多分使わん代入するだけ
    public List<GameObject> currentDarumas = new List<GameObject>();

    private Gamepad gamepad;

    [SerializeField] private float inputCooldown = 0.1f; //ボタン押す時間の感覚
    private float lastInputTime = -999f; //最後にボタンを押した時間

    [Header("バフ表示UI")]
    [SerializeField] private Image invincibleIcon; //無敵アイコン
    [SerializeField] private Image barrierIcon;    //バリアアイコン

    [Header("コンボUI")]
    [SerializeField] private TextMeshProUGUI comboText; //コンボ数表示
    [SerializeField] private TextMeshProUGUI multiplierText; //倍率表示

    private int comboCount = 0; //コンボ数
    private float comboMultiplier = 1f; //コンボ倍率

    public void SetGamepad(Gamepad assignedPad) //PlayerManagerからコントローラーを登録する
    {
        gamepad = assignedPad;
    }

    void Update()
    {
        if (gamepad == null) return;
        if (!gamepad.added) return; //それぞれのDarumaManagerに紐づけたコントローラー以外は無視のガード節

        if (Time.time - lastInputTime < inputCooldown) return; //クールダウン中はガード

        bool pressed = false;

        if (gamepad.buttonWest.wasPressedThisFrame) //コントローラーボタン登録
        {
            TryBreakDaruma(SquarePrefab);
            pressed = true;
        }

        if (gamepad.buttonSouth.wasPressedThisFrame)
        {
            TryBreakDaruma(CrossPrefab);
            pressed = true;
        }

        if (gamepad.buttonEast.wasPressedThisFrame)
        {
            TryBreakDaruma(CirclePrefab);
            pressed = true;
        }

        if (gamepad.buttonNorth.wasPressedThisFrame)
        {
            TryBreakDaruma(TrianglePrefab);
            pressed = true;
        }

        if (pressed) //押されたら時間更新
        {
            lastInputTime = Time.time;
        }

        UpdateBuffIcons(); // ← 追加：UI更新
        UpdateComboUI();   // ← 追加：コンボUI更新
    }

    void TryBreakDaruma(GameObject prefab)
    {
        if (!timecount.StartFlag) return;
        if (currentDarumas.Count == 0) return;

        GameObject bottom = currentDarumas[0];

        if (buffController != null && buffController.IsInvincibleActive()) //無敵バフ中はどのボタンでも成功扱いする
        {
            Debug.Log("無敵バフ中：どのボタンでも成功！");
            currentDarumas.RemoveAt(0);
            Destroy(bottom);

            comboCount++; // ← コンボ加算
            UpdateComboMultiplier();

            if (scoreManager != null)
                scoreManager.AddScore(Mathf.RoundToInt(100 * comboMultiplier)); //倍率反映

            if (buffController != null)
                buffController.OnSuccessfulHit();

            spawner.SpawnNewDaruma();
            return;
        }

        if (bottom.name.StartsWith(prefab.name))
        {
            currentDarumas.RemoveAt(0);

            Rigidbody rb = bottom.GetComponent<Rigidbody>();
            Collider col = bottom.GetComponent<Collider>();

            rb.constraints = RigidbodyConstraints.None; //ぶっ飛ばす段だけRigidBodyの制約解除
            rb.isKinematic = false;

            StartCoroutine(TempTrigger(col)); //一瞬だけ当たり判定を消すコルーチン呼ぶ

            rb.AddForce(Vector3.forward * 50f, ForceMode.Impulse); //ぶっ飛ばす力の強さ

            Destroy(bottom, 5f); //後片付け

            comboCount++; //コンボ加算
            UpdateComboMultiplier();

            if (scoreManager != null)
                scoreManager.AddScore(Mathf.RoundToInt(100 * comboMultiplier)); //倍率反映

            if (buffController != null)
                buffController.OnSuccessfulHit(); //成功時バフ抽選

            spawner.SpawnNewDaruma(); //新しい段生成

            foreach (GameObject daruma in currentDarumas) //段を固定
            {
                if (daruma == null) continue;
                Rigidbody r = daruma.GetComponent<Rigidbody>();
                if (r != null)
                {
                    r.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
                }
            }
        }
        else
        {
            if (buffController != null && buffController.TryConsumeBarrier()) //バリア発動チェック
            {
                Debug.Log("バリアによりミス無効化！");
                HandleSuccess(bottom); //成功処理まとめて呼ぶ
                return;
            }

            Debug.Log("外した！");

            comboCount = 0; //コンボ数と倍率リセット
            comboMultiplier = 1f;
        }
    }

    void HandleSuccess(GameObject bottom) //成功時の共通処理上とほぼ同じ
    {
        currentDarumas.RemoveAt(0);

        Rigidbody rb = bottom.GetComponent<Rigidbody>();
        Collider col = bottom.GetComponent<Collider>();

        rb.constraints = RigidbodyConstraints.None;
        rb.isKinematic = false;

        StartCoroutine(TempTrigger(col));
        rb.AddForce(Vector3.forward * 50f, ForceMode.Impulse);
        Destroy(bottom, 5f);

        comboCount++; //コンボ加算
        UpdateComboMultiplier();

        if (scoreManager != null)
            scoreManager.AddScore(Mathf.RoundToInt(100 * comboMultiplier)); //倍率反映

        if (buffController != null)
            buffController.OnSuccessfulHit();

        spawner.SpawnNewDaruma();

        foreach (GameObject daruma in currentDarumas)
        {
            if (daruma == null) continue;
            Rigidbody r = daruma.GetComponent<Rigidbody>();
            if (r != null)
            {
                r.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
            }
        }
    }

    void UpdateComboMultiplier() //コンボ倍率用メソッド
    {
        if (comboCount <= 10) //10以下等倍、11以上30以下1.1倍、31以上50以下1.2倍、51以上1.3倍 あとで帰る可能性大
            comboMultiplier = 1f;
        else if (comboCount <= 30)
            comboMultiplier = 1.1f;
        else if (comboCount <= 50)
            comboMultiplier = 1.2f;
        else
            comboMultiplier = 1.3f;

        Debug.Log($"コンボ: {comboCount}  倍率: {comboMultiplier}"); //一応コンボ数と倍率デバッグ
    }

    void UpdateComboUI() // ← 追加：コンボUI更新
    {
        if (comboText != null)
            comboText.text = $"Combo:{comboCount}"; //コンボ表示

        if (multiplierText != null)
            multiplierText.text = $"×{comboMultiplier:F1}"; //倍率表示 小数1桁まで
    }

    IEnumerator TempTrigger(Collider col) //当たり判定を一瞬消すコルーチン
    {
        if (col == null) yield break;
        col.isTrigger = true;
        yield return new WaitForSeconds(0.1f); //ちょっと待つ
        col.isTrigger = false;
    }

    void UpdateBuffIcons() //バフ状態に応じてUI切り替え いつかなくすかも
    {
        if (buffController == null) return;

        if (invincibleIcon != null)
            invincibleIcon.enabled = buffController.IsInvincibleActive();

        if (barrierIcon != null)
            barrierIcon.enabled = buffController.IsBarrierActive();
    }
}


// --------------------------- このスクリプトの流れ ---------------------------
//
// ▼ 概要
// 達磨落としの操作・判定・スコア・コンボ・バフUIなどをまとめて管理するメインスクリプト。
// プレイヤーごとに1つずつ紐づく。各自のコントローラー入力を受けて達磨を消す。
//
// ▼ 主要な流れ
// ・Start() は使っていない。主に Update() 内で動作。
// ・PlayerManager からコントローラーを登録（SetGamepad）
// ・Update() 内で入力監視 → 該当ボタンが押されたら TryBreakDaruma() 実行
// ・クールダウン中や他プレイヤーの入力は無視
//
// ▼ TryBreakDaruma(GameObject prefab)
// 　1. タイマーが動いていなければ無視
// 　2. 一番下の達磨（currentDarumas[0]）を取得
// 　3. 無敵バフ中なら強制成功 → コンボ加算・倍率更新・スコア加算・新段生成
// 　4. 押したボタンが正解なら：
// 　　　・Rigidbody制約解除 → AddForceで前方へ吹っ飛ばす
// 　　　・当たり判定を一瞬OFFにして再ON
// 　　　・コンボ加算、倍率更新、スコア加算、バフ抽選、新しい段を生成
// 　　　・他の段は固定化
// 　5. 間違えた場合：
// 　　　・バリアがあればミス無効 → HandleSuccess() 呼び出し
// 　　　・なければコンボと倍率をリセット
//
// ▼ HandleSuccess(GameObject bottom)
// 　成功時処理の共通化メソッド（バリアなどで呼ばれる）
// 　TryBreakDaruma 内の成功処理とほぼ同じ。
// 　Rigidbody解除 → AddForce → コンボ加算・スコア加算・新段生成 → 他の段を固定。
//
// ▼ UpdateComboMultiplier()
// 　コンボ数に応じて倍率を決定（今は4段階：1.0 / 1.1 / 1.2 / 1.3）
// 　倍率変更時にデバッグ出力。
//
// ▼ UpdateComboUI()
// 　UIテキスト更新（Combo:XX と ×1.X 表示）
//
// ▼ UpdateBuffIcons()
// 　BuffController の状態を見てアイコンON/OFF切り替え。
// 　無敵・バリア両方チェック。
//
// ▼ TempTrigger(Collider col)
// 　一瞬だけ isTrigger = true にして衝突防止するコルーチン。
// 　吹っ飛ばす瞬間に他の段に引っかからないようにする。
//
// ▼ その他
// ・コンボ・スコア・UI・バフすべて Update() 内のループで逐次更新。
// ・一連の処理を通じてゲーム進行と視覚的フィードバックを同期させている。
//
// ---------------------------------------------------------------------------


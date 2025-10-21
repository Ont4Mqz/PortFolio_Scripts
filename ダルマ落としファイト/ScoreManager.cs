using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [Header("UI参照")]
    [SerializeField] private TextMeshProUGUI scoreText;

    public static int score = 0; //スコアを持ち越すためにstatic化しました

    void Start()
    {
        UpdateScoreUI();
    }

    public void AddScore(int points) //DarumaManagerから呼ばれる
    {
        score += points;
        UpdateScoreUI();
    }

    private void UpdateScoreUI() //UIに反映する
    {
        if (scoreText != null)
            scoreText.text = $"Score: {score}";
    }

    public void ResetScore() //Scoreリセット
    {
        score = 0;
        UpdateScoreUI();
    }
}


// --------------------------- このスクリプトの流れ ---------------------------
//
// ▼ 概要
// ゲーム全体のスコア管理を行うスクリプト。
// スコア加算、リセット、UI更新の3つの役割を持つ。
// static変数を利用してシーンをまたいでもスコアを保持できるようになっている。
//
// ▼ Start()
// ・ゲーム開始時に一度だけ UpdateScoreUI() を呼んでスコア表示を初期化。
//
// ▼ AddScore(int points)
// ・DarumaManager などから呼ばれるスコア加算メソッド。
// ・指定された points を static な score に加算し、UIを更新。
//
// ▼ UpdateScoreUI()
// ・scoreText がアタッチされていれば、現在のスコアを "Score: XXX" の形で表示。
// ・UI反映専用の内部メソッド（外部からは直接呼ばない）。
//
// ▼ ResetScore()
// ・スコアを0に戻して、UIにも反映。
// ・ゲームリスタートやリトライ時に呼ばれる想定。
//
// ▼ 補足
// ・static score のため、シーンを切り替えても値が保持される（手動でリセットが必要）。
// ・スコアのUI参照はインスペクタ上で設定。
// ・単純な仕組みだが、他のシステム（コンボ・倍率など）と組み合わせて柔軟に拡張可能。
//
// ---------------------------------------------------------------------------

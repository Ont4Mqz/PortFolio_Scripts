using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem.Controls;

public class DarumaManager : MonoBehaviour
{
    [Header("ほかのスクリプト参照")]
    [SerializeField] private DarumaSpawner spawner; // 自分用スポーナー参照
    [SerializeField] private ScoreManager scoreManager; // スコア管理参照
    [SerializeField] private TimerCount timecount;  // タイマー参照
    [SerializeField] private BuffController buffController; // 自分のバフ管理
    [SerializeField] private DebuffController debuffController; // 相手プレイヤーのデバフ管理
    [SerializeField] private DarumaManager enemyDaruma;     //敵のマネージャー参照

    [Header("各段プレハブ登録")]
    public GameObject CirclePrefab; // 丸
    public GameObject CrossPrefab; // バツ
    public GameObject TrianglePrefab; // 三角
    public GameObject SquarePrefab; // 四角
    [Header("矢印ボタンを使うブロック")]
    public List<GameObject> ArrowPrefabs; // 矢印ボタン用プレハブリスト
    [Header("今の段リスト")]
    public List<GameObject> currentDarumas = new List<GameObject>(); // 今積んでいる達磨リスト
    [Header("同時押しブロック"), SerializeField] private GameObject doublePressBlock;
    [SerializeField] private float inputCooldown = 0.1f; // 入力のクールダウン時間
    private float lastInputTime = -999f; // 最後に入力した時間

    [Header("バフ表示UI")]
    [SerializeField] private Image invincibleIcon; // 無敵アイコン
    [SerializeField] private Image barrierIcon; // バリアアイコン
    [SerializeField] private Image freezeIcon; // フリーズアイコン

    [Header("キャラクター画像")]
    [SerializeField] private Image characterImage; // キャラクターの立ち絵
    [SerializeField] private List<Sprite> joySprites = new List<Sprite>(); // 喜び画像リスト (1,2,3)
    [SerializeField] private Sprite debuffSprite; // デバフ用立ち絵
    [SerializeField] private float debuffImageDuration = 1.0f; // デバフ画像表示時間

    [Header("コンボUI")]
    [SerializeField] private List<Image> comboDigitImages; // コンボ数の桁ごとのImage
    [SerializeField] private TextMeshProUGUI comboCountText; // コンボ数表示（テキスト版）
    [SerializeField] private bool useTextComboDisplay = true; // テキストでコンボ数を表示するか
    [SerializeField] private bool useImageComboDisplay = true; // 画像でコンボ数を表示するか
    [SerializeField] private TextMeshProUGUI multiplierText; // コンボ倍率表示（テキスト版、画像版の場合はnullOK）
    [SerializeField] private List<Image> multiplierDigitImages; // コンボ倍率の桁ごとのImage（x1.0などを画像で表示）
    [SerializeField] private List<Sprite> numberSprites; // 0-9の数字スプライト

    [Header("スコア数字UI")]
    [SerializeField] private List<Image> scoreDigitImages; // スコアの桁ごとのImage

    [Header("プレイヤー情報")]
    [SerializeField] private int playerID; // プレイヤーID(1か2)

    private int comboCount = 0;
    private float comboMultiplier = 1f;
    private bool isInputLocked = false; // デバフ中の入力ロック
    private bool autoSpawnEnabled = true; // 最終局面で自動生成を止めるためのフラグ
    private Gamepad gamepad;

    private int currentJoyIndex = 0; // 現在の喜び画像インデックス
    private Sprite previousJoySprite; // デバフ前に表示されていた画像
    private bool wasInvincibleActive = false; // 前回の無敵状態
    private bool wasBarrierActive = false; // 前回のバリア状態
    [SerializeField] private AudioClip[] SE_hits;
    private AudioClip SE_hit;
    private float counttime = 0.5f;
    bool leftDown = false;
    bool rightDown = false;
    private List<ButtonControl> dpadButtons = new();
    private bool isCorrect = false;

    private ButtonControl _rightButton;
    private ButtonControl _leftButton;
    AudioSource audioSource;


    private void Awake()
    {
        debuffController = gameObject.GetComponent<DebuffController>();

        if (characterImage != null && joySprites.Count > 0)
        {
            characterImage.sprite = joySprites[0];
            previousJoySprite = joySprites[0];
        }
        var gamepad = Gamepad.current;
        audioSource = GameObject.Find("BGMManager").GetComponent<AudioSource>();
        debuffController = gameObject.GetComponent<DebuffController>();
        SE_hit = SE_hits[0];
        if (gamepad != null)
        {
            dpadButtons = new List<ButtonControl>
            {
                gamepad.dpad.up,
                gamepad.dpad.down,
                gamepad.dpad.left,
                gamepad.dpad.right
            };
        }
    }
    public void SetGamepad(Gamepad assignedPad) // PlayerManagerからコントローラーを登録
    {
        gamepad = assignedPad;
    }
    public void PressButton(ButtonControl rightButton, ButtonControl leftButton)
    {
        _rightButton = rightButton;
        _leftButton = leftButton;
    }

    void Update()
    {
        if (Time.time - lastInputTime < inputCooldown) return; // クールダウン中は無視
        if (isInputLocked) return; // フリーズ中は入力無効
        if ( currentDarumas.Count == 0) return;


        bool pressed = false;

#if UNITY_EDITOR
        var kb = Keyboard.current;
        if (kb != null)
        {
            if (playerID == 1)
            {
                if (kb.aKey.wasPressedThisFrame)
                {
                    TryBreakDaruma(SquarePrefab, debuffController.obstacleBlockPrefab[2], debuffController.BleachingObj[2]);
                    pressed = true;
                }
                if (kb.sKey.wasPressedThisFrame)
                {
                    TryBreakDaruma(CrossPrefab, debuffController.obstacleBlockPrefab[1], debuffController.BleachingObj[1]);
                    pressed = true;
                }
                if (kb.dKey.wasPressedThisFrame)
                {
                    TryBreakDaruma(CirclePrefab, debuffController.obstacleBlockPrefab[0], debuffController.BleachingObj[0]);
                    pressed = true;
                }
                if (kb.wKey.wasPressedThisFrame)
                {
                    TryBreakDaruma(TrianglePrefab, debuffController.obstacleBlockPrefab[3], debuffController.BleachingObj[3]);
                    pressed = true;
                }
            }
        
            else if (playerID == 2)
            {
                if (kb.leftArrowKey.wasPressedThisFrame)
                {
                    TryBreakDaruma(SquarePrefab, debuffController.obstacleBlockPrefab[2], debuffController.BleachingObj[2]);
                    pressed = true;
                }
                if (kb.downArrowKey.wasPressedThisFrame)
                {
                    TryBreakDaruma(CrossPrefab, debuffController.obstacleBlockPrefab[1], debuffController.BleachingObj[1]);
                    pressed = true;
                }
                if (kb.rightArrowKey.wasPressedThisFrame)
                {
                    TryBreakDaruma(CirclePrefab, debuffController.obstacleBlockPrefab[0], debuffController.BleachingObj[0]);
                    pressed = true;
                }
                if (kb.upArrowKey.wasPressedThisFrame)
                {
                    TryBreakDaruma(TrianglePrefab, debuffController.obstacleBlockPrefab[3], debuffController.BleachingObj[3]);
                    pressed = true;
                }
            }
        }
#endif

        if (gamepad != null) //コントローラーボタン登録
        {
            if (_leftButton != null && _rightButton != null)
            {
                foreach (var button in dpadButtons)
                {
                    if (button.wasPressedThisFrame)
                    {
                        JudgeButton(button);
                    }
                }

                foreach (var control in gamepad.allControls)
                {
                    if (control is ButtonControl button)
                    {
                        if (button.wasPressedThisFrame)
                        {
                            JudgeButton(button);
                        }
                    }
                }
            }

            else
            {
                if (gamepad.buttonWest.wasPressedThisFrame)
                {
                    TryBreakDaruma(SquarePrefab, debuffController.obstacleBlockPrefab[2], debuffController.BleachingObj[2]);
                    pressed = true;
                }

                if (gamepad.buttonSouth.wasPressedThisFrame)
                {
                    TryBreakDaruma(CrossPrefab, debuffController.obstacleBlockPrefab[1], debuffController.BleachingObj[1]);
                    pressed = true;
                }

                if (gamepad.buttonEast.wasPressedThisFrame)
                {
                    TryBreakDaruma(CirclePrefab, debuffController.obstacleBlockPrefab[0], debuffController.BleachingObj[0]);
                    pressed = true;
                }

                if (gamepad.buttonNorth.wasPressedThisFrame)
                {
                    TryBreakDaruma(TrianglePrefab, debuffController.obstacleBlockPrefab[3], debuffController.BleachingObj[3]);
                    pressed = true;
                }

                if (DifficultyLevel.normal || DifficultyLevel.hard)
                {
                    if (gamepad.dpad.up.wasPressedThisFrame)
                    {
                        TryBreakDaruma(ArrowPrefabs[3], ArrowPrefabs[3], ArrowPrefabs[3]);
                        pressed = true;
                    }
                    if (gamepad.dpad.down.wasPressedThisFrame)
                    {
                        TryBreakDaruma(ArrowPrefabs[0], ArrowPrefabs[0], ArrowPrefabs[0]);
                        pressed = true;
                    }
                    if (gamepad.dpad.left.wasPressedThisFrame)
                    {
                        TryBreakDaruma(ArrowPrefabs[1], ArrowPrefabs[1], ArrowPrefabs[1]);
                        pressed = true;
                    }
                    if (gamepad.dpad.right.wasPressedThisFrame)
                    {
                        TryBreakDaruma(ArrowPrefabs[2], ArrowPrefabs[2], ArrowPrefabs[2]);
                        pressed = true;
                    }
                }
            }
        }

        if (pressed)
            lastInputTime = Time.time; // 入力時間を更新

        UpdateBuffIcons();
        UpdateComboUI();
    }
    void JudgeButton(ButtonControl pressed)
    {
        isCorrect = false;

        counttime -= Time.deltaTime;
        if (counttime > 0)
        {

            if (pressed.name == _leftButton.name)
            {
                leftDown = true;
                isCorrect = true;
            }
            else if (pressed.name == _rightButton.name)
            {
                rightDown = true;
                isCorrect = true;
            }

            if (isCorrect)
            {
                counttime = 0.5f;
            }
            else
            {
                Debug.Log("惜しい！");
                comboCount = 0;
                comboMultiplier = 1f;
                counttime = 0.5f;
                rightDown = false;
                leftDown = false;
            }
        }
        else
        {
            Debug.Log("外した！");
            comboCount = 0;
            comboMultiplier = 1f; // コンボと倍率をリセット
            counttime = 0.5f;
        }
        if (rightDown && leftDown)
        {
            HandleSuccess(currentDarumas[0]);
            rightDown = false;
            leftDown = false;
            counttime = 0.5f;
            _leftButton = null;
            _rightButton = null;
        }
    }

    void TryBreakDaruma(GameObject prefab, GameObject prefab2, GameObject prefab3)
    {
        if (currentDarumas.Count == 0) return; // 達磨がなければ無視
        if (timecount.StartFlag == false) return;

        GameObject bottom = currentDarumas[0]; // 一番下の達磨

        if (buffController != null && buffController.IsInvincibleActive()) // 無敵バフ中なら強制成功
        {
            HandleSuccess(bottom);
            return;
        }

        if (bottom.CompareTag("ShinyDaruma")) // ShinyDarumaはどのボタンでも飛ばせる
        {
            Debug.Log("キラキラ達磨を飛ばした！");
            HandleSuccess(bottom);
            if (buffController != null)
                buffController.ActivateRandomBuffFromShiny(); // ShinyDarumaをたたいた場合のみ確定でバフを付与(2種類からランダム)
            return;
        }

        if (bottom.name.StartsWith(prefab.name)
            || bottom.name.StartsWith(prefab2.name)
            || bottom.name.StartsWith(prefab3.name)) // 正解のボタンを押した
        {
            HandleSuccess(bottom);

            if (bottom.CompareTag("DebuffDaruma")) // DebuffDarumaをたたいた場合のみ確定で相手をフリーズ
            {
                Debug.Log("デバフ達磨を飛ばした！");
                if (debuffController != null)
                {
                        debuffController.ReceiveDebuff();
                }
            }
            // 普通の達磨をたたいた場合はバフもデバフも発生しない
        }
        else // 間違えたボタンを押した
        {
            if (buffController != null && buffController.IsBarrierActive()) // バリアがあればミス無効
            {
                buffController.TryConsumeBarrier();
                HandleSuccess(bottom);
                return;
            }

            Debug.Log("外した！");
            comboCount = 0;
            comboMultiplier = 1f; // コンボと倍率をリセット
        }
    }

    void HandleSuccess(GameObject bottom)
    {
        Debug.Log("達磨を飛ばした！");
        if(currentDarumas[0].tag == "DebuffDaruma")
        {
            audioSource.PlayOneShot(SE_hit);
               if(Random.value > 0.25f)
               {
                    AddBleachingBlocks(5);
               }
               else AddObstacleBlocks(5);

               if (enemyDaruma != null) // 相手がデバフ達磨を飛ばしたので、自分の画像をデバフ画像に一瞬切り替え
               {
                   enemyDaruma.OnEnemyHitDebuffDaruma();
               }
        }
        currentDarumas.RemoveAt(0); // リストから削除

        Rigidbody rb = bottom.GetComponent<Rigidbody>();
        Collider col = bottom.GetComponent<Collider>();

        rb.constraints = RigidbodyConstraints.None; // 物理制約を解除
        rb.isKinematic = false;

        StartCoroutine(TempTrigger(col));
        rb.AddForce(Vector3.forward * 50f, ForceMode.Impulse); // 前方へ吹っ飛ばす
        Destroy(bottom, 5f);

        comboCount++;
        UpdateComboMultiplier();

        if (!bottom.CompareTag("Obstacle"))      //お邪魔ブロックでなければスコア加算＆新しい段を生成する
        {
            if (scoreManager != null)
                scoreManager.AddScore(playerID, Mathf.RoundToInt(100 * comboMultiplier)); // 自分のIDに応じて加算

            if (!bottom.CompareTag("BleachingBlock") && autoSpawnEnabled)
                spawner.SpawnNewDaruma(); // 新しい段を生成

            foreach (GameObject daruma in currentDarumas) // 他の段を固定
            {
                if (daruma == null) continue;
                Rigidbody r = daruma.GetComponent<Rigidbody>();
                if (r != null)
                {
                    r.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
                }
            }
        }
    }

    void UpdateComboMultiplier() // コンボ数に応じて倍率を決定
    {
        if (comboCount <= 10)
        {
            comboMultiplier = 1f;
            SE_hit = SE_hits[0];
        }
        else if (comboCount <= 30)
        {
            comboMultiplier = 1.1f;
            SE_hit = SE_hits[0];
        }
        else if (comboCount <= 50)
            {comboMultiplier = 1.1f;
            SE_hit = SE_hits[1];
        }
        else
            { comboMultiplier = 1.3f;
            SE_hit = SE_hits[2];
        }
    }

    void UpdateComboUI() // コンボUIを更新
    {
        // コンボ数を画像で表示
        if (useImageComboDisplay)
        {
            SetNumberSprites(comboCount, comboDigitImages);
        }
        else
        {
            // 画像表示を無効にする場合、Image要素を非表示に
            if (comboDigitImages != null)
                foreach (var img in comboDigitImages)
                    if (img != null) img.enabled = false;
        }

        // コンボ数をテキストで表示
        if (useTextComboDisplay && comboCountText != null)
        {
            comboCountText.text = comboCount.ToString();
        }
        else if (!useTextComboDisplay && comboCountText != null)
        {
            comboCountText.text = "";
        }

        // コンボ倍率をテキストまたは画像で表示
        if (multiplierText != null)
            multiplierText.text = $"×{comboMultiplier:F1}";

        // コンボ倍率を画像で表示する場合（小数点第1位まで）
        if (multiplierDigitImages != null && multiplierDigitImages.Count > 0)
        {
            string multiplierStr = comboMultiplier.ToString("F1"); // "1.1", "1.2" など
            SetNumberSpritesWithDecimal(multiplierStr, multiplierDigitImages);
        }

        // スコアを画像で表示
        if (scoreDigitImages != null && scoreDigitImages.Count > 0 && scoreManager != null)
        {
            int currentScore = (playerID == 1) ? ScoreManager.Player1Score : ScoreManager.Player2Score;
            SetNumberSprites(currentScore, scoreDigitImages);
        }
    }

    IEnumerator TempTrigger(Collider col) // 一瞬だけisTriggerをtrueにして衝突防止
    {
        if (col == null) yield break;
        col.isTrigger = true;
        yield return new WaitForSeconds(0.1f);
        col.isTrigger = false;
    }

    void UpdateBuffIcons() // バフアイコンの表示更新
    {
        if (buffController == null) return;

        bool isInvincibleActive = buffController.IsInvincibleActive();
        bool isBarrierActive = buffController.IsBarrierActive();

        if ((isInvincibleActive && !wasInvincibleActive) || (isBarrierActive && !wasBarrierActive)) // バフが新しくアクティブになったら喜び画像を切り替え
        {
            SwitchToNextJoySprite();
        }

        wasInvincibleActive = isInvincibleActive;
        wasBarrierActive = isBarrierActive;

        if (invincibleIcon != null)
            invincibleIcon.enabled = isInvincibleActive;

        if (barrierIcon != null)
            barrierIcon.enabled = isBarrierActive;

        if (freezeIcon != null)
            freezeIcon.enabled = isInputLocked;
    }

    void SwitchToNextJoySprite()
    {
        if (characterImage == null || joySprites.Count == 0) return;

        currentJoyIndex = (currentJoyIndex + 1) % joySprites.Count;
        characterImage.sprite = joySprites[currentJoyIndex];
        previousJoySprite = joySprites[currentJoyIndex];
    }

    void SetNumberSprites(int number, List<Image> digitImages)
    {
        if (digitImages == null || numberSprites == null || numberSprites.Count < 10) return;

        string numStr = number.ToString();
        int startIndex = Mathf.Max(0, digitImages.Count - numStr.Length);

        // 余った桁を0で埋める
        for (int i = 0; i < startIndex; i++)
        {
            if (digitImages[i] != null)
                digitImages[i].sprite = numberSprites[0];
        }

        // 数字を設定
        for (int i = 0; i < numStr.Length && startIndex + i < digitImages.Count; i++)
        {
            int digit = numStr[i] - '0';
            if (digit >= 0 && digit <= 9 && digitImages[startIndex + i] != null)
            {
                digitImages[startIndex + i].sprite = numberSprites[digit];
            }
        }

        // 余った桁を非表示
        for (int i = startIndex + numStr.Length; i < digitImages.Count; i++)
        {
            if (digitImages[i] != null)
                digitImages[i].enabled = false;
        }
    }

    void SetNumberSpritesWithDecimal(string numberStr, List<Image> digitImages)
    {
        if (digitImages == null || numberSprites == null || numberSprites.Count < 11) return; // 0-9 + dot

        // 例："1.1" を処理する場合、スプライトの配置を工夫する必要がある
        // ここでは "1.1" → [1, dot, 1] という想定で実装
        int dotIndex = -1; // numberSprites リストに "dot" (小数点) スプライトがあるなら設定
        
        List<string> parts = new List<string>(numberStr.Split('.'));
        
        if (parts.Count == 2)
        {
            // "1.1" の場合: parts[0]="1", parts[1]="1"
            string intPart = parts[0];
            string decPart = parts[1];

            // 左側に整数部分、小数点、右側に小数部分を配置
            int currentIndex = 0;
            
            // 整数部分を左詰めで配置
            int intStartIndex = Mathf.Max(0, digitImages.Count - intPart.Length - 1 - decPart.Length);
            for (int i = 0; i < intStartIndex; i++)
            {
                if (digitImages[i] != null)
                    digitImages[i].sprite = numberSprites[0];
            }

            // 整数部分のスプライト設定
            for (int i = 0; i < intPart.Length; i++)
            {
                int digit = intPart[i] - '0';
                if (digit >= 0 && digit <= 9 && intStartIndex + i < digitImages.Count && digitImages[intStartIndex + i] != null)
                {
                    digitImages[intStartIndex + i].sprite = numberSprites[digit];
                }
                currentIndex = intStartIndex + i + 1;
            }

            // 小数部分のスプライト設定（小数点記号が必要なら別途実装）
            for (int i = 0; i < decPart.Length; i++)
            {
                int digit = decPart[i] - '0';
                if (digit >= 0 && digit <= 9 && currentIndex + i < digitImages.Count && digitImages[currentIndex + i] != null)
                {
                    digitImages[currentIndex + i].sprite = numberSprites[digit];
                }
            }

            // 余った桁を非表示
            int filledCount = intStartIndex + intPart.Length + decPart.Length;
            for (int i = filledCount; i < digitImages.Count; i++)
            {
                if (digitImages[i] != null)
                    digitImages[i].enabled = false;
            }
        }
        else
        {
            // 小数点がない場合は通常の数字表示
            SetNumberSprites(int.Parse(numberStr), digitImages);
        }
    }

    public void OnEnemyHitDebuffDaruma() // 相手がデバフ達磨を飛ばしたときに呼ばれる
    {
        if (characterImage != null && debuffSprite != null)
        {
            StartCoroutine(ShowDebuffImage());
        }
    }

    private IEnumerator ShowDebuffImage()
    {
        Sprite currentSprite = characterImage.sprite;
        characterImage.sprite = debuffSprite;
        yield return new WaitForSeconds(debuffImageDuration);
        characterImage.sprite = currentSprite; // 現在の画像に戻す（previousJoySpriteではなく現在の）
    }

    public void SetInputLock(bool locked) // DebuffControllerから入力ロックを制御するための公開メソッド
    {
        isInputLocked = locked;

        if (characterImage != null)
        {
            if (locked && debuffSprite != null)
            {
                // デバフ中はデバフ画像に切り替え
                characterImage.sprite = debuffSprite;
            }
            else if (!locked)
            {
                // デバフ解除時は前に表示されていた喜び画像に戻す
                characterImage.sprite = previousJoySprite;
            }
        }

        UpdateBuffIcons();
    }

    public void SetSpawner(DarumaSpawner newSpawner) // スポーナー参照を設定
    {
        spawner = newSpawner;
    }

    public void SetAutoSpawnEnabled(bool enabled) // 自動生成の有効/無効を設定
    {
        autoSpawnEnabled = enabled;
    }

    public void AddObstacleBlocks(int count) //お邪魔ブロック追加
    {
        if (enemyDaruma.currentDarumas.Count < 15) //最大数制限
            for (int i = 0; i < count; i++)
                debuffController.SpawnObstacleBlock();

        else return;

    }
    public void AddBleachingBlocks(int count) //漂白ブロック追加
    {
        if (enemyDaruma.currentDarumas.Count < 15)
            for (int i = 0; i < count; i++)
                debuffController.SpawnBleachingBlock();

        else return;
    }

}
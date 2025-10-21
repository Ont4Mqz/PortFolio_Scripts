using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonManagerHamasaki : MonoBehaviour
{
    public Button button1;                // ボタン1
    public Button button2;                // ボタン2
    public Button newButton;              // 新しく表示するボタン
    public GameObject objectToShow1;      // ボタン1を押した後に出現させるオブジェクト
    public GameObject objectToShow2;      // ボタン2を押した後に出現させるオブジェクト
    public string nextSceneName;          // 遷移先のシーン名

    public Vector3 targetPosition1;       // オブジェクト1の移動先
    public Vector3 targetPosition2;       // オブジェクト2の移動先
    public float moveSpeed = 1f;          // 移動速度

    private bool button1Pressed = false;
    private bool button2Pressed = false;

    void Start()
    {
        // 初期設定：オブジェクトと新しいボタンを非表示にしておく
        objectToShow1.SetActive(false);
        objectToShow2.SetActive(false);
        newButton.gameObject.SetActive(false);

        // ボタンのリスナーを設定
        button1.onClick.AddListener(OnButton1Clicked);
        button2.onClick.AddListener(OnButton2Clicked);
        newButton.onClick.AddListener(OnNewButtonClicked);
    }

    void OnButton1Clicked()
    {
        button1.gameObject.SetActive(false);   // ボタン1を非表示
        objectToShow1.SetActive(true);         // オブジェクト1を表示
        button1Pressed = true;
        CheckIfBothButtonsPressed();
    }

    void OnButton2Clicked()
    {
        button2.gameObject.SetActive(false);   // ボタン2を非表示
        objectToShow2.SetActive(true);         // オブジェクト2を表示
        button2Pressed = true;
        CheckIfBothButtonsPressed();
    }

    void CheckIfBothButtonsPressed()
    {
        if (button1Pressed && button2Pressed)
        {
            // 新しいボタンを表示
            newButton.gameObject.SetActive(true);
        }
    }

    void OnNewButtonClicked()
    {
        // 新しいボタンが押されたときにシーンを遷移
        SceneManager.LoadScene(nextSceneName);
    }

    void Update()
    {
        // オブジェクト1が表示されている場合、移動させる
        if (button1Pressed && objectToShow1.activeSelf)
        {
            objectToShow1.transform.position = Vector3.MoveTowards(
                objectToShow1.transform.position,
                targetPosition1,
                moveSpeed * Time.deltaTime);
        }

        // オブジェクト2が表示されている場合、移動させる
        if (button2Pressed && objectToShow2.activeSelf)
        {
            objectToShow2.transform.position = Vector3.MoveTowards(
                objectToShow2.transform.position,
                targetPosition2,
                moveSpeed * Time.deltaTime);
        }
    }
}

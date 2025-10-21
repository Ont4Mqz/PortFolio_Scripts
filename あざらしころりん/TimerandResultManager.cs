using UnityEngine;

public class TimerandResultManager : MonoBehaviour
{
    public static TimerandResultManager Instance { get; private set; }

    // タイマー情報
    public int minutes = 5;
    public int seconds = 49;

    // リザルト情報
    public static int SelectedCategoryIndex = 0; // 0:Result1, 1:Result2, 2:Result3
    public static string SelectedRating = "A";    // "A", "B", "C", "D"

    // プレハブなどにアタッチする場合
    public RatingObjectSwitcher ratingObjectSwitcher;

    [SerializeField]
    private GameObject effectObj;

    private void Awake()
    {
        //変更した(中村)
        //2匹ともゴールしたら
        if (GameManager.IsResultGoal == true)
        {
            //リザルトのアザラシの表記は二匹いる(元気)
            SelectedCategoryIndex = 0;
            //時間評価
            if (TimerManager.Min == 0 && TimerManager.Sec <= 35) SelectedRating = "A";
            else if (TimerManager.Min == 0 && TimerManager.Sec >  35) SelectedRating = "B";
            else if (TimerManager.Min == 1 && TimerManager.Sec <= 30) SelectedRating = "B";
            else if(TimerManager.Min == 1 && TimerManager.Sec > 30) SelectedRating = "C";
            else if (TimerManager.Min >= 2) SelectedRating = "C";
            GameManager.IsResultGoal = false;
            GameManager.IsGoalTime = false;
            //エフェクトを流す
            effectObj.SetActive(true);
            //SE再生
            SeManager.Instance.PlaySE(16);
        }

        //数秒空けても二匹目がゴールしなかったら
        if (GameManager.IsResultOne == true)
        {
            //リザルトのアザラシの表記は一匹のみ
            SelectedCategoryIndex = 1;
            //時間評価
            SelectedRating = "D";
            GameManager.IsResultOne = false;
            //エフェクトを止める
            effectObj.SetActive(false);
        }

        //どちらかが落ちたら
        if (GameManager.IsResultFell == true)
        {
            //リザルトのアザラシの表記は二匹いる(つぶれてる)
            SelectedCategoryIndex = 2;
            //時間評価
            SelectedRating = "D";
            GameManager.IsResultFell = false;
            //エフェクトを止める
            effectObj.SetActive(false);
            //SE再生
            SeManager.Instance.PlaySE(15);
        }
    }

    // タイマーを設定するメソッド
    public void SetTime(int newMinutes, int newSeconds)
    {
        minutes = newMinutes;
        seconds = newSeconds;
    }

    // リザルトの選択を設定するメソッド
    public void SetCategoryAndRating(int categoryIndex, string rating)
    {
        SelectedCategoryIndex = categoryIndex;
        SelectedRating = rating;
    }
}

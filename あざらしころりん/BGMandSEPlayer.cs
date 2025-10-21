using UnityEngine;

public class BGMandSEPlayer : MonoBehaviour
{
    void Start()
    {
        // BGMのインデックス（bgmListの順番）、ボリューム、ループ設定
        BgmManager.Instance.PlayBGM(3, 0.15f, true); // BGMリストの0番目を再生

        SeManager.Instance.PlaySE(8, 0.15f, false);
    }
}

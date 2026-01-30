using System.Collections;
using UnityEngine;

public class BGMManager2 : MonoBehaviour
{
    [Header("Audio Clips")]
    [SerializeField] private AudioClip bgmNormal;   // 最初のBGM
    [SerializeField] private AudioClip bgmHurry;    // 残り10秒で切り替えるBGM

    [Header("Fade Settings")]
    [SerializeField] private float fadeTime = 1.0f; // フェードイン・アウトの時間

    private AudioSource audioSource; // AudioSourceコンポーネント
    private TimerCount timer; // TimerCountスクリプトの参照
    private bool switched = false; // BGMが切り替わったかどうかのフラグ

    void Start() // 初期化
    {
        audioSource = GetComponent<AudioSource>();
        timer = FindObjectOfType<TimerCount>();

        audioSource.clip = bgmNormal;
        audioSource.loop = true;
    }

    public void BGMStart()
    {
         audioSource.Play();
    }
    void Update() // 毎フレームチェック
    {
        if (timer == null) return; // TimerCountが見つからない場合は終了
        if (switched) return; // 既に切り替わっている場合は終了
        //if (timer.StartFlag) return;
        if (timer._timerCount <= 10f) // 残り10秒でBGM切り替え
        {
            switched = true;
            StartCoroutine(SwitchBGM());
        }
    }

    private IEnumerator SwitchBGM() // BGM切り替えのコルーチン
    {
        float startVolume = audioSource.volume;

        for (float t = 0; t < fadeTime; t += Time.deltaTime) // フェードアウト
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0f, t / fadeTime);
            yield return null;
        }

        audioSource.volume = 0f;
        audioSource.Stop();

        audioSource.clip = bgmHurry; // 新しいBGMに切り替え
        audioSource.Play();

        for (float t = 0; t < fadeTime; t += Time.deltaTime) // フェードイン
        {
            audioSource.volume = Mathf.Lerp(0f, startVolume, t / fadeTime);
            yield return null;
        }

        audioSource.volume = startVolume;
    }
}

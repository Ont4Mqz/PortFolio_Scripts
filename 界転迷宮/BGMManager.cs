using UnityEngine;

public class BGMManager : MonoBehaviour
{
    [Header("再生するBGM")]
    [SerializeField] private AudioClip bgmClip;

    [Header("オプション")]
    [SerializeField] private bool loop = true;

    private AudioSource audioSource;

    void Awake()
    {
        // AudioSourceを取得または追加
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        // AudioSourceの設定
        audioSource.clip = bgmClip;
        audioSource.loop = loop;
        audioSource.playOnAwake = false;
        audioSource.volume = 0.5f; // 任意調整

        // BGMを再生
        if (bgmClip != null)
            audioSource.Play();
    }
}

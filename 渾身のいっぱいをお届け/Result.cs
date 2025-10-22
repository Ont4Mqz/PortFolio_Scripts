using UnityEngine;
using UnityEngine.UI;

public class Result : MonoBehaviour
{
    [SerializeField] private Image rankImage;

    [Header("ランクごとのスプライト")]
    [SerializeField] private Sprite spriteS;
    [SerializeField] private Sprite spriteA;
    [SerializeField] private Sprite spriteB;

    void Start()
    {
        float score = ScoreManager.Instance != null ? ScoreManager.Instance.Score : 0f;

        if (score >= 70f && score <= 100f)
        {
            SetRank(spriteS);
        }
        else if (score >= 30f && score <= 69f)
        {
            SetRank(spriteA);
        }
        else // 0~29
        {
            SetRank(spriteB);
        }
    }

    private void SetRank(Sprite sprite)
    {
        if (rankImage != null && sprite != null)
        {
            rankImage.sprite = sprite;
        }
    }
}

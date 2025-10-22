using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public float Score { get; private set; }

    [SerializeField] private GameObject gameOverPanel; // �Q�[���I�[�o�[�p�l��
    [SerializeField] private string gameOverTag = "Enemy"; // �Q�[���I�[�o�[�p�R���C�_�[�̃^�O

    private bool isGameOver = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        // Player�̃X�R�A�𖈃t���[���擾
        Score = Player.GetScore();

        // �X�R�A��0�ȉ��ɂȂ�����Q�[���I�[�o�[
        if (!isGameOver && Score <= 0)
        {
            TriggerGameOver();
        }


    }

    private void TriggerGameOver()
    {
        isGameOver = true;
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            Invoke(nameof(ToTitle), 3.5f);
        }
    }
    void ToTitle()
    {
        SceneManager.LoadScene("TitleScene");

    }

    // �R���C�_�[�ɓ���������Q�[���I�[�o�[�i���̃X�N���v�g���A�^�b�`���ꂽ�I�u�W�F�N�g��Collider2D���K�v�j
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isGameOver && collision.CompareTag(gameOverTag))
        {
            TriggerGameOver();
        }
    }
    public bool IsGameOver()
    {
        return isGameOver;
    }

}
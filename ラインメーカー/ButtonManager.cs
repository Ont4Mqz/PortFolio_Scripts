using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    // �������I�����̃{�^��
    public Button[] choiceButtons; // 8�̃{�^���i�C���X�y�N�^�[�Őݒ�\�j
    public GameObject[] objectsToDestroy; // �����I�u�W�F�N�g�i�����I���\�j
    public int correctButtonIndex; // �������{�^���̃C���f�b�N�X

    private void Start()
    {
        // �{�^���Ƀ��X�i�[��ǉ�
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            int index = i; // ���[�J���ϐ��ɕۑ�
            choiceButtons[i].onClick.AddListener(() => OnButtonClicked(index));
        }
    }

    private void OnButtonClicked(int buttonIndex)
    {
        if (buttonIndex == correctButtonIndex)
        {
            // �������I�����������ꂽ�ꍇ
            foreach (GameObject obj in objectsToDestroy)
            {
                Destroy(obj); // �I�u�W�F�N�g������
            }
            Debug.Log("�������I�������I�΂�܂����I");
        }
        else
        {
            // �Ԉ�����I�����������ꂽ�ꍇ
            Debug.Log("�Ԉ�����I�������I�΂�܂����BGameOver�V�[���ɑJ�ڂ��܂��B");
            SceneManager.LoadScene("GameOverHamasaki"); // GameOver�V�[���ɑJ��
        }
    }
}

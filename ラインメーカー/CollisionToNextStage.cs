using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionToNextStage : MonoBehaviour
{
    // �Փˎ��ɌĂ΂�郁�\�b�h
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // �Փˑ���̏������O�ɏo��
        Debug.Log("�Փ˂��܂���: " + collision.gameObject.name);

        // GameOver�V�[���ɑJ�ڂ���
        Debug.Log("GameOver�V�[���ɑJ�ڂ��܂�...");
        SceneManager.LoadScene("MainSceneHamasaki2");
    }
}

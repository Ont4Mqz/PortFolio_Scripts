using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearManager : MonoBehaviour
{
    [SerializeField] private Collider2D targetCollider; // �w�肷�鑊��̃R���C�_�[

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other == targetCollider)
        {
            Debug.Log("�N���A����IResultScene�ֈڍs");
            SceneManager.LoadScene("ResultScene");
        }
    }
}

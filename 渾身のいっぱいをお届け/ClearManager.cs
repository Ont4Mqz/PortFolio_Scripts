using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearManager : MonoBehaviour
{
    [SerializeField] private Collider2D targetCollider; // 指定する相手のコライダー

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other == targetCollider)
        {
            Debug.Log("クリア判定！ResultSceneへ移行");
            SceneManager.LoadScene("ResultScene");
        }
    }
}

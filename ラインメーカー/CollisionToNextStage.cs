using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionToNextStage : MonoBehaviour
{
    // 衝突時に呼ばれるメソッド
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 衝突相手の情報をログに出力
        Debug.Log("衝突しました: " + collision.gameObject.name);

        // GameOverシーンに遷移する
        Debug.Log("GameOverシーンに遷移します...");
        SceneManager.LoadScene("MainSceneHamasaki2");
    }
}

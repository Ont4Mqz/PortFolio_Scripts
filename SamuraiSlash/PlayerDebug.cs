using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerDebug : MonoBehaviour
{
    public int hp = 10; // 適当なテスト用HP

    [SerializeField] string _sceneName;

    // ゾンビから呼ばれる想定のダメージ関数
    public void TakeDamage(int damage)
    {
        hp -= damage;
        Debug.Log($"{gameObject.name} が {damage} ダメージを受けた！ 残りHP: {hp}");

        if (hp <= 0)
        {
            Debug.Log($"{gameObject.name} は倒れた！");
            SceneManager.LoadScene( _sceneName );
        }
    }
}

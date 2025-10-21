using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerDebug : MonoBehaviour
{
    public int hp = 10; // �K���ȃe�X�g�pHP

    [SerializeField] string _sceneName;

    // �]���r����Ă΂��z��̃_���[�W�֐�
    public void TakeDamage(int damage)
    {
        hp -= damage;
        Debug.Log($"{gameObject.name} �� {damage} �_���[�W���󂯂��I �c��HP: {hp}");

        if (hp <= 0)
        {
            Debug.Log($"{gameObject.name} �͓|�ꂽ�I");
            SceneManager.LoadScene( _sceneName );
        }
    }
}

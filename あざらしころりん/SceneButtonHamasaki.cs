using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneButton : MonoBehaviour
{
    // インスペクターからシーン名を指定できる
    public string sceneName = "TitleSceneHamasakiKari"; // デフォルトで TitleSceneHamasakiKari に設定

    // ボタンが押されたときに呼び出される関数
    public void OnButtonPress()
    {
        // シーン名が指定されている場合、そのシーンへ遷移
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("シーン名が指定されていません！");
        }
    }
}

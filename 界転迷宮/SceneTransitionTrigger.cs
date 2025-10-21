using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransitionTrigger : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;
    [SerializeField] private ClearMove clearMove;

    private bool hasTriggered = false;

    void Start()
    {
        if (clearMove == null)
        {
            clearMove = FindObjectOfType<ClearMove>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (!hasTriggered && other.CompareTag("Player"))
        {
            hasTriggered = true;
            clearMove.PlayClearVideo();
            StartCoroutine(LoadSceneWithDelay(3f));
        }
    }

    private IEnumerator LoadSceneWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneToLoad);

    }
}

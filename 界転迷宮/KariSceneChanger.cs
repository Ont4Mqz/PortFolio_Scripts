using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class KariSceneChanger : MonoBehaviour
{
    [SerializeField] private InputActionReference KariButton; // ← InputActionReferenceに変更

    [SerializeField] private string targetSceneName;

    private void OnEnable()
    {
        KariButton.action.Enable();
        KariButton.action.performed += OnKariButtonPressed;
    }

    private void OnDisable()
    {
        KariButton.action.performed -= OnKariButtonPressed;
        KariButton.action.Disable();
    }

    private void OnKariButtonPressed(InputAction.CallbackContext context)
    {
        SceneManager.LoadScene(targetSceneName);
    }
}

using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.CompilerServices;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class KariClearWindow : MonoBehaviour
{
    public InputActionReference kariButton; // D-Pad/Up に対応した InputAction
    public CanvasGroup fadePanel;           // 黒背景 (ImageにCanvasGroup付き)
    public GameObject clearWindow;          // クリアパネル

    public float fadeDuration = 1.5f;
    public float exitDelay = 3f;

    private bool isFading = false;
    private bool hasCleared = false;
    private float fadeTime = 0f;
    private float exitTimer = 0f;

    private void Start()
    {
        fadePanel.alpha = 0f;
        fadePanel.gameObject.SetActive(false);
        clearWindow.SetActive(false);
    }

    private void OnEnable()
    {
        kariButton.action.performed += OnKariButtonPressed;
        kariButton.action.Enable();
    }

    private void OnDisable()
    {
        kariButton.action.performed -= OnKariButtonPressed;
        kariButton.action.Disable();
    }

    private void OnKariButtonPressed(InputAction.CallbackContext context)
    {
        if (!isFading && !hasCleared)
        {
            isFading = true;
            fadeTime = 0f;
            fadePanel.gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        if (isFading)
        {
            fadeTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(fadeTime / fadeDuration);
            fadePanel.alpha = alpha;

            if (alpha >= 1f)
            {
                isFading = false;
                hasCleared = true;
                clearWindow.SetActive(true);
                exitTimer = 0f;
                kariButton.action.Disable(); // 入力無効化
            }
        }

        if (hasCleared)
        {
            StartCoroutine(DelayedAction());
        }

        IEnumerator DelayedAction()
        {
            yield return new WaitForSeconds(3f);

            SceneManager.LoadScene("TitleScenes", LoadSceneMode.Single);
        }
    }
}


using UnityEngine;

public class WaniClicker : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    private WaniWaniManager manager;
    [SerializeField] private ScoreManager scoreManager;

    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        manager = FindObjectOfType<WaniWaniManager>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ClickCheck();
        }
    }

    void ClickCheck()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("MovingWani"))
            {
                manager.HitWani(hit.collider.transform);
                scoreManager.AddScore();

            }
        }
    }
}

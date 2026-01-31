using UnityEngine;

public class CameraMover : MonoBehaviour
{
    [SerializeField] private Transform targetPos;
    [SerializeField] private float moveTime = 3f;
    [SerializeField] private TimerManager timerManager;

    private Vector3 startPos;
    private float timer;
    private bool finished = false;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        if (finished) return;

        timer += Time.deltaTime;
        float t = Mathf.Clamp01(timer / moveTime);
        t = Mathf.SmoothStep(0f, 1f, t);
        transform.position = Vector3.Lerp(startPos, targetPos.position, t);

        if (t >= 1f)
        {
            finished = true;
            timerManager.StartGameCountdown();
        }
    }
}

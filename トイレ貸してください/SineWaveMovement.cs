using UnityEngine;

public class SineWaveMovement : MonoBehaviour
{
    // 蛇行の幅とスピードをInspectorで設定できるように
    [SerializeField] private float amplitude = 1.0f; // 蛇行の幅
    [SerializeField] private float frequency = 1.0f; // 蛇行のスピード
    [SerializeField] private float moveSpeed = 1.0f; // 左への進むスピード

    private Vector3 startPosition;

    void Start()
    {
        // 初期位置を記録
        startPosition = transform.position;
    }

    void Update()
    {
        // 左に進む動き
        float leftMovement = moveSpeed * Time.deltaTime;
        transform.position -= new Vector3(leftMovement, 0, 0);

        // 蛇行の動き
        float wave = Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = new Vector3(transform.position.x, startPosition.y + wave, transform.position.z);
    }
}

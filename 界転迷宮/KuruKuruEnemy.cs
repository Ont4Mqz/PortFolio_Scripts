using UnityEngine;

public class KurukuruEnemy : MonoBehaviour
{
    [Header("移動速度")]
    [SerializeField] private float moveSpeed = 3f;

    [Header("回転速度 (度/秒)")]
    [SerializeField] private float rotationSpeed = 180f;

    [Header("無視するコライダー（地面など）")]
    [SerializeField] private Collider[] ignoreColliders;

    private bool isRotating = false;
    private Quaternion targetRotation;

    private void Update()
    {
        // XとZの回転を常に0に固定（姿勢が崩れないように）
        Vector3 euler = transform.eulerAngles;
        transform.rotation = Quaternion.Euler(0f, euler.y, 0f);

        if (!isRotating)
        {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
        else
        {
            RotateEnemy();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        foreach (var ignore in ignoreColliders)
        {
            if (collision.collider == ignore)
                return;
        }

        if (!isRotating)
        {
            StartRotation();
        }
    }

    private void StartRotation()
    {
        isRotating = true;
        float newYRotation = transform.eulerAngles.y + 180f;
        targetRotation = Quaternion.Euler(0f, newYRotation, 0f);
    }

    private void RotateEnemy()
    {
        float step = rotationSpeed * Time.deltaTime;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, step);

        // XZ軸を固定しながらYのみ制御
        Vector3 euler = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(0f, euler.y, 0f);

        if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
        {
            transform.rotation = targetRotation;
            isRotating = false;
        }
    }
}

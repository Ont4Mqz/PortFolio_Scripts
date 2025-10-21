using UnityEngine;
using System.Collections.Generic;

public class ClippingObject : MonoBehaviour
{
    [Header("対象のカメラ")]
    [SerializeField] private Camera targetCamera;

    [Header("コライダーグループ1")]
    [SerializeField] private List<Collider> group1Colliders;
    [SerializeField] private float group1NearClip = 0.3f;
    [SerializeField] private float group1FarClip = 1000f;

    [Header("コライダーグループ2")]
    [SerializeField] private List<Collider> group2Colliders;
    [SerializeField] private float group2NearClip = 0.3f;
    [SerializeField] private float group2FarClip = 500f;

    [Header("コライダーグループ3")]
    [SerializeField] private List<Collider> group3Colliders;
    [SerializeField] private float group3NearClip = 0.1f;
    [SerializeField] private float group3FarClip = 300f;

    [Header("コライダーグループ4")]
    [SerializeField] private List<Collider> group4Colliders;
    [SerializeField] private float group4NearClip = 1f;
    [SerializeField] private float group4FarClip = 100f;

    [Header("コライダーグループ5")]
    [SerializeField] private List<Collider> group5Colliders;
    [SerializeField] private float group5NearClip = 0.5f;
    [SerializeField] private float group5FarClip = 200f;

    [Header("デフォルトClipping値（条件未達時）")]
    [SerializeField] private float defaultNearClip = 0.3f;
    [SerializeField] private float defaultFarClip = 1000f;

    [Header("Clippingを無視するオブジェクト")]
    [SerializeField] private List<GameObject> ignoreObjects;

    private bool isResetApplied = false;

    private void Start()
    {
        targetCamera = Camera.main;
    }

    private void Update()
    {
        if (targetCamera == null) return;

        if (!IsValidCameraCondition())
        {
            if (!isResetApplied)
            {
                ResetClipping();
                isResetApplied = true;
            }
        }
        else
        {
            isResetApplied = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsValidCameraCondition())
        {
            CheckCollision(other);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (IsValidCameraCondition())
        {
            CheckCollision(other);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (IsValidCameraCondition())
        {
            CheckCollision(collision.collider);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (IsValidCameraCondition())
        {
            CheckCollision(collision.collider);
        }
    }

    private void CheckCollision(Collider col)
    {
        if (targetCamera == null) return;

        // 例外オブジェクトに含まれていたらスキップ
        if (ignoreObjects.Contains(col.gameObject)) return;

        if (group1Colliders.Contains(col))
        {
            SetClipping(group1NearClip, group1FarClip);
        }
        else if (group2Colliders.Contains(col))
        {
            SetClipping(group2NearClip, group2FarClip);
        }
        else if (group3Colliders.Contains(col))
        {
            SetClipping(group3NearClip, group3FarClip);
        }
        else if (group4Colliders.Contains(col))
        {
            SetClipping(group4NearClip, group4FarClip);
        }
        else if (group5Colliders.Contains(col))
        {
            SetClipping(group5NearClip, group5FarClip);
        }
    }

    private bool IsValidCameraCondition()
    {
        float yPos = targetCamera.transform.position.y;
        float yRot = targetCamera.transform.eulerAngles.y;

        bool isHeightOk = Mathf.Abs(yPos - 2f) < 0.01f;
        bool isRotationOk =
            Mathf.Abs(yRot - 0f) < 5f ||
            Mathf.Abs(yRot - 90f) < 5f ||
            Mathf.Abs(yRot - 180f) < 5f ||
            Mathf.Abs(yRot - 270f) < 5f;

        return isHeightOk && isRotationOk;
    }

    private void SetClipping(float near, float far)
    {
        targetCamera.nearClipPlane = near;
        targetCamera.farClipPlane = far;
    }

    private void ResetClipping()
    {
        targetCamera.nearClipPlane = defaultNearClip;
        targetCamera.farClipPlane = defaultFarClip;
    }
}

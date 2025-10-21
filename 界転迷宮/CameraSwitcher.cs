using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraSwitcher : MonoBehaviour
{
    //フラグ
    public static bool IsCamera2D { get; private set; } = false;
    public bool isCamera2D = false;

    //カメラ設定
    private Vector3 twoDimentionCamInitPos = new Vector3(4f, 2f, -10f);
    private Vector3 twoDimentionCamInitRot = new Vector3(0f, 0f, 0f);
    private Vector3 threeDimentionCamInitPos = new Vector3(0f, 10f, -10f);
    private Vector3 threeDimentionCamInitRot = new Vector3(30f, 0f, 0f);

    // 保存用
    private Vector3 savedThreeDPos;
    private Quaternion savedThreeDRot;
    private Vector3 savedTwoDPos;
    private Quaternion savedTwoDRot;
    private bool hasSavedThreeD = false;
    private bool hasSavedTwoD = false;

    //公転用のターゲット
    public Transform orbitTarget;

    //公転の設定値
    public float distance = 10f;
    public float rotationSpeed = 60f;
    public float initialElevation = 30f;

    private float currentElevation;
    private float currentRotation = 0f;

    private bool isPositionLocked = false;

    //遷移や入力制御用
    private Camera mainCamera;
    private KaitenMeikyu controls;
    private bool isTransitioning = false;
    private bool isOrbiting = false;
    private bool waitingForTransitionEnd = false;

    private float currentAngle = 0f;
    private float orbitTransitionTime = 0.15f;

    private Vector2 lookInput;
    private bool suppressSouthInput = false;
    private StageManager _stageManager;

    //切り替えの時のやつ
    [SerializeField] private GameObject HokenPanel;//アニメーションの切れ目を隠す用
    [SerializeField] private GameObject transitionObject;
    private Animator transitionAnimator;

    private void Awake()//初期化
    {
        mainCamera = Camera.main;
        controls = new KaitenMeikyu();
        currentElevation = initialElevation;
    }

    private void OnEnable()//On
    {
        controls.Enable();
        controls.Camera.Look.performed += OnLookPerformed;
        controls.Camera.Look.canceled += OnLookCanceled;
        controls.Camera._3DReset.performed += On3DResetPressed;
        controls.Camera.MoveCamera.performed += ctx => OnToggleCameraPressed().Forget();
    }

    private void OnDisable()//Off
    {
        controls.Camera.Look.performed -= OnLookPerformed;
        controls.Camera.Look.canceled -= OnLookCanceled;
        controls.Camera._3DReset.performed -= On3DResetPressed;
        controls.Camera.MoveCamera.performed -= ctx => OnToggleCameraPressed().Forget();
        controls.Disable();
    }

    private void Start()
    {
        _stageManager = StageManager.Instance;//ステージマネージャーの取得
        if (orbitTarget != null)
        {
            Vector3 dir = mainCamera.transform.position - orbitTarget.position;//公転の設定
            currentAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
            currentAngle = Mathf.Round(currentAngle / 90f) * 90f;
        }
    }

    private void LateUpdate()
    {
        if (isPositionLocked) return;
        if (_stageManager != null && !_stageManager.IsPlaying) return;//プレイしてたらいける

        if (!isCamera2D)//3Dカメラの操作ぐるぐる
        {
            currentRotation += lookInput.x * rotationSpeed * Time.deltaTime;
            Quaternion rotation = Quaternion.Euler(currentElevation, currentRotation, 0);
            Vector3 position = rotation * new Vector3(0, 0, -distance) + orbitTarget.position;

            mainCamera.transform.rotation = rotation;
            mainCamera.transform.position = position;
        }
    }

    private void OnLookPerformed(InputAction.CallbackContext ctx)
    {
        if (_stageManager != null && !_stageManager.IsPlaying) return;
        lookInput = ctx.ReadValue<Vector2>();
    }
    private void OnLookCanceled(InputAction.CallbackContext ctx)
    {
        if (_stageManager != null && !_stageManager.IsPlaying) return;
        lookInput = Vector2.zero;
    }
    private void On3DResetPressed(InputAction.CallbackContext ctx)//3Dの時の視点リセット用
    {
        if (_stageManager != null && !_stageManager.IsPlaying) return;
        ResetView();
        suppressSouthInput = true;
        Invoke(nameof(ResetSouthSuppression), 0.1f);
    }
    private void ResetSouthSuppression() => suppressSouthInput = false;
    public bool IsSouthInputSuppressed => suppressSouthInput;

    public void UnlockCameraPosition() => isPositionLocked = false;//カメラ固定とか
    public void LockCameraPosition() => isPositionLocked = true;
    public void ResetElevation() => currentElevation = initialElevation;
    public void ResetView() { currentRotation = 0f; ResetElevation(); }

    private async UniTaskVoid OnToggleCameraPressed()//カメラ切り替え
    {
        if (isTransitioning || isOrbiting || waitingForTransitionEnd) return;
        if (_stageManager != null && !_stageManager.IsPlaying) return;

        if (transitionObject != null)
        {
            transitionObject.SetActive(true);
            transitionAnimator = transitionObject.GetComponent<Animator>();
        }
        transitionAnimator.SetTrigger("TransitionStart");//閉じてくアニメーション開始
        waitingForTransitionEnd = true;
    }

    public async void EndTransition()
    {
        await ToggleCameraAfterTransition();
        HokenPanel.SetActive(true);
        Invoke(nameof(DisableHokenPanel), 0.05f);
        if (transitionAnimator != null)
            transitionAnimator.SetTrigger("TransitionNext");
        Invoke(nameof(DisableTransitionObject), 0f);
    }

    private void DisableHokenPanel() => HokenPanel.SetActive(false);
    private void DisableTransitionObject()
    {
        if (transitionObject != null) transitionObject.SetActive(false);
        waitingForTransitionEnd = false;
    }

    private async UniTask ToggleCameraAfterTransition()//実際の切り替え処理
    {
        if (MenuUI.IsMenuActive) return;
        
        isCamera2D = !isCamera2D;
        IsCamera2D = isCamera2D;
        isTransitioning = true;

        controls.Camera.Disable();
        LockCameraPosition();

        if (isCamera2D)
        {
            // 3D → 2D
            savedThreeDPos = mainCamera.transform.position;
            savedThreeDRot = mainCamera.transform.rotation;
            hasSavedThreeD = true;

            if (hasSavedTwoD)
            {
                mainCamera.transform.position = savedTwoDPos;
                mainCamera.transform.rotation = savedTwoDRot;
            }
            else
            {
                mainCamera.transform.position = twoDimentionCamInitPos;
                mainCamera.transform.rotation = Quaternion.Euler(twoDimentionCamInitRot);
            }
        }
        else
        {
            // 2D → 3D
            savedTwoDPos = mainCamera.transform.position;
            savedTwoDRot = mainCamera.transform.rotation;
            hasSavedTwoD = true;

            Vector3 dir = mainCamera.transform.forward;
            float yaw = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg; // 2Dで見てた方向

            currentRotation = yaw;
            currentElevation = initialElevation;

            Vector3 pos = orbitTarget.position - (Quaternion.Euler(currentElevation, currentRotation, 0) * Vector3.forward * distance);
            mainCamera.transform.position = pos;
            mainCamera.transform.rotation = Quaternion.Euler(currentElevation, currentRotation, 0);

            hasSavedThreeD = true;
        }

        mainCamera.orthographic = isCamera2D;

        isTransitioning = false;
        if (!isCamera2D) { UnlockCameraPosition(); }
        else { UpdateCurrentAngleFromCamera(); }

        controls.Camera.Enable();
    }

    public async UniTask RotateAroundTarget(float angleDelta)//2Dの時の公転
    {
        if (!isCamera2D || isOrbiting || orbitTarget == null || isTransitioning) return;
        if (_stageManager != null && !_stageManager.IsPlaying) return;
        isOrbiting = true;
        float startAngle = currentAngle;
        float endAngle = currentAngle + angleDelta;
        float elapsed = 0f;
        while (elapsed < orbitTransitionTime)//移動を補完
        {
            elapsed += Time.deltaTime;
            float t = elapsed / orbitTransitionTime;
            float angle = Mathf.Lerp(startAngle, endAngle, t);
            UpdateOrbitPositionAtAngle(angle);
            await UniTask.Yield();
        }
        currentAngle = endAngle % 360f;
        UpdateOrbitPositionAtAngle(currentAngle);
        isOrbiting = false;
    }

    private void UpdateOrbitPositionAtAngle(float angle)//公転の位置更新
    {
        float radius = Vector3.Distance(mainCamera.transform.position, orbitTarget.position);
        float height = mainCamera.transform.position.y - orbitTarget.position.y;
        Vector3 offset = new Vector3(
            Mathf.Sin(angle * Mathf.Deg2Rad) * radius,
            height,
            Mathf.Cos(angle * Mathf.Deg2Rad) * radius
        );
        mainCamera.transform.position = orbitTarget.position + offset;
        mainCamera.transform.LookAt(orbitTarget.position);
    }

    private void UpdateCurrentAngleFromCamera()
    {
        Vector3 direction = mainCamera.transform.position - orbitTarget.position;
        currentAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        currentAngle = Mathf.Round(currentAngle / 90f) * 90f;
    }

    public void ResetToInitialPosition()//ステージリロード時のカメラリセット
    {
        if (isCamera2D)
        {
            mainCamera.transform.position = twoDimentionCamInitPos;
            mainCamera.transform.rotation = Quaternion.Euler(twoDimentionCamInitRot);
        }
        else
        {
            mainCamera.transform.position = threeDimentionCamInitPos;
            mainCamera.transform.rotation = Quaternion.Euler(threeDimentionCamInitRot);
            currentRotation = 0f;          // 方向もリセット
            currentElevation = initialElevation;
        }
    }

}

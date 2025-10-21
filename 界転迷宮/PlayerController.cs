using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("移動の速さ"), SerializeField] private float _speed = 3;//ステータス
    [Header("ジャンプする瞬間の速さ"), SerializeField] private float _jumpSpeed = 7;
    [Header("重力加速度"), SerializeField] private float _gravity = 15;
    [Header("落下時の速さ制限（Infinityで無制限）"), SerializeField] private float _fallSpeed = 10;
    [Header("落下の初速"), SerializeField] private float _initFallSpeed = 2;
    [Header("カメラのTransform"), SerializeField] private Transform _cameraTransform;
    [Header("GameClear演出呼び出し先"), SerializeField] private GameClearSpritAnimarion _clearAnimation;

    private Transform _transform;
    private CharacterController _characterController;
    private Vector2 _inputMove;
    private float _verticalVelocity;
    private bool _isGroundedPrev;

    private CameraSwitcher _cameraSwitcher;
    private ChaseBlockManager _chaseBlockManager;

    private bool _canMove = true;
    private Vector3 _currentFrameMoveAmount;

    private StageManager _stageManager;

    [SerializeField] private Animator _animator;
    [SerializeField] private TimeCounter timeCounter;

    // ← 追加: カメラが実際に2Dかを確実に判定するため、実機のカメラ投影を参照
    private bool IsCamera2D => Camera.main != null && Camera.main.orthographic;

    public void SetCanMove(bool canMove)
    {
        _canMove = canMove;
    }

    public Vector3 GetCurrentFrameMoveAmount()
    {
        return _currentFrameMoveAmount;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!_canMove || (_stageManager != null && !_stageManager.IsPlaying))
        {
            _inputMove = Vector2.zero;
            return;
        }

        _inputMove = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        // ← 修正: CameraSwitcher.IsCamera2D ではなく実際のカメラ投影を参照
        if (!IsCamera2D) return;
        if (_stageManager != null && !_stageManager.IsPlaying) return;
        if (!context.performed || !_characterController.isGrounded) return;

       _verticalVelocity = _jumpSpeed;

        if (_animator != null)
        {
            _animator.SetTrigger("JumpTrigger");
            _animator.SetBool("JumpBool", true);
        }
    }

    // public void DoJump()
    // {
    //     _verticalVelocity = _jumpSpeed;
    // }

    public void AnimetionSpeedUp()
    {
        if (_animator != null)
        {
            _animator.speed = 2.2f;
        }
    }

    public void ResetAnimetionSpeed()
    {
        if (_animator != null)
        {
            _animator.speed = 1f;
        }
    }


    public async void OnRightRotate(InputAction.CallbackContext context)
    {
        if (MenuUI.IsMenuActive) return;

        if (!context.performed || !IsCamera2D) return;
        if (_stageManager != null && !_stageManager.IsPlaying) return;
        if (!_chaseBlockManager.RevolveAllBlocks(true)) return;

        await _cameraSwitcher.RotateAroundTarget(-90f);

        _chaseBlockManager.SetMoveBlockLimits();
    }

    public async void OnLeftRotate(InputAction.CallbackContext context)
    {
        if (MenuUI.IsMenuActive) return;

        if (!context.performed || !IsCamera2D) return;
        if (_stageManager != null && !_stageManager.IsPlaying) return;
        if (!_chaseBlockManager.RevolveAllBlocks(false)) return;

        await _cameraSwitcher.RotateAroundTarget(90f);

        _chaseBlockManager.SetMoveBlockLimits();
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (GameManager.Instance.IsDamaged) return;

        if (hit.gameObject.layer == LayerMask.NameToLayer("DamageObj"))
        {
            hit.gameObject.GetComponent<DamageObject>().DamageAndReload();
        }

        if (hit.gameObject.layer == LayerMask.NameToLayer("Stage") && hit.gameObject.layer == LayerMask.NameToLayer("Default") && hit.gameObject.layer == LayerMask.NameToLayer("MoveChaseBlock"))
        {
            if (_animator != null)
            {
                _animator.SetBool("JumpBool", false);
            }
            return;
        }
        {
            _animator.SetBool("JumpBool", false);
        }

        if (hit.gameObject.CompareTag("Gool"))
        {
            GameResultData.Instance.SetGameResult(StageManager.Instance.GetHpValue(), timeCounter.RemainingTime);

            // ClearMove を探して動画再生を呼ぶ
            ClearMove clearMove = FindObjectOfType<ClearMove>();
            if (clearMove != null)
            {
                clearMove.PlayClearVideo();
                SetCanMove(false);
            }
            else
            {
                //SceneManager.LoadScene("Clear");
                //GameClear演出導入
                _clearAnimation.gameObject.SetActive(true);
            }
        }

    }

    private void Awake()
    {
        _transform = transform;
        _characterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        timeCounter = FindObjectOfType<TimeCounter>();
        _cameraSwitcher = Camera.main.GetComponent<CameraSwitcher>();
        _cameraTransform = Camera.main.transform;

        _stageManager = StageManager.Instance;
        _animator = GetComponentInChildren<Animator>();
        if (_chaseBlockManager == null)
        {
            _chaseBlockManager = FindObjectOfType<ChaseBlockManager>();
        }
    }

    private void Update()
    {
        if (MenuUI.IsMenuActive) return;

        // ← 修正: CameraSwitcher.IsCamera2D ではなく実カメラの orthographic を見て判定
        if (!IsCamera2D || !_canMove || (_stageManager != null && !_stageManager.IsPlaying))
        {
            _inputMove = Vector2.zero;
            _verticalVelocity = 0f;
            _currentFrameMoveAmount = Vector3.zero;

            if (_animator != null)
            {
                _animator.SetFloat("MoveFloat", 0f);
                _animator.SetBool("isMove", false);
            }


            return;
        }

        bool isGrounded = _characterController.isGrounded;

        if (isGrounded && !_isGroundedPrev)
        {
            _verticalVelocity = -_initFallSpeed;
        }
        else if (!isGrounded)
        {
            _verticalVelocity -= _gravity * Time.deltaTime;
            if (_verticalVelocity < -_fallSpeed)
                _verticalVelocity = -_fallSpeed;
        }

        _isGroundedPrev = isGrounded;

        Vector3 right = _cameraTransform.right;
        right.y = 0;
        right.Normalize();

        Vector3 moveDirection = right * _inputMove.x;

        _currentFrameMoveAmount = new Vector3(
            moveDirection.x * _speed * Time.deltaTime,
            _verticalVelocity * Time.deltaTime,
            moveDirection.z * _speed * Time.deltaTime
        );

        if (_chaseBlockManager != null && _chaseBlockManager.LimitMove(_currentFrameMoveAmount))
        {
            Vector3 prevPosition = transform.position;
            _characterController.Move(_currentFrameMoveAmount);
            _chaseBlockManager.MoveBlocks(transform.position - prevPosition);
        }
        else
        {
            _currentFrameMoveAmount.x = 0;
            _currentFrameMoveAmount.z = 0;
            _characterController.Move(_currentFrameMoveAmount);
        }

        if (_inputMove.x != 0)
        {
            _transform.rotation = Quaternion.LookRotation(moveDirection);
        }

        if (_animator != null)
        {
            // XZ方向の移動量の大きさ（Vector3）を計算
            float moveAmount = new Vector2(_currentFrameMoveAmount.x, _currentFrameMoveAmount.z).magnitude;

            // Animator の float パラメータ "MoveAmount" に設定
            _animator.SetFloat("MoveFloat", moveAmount);
            bool isMoveing = moveAmount > 0.01f;
            _animator.SetBool("MoveBool", isMoveing);

        }

    }

    public void SetChaseManager(ChaseBlockManager manager)
    {
        _chaseBlockManager = manager;
    }
}

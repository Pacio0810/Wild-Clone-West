using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("MovementPlayer")]
    [SerializeField] private float MouseVel = 70f;
    private float targetSpeed;
    public float currentSpeed;
    public float lerpSpeed;
    private float speedMultiplier;
    [SerializeField] private float maxWalkSpeed;
    Vector3 movement;
    [SerializeField] private float maxjumpForce;
    private float jumpForce;
    [SerializeField] private float maxGravity;
    [SerializeField] private float currentGravity = 0;
    [SerializeField] private float gravityLerp;
    [SerializeField] private float groundLenghtCheck;

    [Header("Animation")]
    [SerializeField] private float animBlendSpeed;
    private float _rotationVelocity;
    private int _animIDGrounded;
    private int _animIDJump;
    private int _animIDFreeFall;
    private float _fallTimeoutDelta;
    private float _jumpTimeoutDelta;
    private bool isjumping;

    [Header("Checks")]
    public LayerMask groundMask;
    public bool isGrounded = true;

    Rigidbody rb;
    private Animator _animator;

    [Header("Camera")]
    [SerializeField] private Transform cameraPivot;
    [SerializeField] private float minPitch;
    [SerializeField] private float maxPitch;
    private float pitchCamera;
    private float YawCamera;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        targetSpeed = maxWalkSpeed;
        _jumpTimeoutDelta = 0.5f;
        AssignAnimationIDs();
    }
    void Update()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundLenghtCheck, groundMask);
        if (_animator != null)
        {
            _animator.SetBool(_animIDGrounded, isGrounded);
        }
        CameraRotation();
        if (isGrounded)
        {
            rb.drag = 3;
            currentGravity = 0;
            isjumping = false;
        }
        else
        {
            currentGravity = Mathf.Lerp(currentGravity, maxGravity, gravityLerp * Time.deltaTime);
            rb.drag = 0;
        }
    }
    private void FixedUpdate()
    {
        PlayerMovement();
    }
    void CameraRotation()
    {
        YawCamera += InputSystem.self.DeltaMouseValue.x * MouseVel * Time.deltaTime;
        pitchCamera -= InputSystem.self.DeltaMouseValue.y * MouseVel * Time.deltaTime;
        pitchCamera = Mathf.Clamp(pitchCamera, minPitch, maxPitch);

        cameraPivot.rotation = Quaternion.Euler(pitchCamera, YawCamera, 0);
    }
    void PlayerMovement()
    {
        Sprint();
        Vector2 move = InputSystem.self.LocalAxisMove;
        Vector3 forwardRelative = move.y * cameraPivot.forward;
        Vector3 rightRelative = move.x * cameraPivot.right;

        if (move != Vector2.zero)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, lerpSpeed * Time.deltaTime);
            _animator.SetBool("IsMoving", true);
            movement = forwardRelative + rightRelative;
            movement.y = 0;

            float _blendSpeed = (currentSpeed / maxWalkSpeed) * speedMultiplier;
            if (isGrounded)
            {
                _animator.SetFloat("speed", _blendSpeed, animBlendSpeed, Time.deltaTime);
            }

            rb.velocity = movement * currentSpeed;

            float _targetRotation = Mathf.Atan2(move.x, move.y) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, 0.1f);
            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }
        else
        {
            _animator.SetBool("IsMoving", false);
            _animator.SetFloat("speed", 0, animBlendSpeed, Time.deltaTime);
            currentSpeed = 0;
            movement = Vector3.zero;
        }
        Jump();
        if (!isGrounded) { jumpForce -= currentGravity * Time.deltaTime; }
        else { jumpForce = 0; }
        rb.velocity += new Vector3(0, jumpForce, 0);
    }
    void Sprint()
    {
        targetSpeed = InputSystem.self.Sprint == true ? maxWalkSpeed * 2 : maxWalkSpeed;
        speedMultiplier = InputSystem.self.Sprint == true ? 2 : 1;
    }
    void Jump()
    {
        if (isGrounded)
        {
            _fallTimeoutDelta = 0.15f;

            if (_animator != null)
            {
                _animator.SetBool(_animIDJump, false);
                _animator.SetBool(_animIDFreeFall, false);
            }

            if (InputSystem.self.Jump && _jumpTimeoutDelta <= 0.0f && !isjumping)
            {
                if (_animator != null)
                {
                    _animator.SetBool(_animIDJump, true);

                    isjumping = true;
                    jumpForce = maxjumpForce;
                }
            }

            if (_jumpTimeoutDelta >= 0.0f)
            {
                _jumpTimeoutDelta -= Time.deltaTime;
            }
        }
        else
        {
            _jumpTimeoutDelta = 0.5f;

            if (_fallTimeoutDelta >= 0.0f)
            {
                _fallTimeoutDelta -= Time.deltaTime;
            }
            else
            {
                if (_animator != null)
                {
                    _animator.SetBool(_animIDFreeFall, true);
                }
            }
            InputSystem.self.Jump = false;
        }
    }
    private void AssignAnimationIDs()
    {
        _animIDGrounded = Animator.StringToHash("IsGrounded");
        _animIDJump = Animator.StringToHash("IsJumping");
        _animIDFreeFall = Animator.StringToHash("FreeFalling");
    }
}

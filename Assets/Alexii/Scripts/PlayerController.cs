using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header("Physic")]
    private Rigidbody _rb;
    [SerializeField] private LayerMask _groundMaskCheck;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _currentSpeed;
    [SerializeField] private float _MaxWalkSpeed;
    [SerializeField] private float _lerpSpeed;
    [SerializeField] private float _checkRadius;
    [SerializeField] private float _airControl;
    private float _rotationVelocity;
    private int sprintMultiply;

    [Header("Animation")]
    private Animator _anim;
    [SerializeField] private bool IsGrounded;
    private int IsJumpingID, IsFallingID, IsMovingID, IsGroundedID;

    [Header("Camera")]
    [SerializeField] private Transform _camera;
    private float yaw, pitch;
    [SerializeField] private float maxPitch, minPitch;
    [SerializeField] private float sensivity;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
        AssingAnimIds();
    }
    void Update()
    {
        CameraRotation();
        Move();
        CheckGround();
        Jump();
    }
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
            _anim.SetBool(IsJumpingID, true);
        }
    }
    void CheckGround()
    {
        if (Physics.CheckSphere(transform.position, _checkRadius, _groundMaskCheck))
        {
            IsGrounded = true;
            _anim.SetBool(IsGroundedID, true);
            _anim.SetBool(IsJumpingID, false);
            _anim.SetBool(IsFallingID, false);
            _rb.drag = 10;
        }
        else
        {
            if (IsGrounded)
            {
                IsGrounded = false;
                _anim.SetBool(IsGroundedID, false);
                _anim.SetBool(IsFallingID, true);
                _rb.drag = 0;
            }
        }
    }
    void AssingAnimIds()
    {
        IsJumpingID = Animator.StringToHash("IsJumping");
        IsFallingID = Animator.StringToHash("IsFalling");
        IsMovingID = Animator.StringToHash("IsMoving");
        IsGroundedID = Animator.StringToHash("IsGrounded");
    }
    void Move()
    {
        float side = InputSystem.self.LocalAxisMove.x;
        float forward = InputSystem.self.LocalAxisMove.y;

        Vector3 direction = side * _camera.right + forward * _camera.forward;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            sprintMultiply = InputSystem.self.Sprint ? 2 : 1;

            _currentSpeed = Mathf.Lerp(_currentSpeed, _MaxWalkSpeed, _lerpSpeed * Time.deltaTime);
            _anim.SetBool(IsMovingID, true);
            _anim.SetFloat("Velocity", sprintMultiply,0.3f,Time.deltaTime);
        }
        else { _currentSpeed = 0; _anim.SetBool(IsMovingID, false); _anim.SetFloat("Velocity", 0, 0.9f, Time.deltaTime); }

        float multiplySpeed = (IsGrounded ? 10 : _airControl) * sprintMultiply;

        _rb.AddForce(direction * _currentSpeed * multiplySpeed * Time.deltaTime, ForceMode.Force);

        float _targetRotation = Mathf.Atan2(side, forward) * Mathf.Rad2Deg + _camera.transform.eulerAngles.y;
        float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, 0.1f);
        transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
    }
    void CameraRotation()
    {
        yaw += Input.GetAxis("Mouse X");
        pitch -= Input.GetAxis("Mouse Y");

        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        _camera.rotation = Quaternion.Euler(pitch, yaw, 0);
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}

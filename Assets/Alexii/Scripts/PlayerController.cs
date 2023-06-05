using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    private int IsJumpingID, IsFallingID, IsMovingID, IsGroundedID, Velocity, Side, Forward, IsArmed;
    [SerializeField] Transform targetPosition;
    [SerializeField] Transform bone;
    [SerializeField] Transform root;
    private bool ShouldDOIK = true;

    [Header("Camera")]
    [SerializeField] private Transform _camera;
    private float yaw, pitch;
    [SerializeField] private float maxPitch, minPitch;
    [SerializeField] private float sensivity;

    public bool isGrappling;

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
        EquipWeapon();
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
        if (Physics.CheckSphere(transform.position, _checkRadius, _groundMaskCheck) && !isGrappling)
        {
            IsGrounded = true;
            _anim.SetBool(IsGroundedID, true);
            _anim.SetBool(IsJumpingID, false);
            _anim.SetBool(IsFallingID, false);
            _rb.drag = 10;
            ShouldDOIK = true;
        }
        else
        {
            if (IsGrounded)
            {
                IsGrounded = false;
                _anim.SetBool(IsGroundedID, false);
                _anim.SetBool(IsFallingID, true);
                _rb.drag = 0;
                ShouldDOIK = false;
            }
        }
    }
    void AssingAnimIds()
    {
        IsJumpingID = Animator.StringToHash("IsJumping");
        IsFallingID = Animator.StringToHash("IsFalling");
        IsMovingID = Animator.StringToHash("IsMoving");
        IsGroundedID = Animator.StringToHash("IsGrounded");
        Velocity = Animator.StringToHash("Velocity");
        Side = Animator.StringToHash("Side");
        Forward = Animator.StringToHash("Forward");
        IsArmed = Animator.StringToHash("IsArmed");
    }
    void Move()
    {
        if (isGrappling)
        {
            return;
        }
        sprintMultiply = InputSystem.self.Sprint ? 2 : 1;

        float side = InputSystem.self.LocalAxisMove.x;
        float forward = InputSystem.self.LocalAxisMove.y;
        _anim.SetFloat(Side, side * sprintMultiply, 0.2f, Time.deltaTime);
        _anim.SetFloat(Forward, forward * sprintMultiply, 0.2f, Time.deltaTime);

        Vector3 direction = side * _camera.right + forward * _camera.forward;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            _currentSpeed = Mathf.Lerp(_currentSpeed, _MaxWalkSpeed, _lerpSpeed * Time.deltaTime);
            _anim.SetBool(IsMovingID, true);
            _anim.SetFloat(Velocity, sprintMultiply, 0.3f, Time.deltaTime);

            
        }
        else { _currentSpeed = 0; _anim.SetBool(IsMovingID, false); _anim.SetFloat(Velocity, 0, 0.9f, Time.deltaTime); }

        float multiplySpeed = (IsGrounded ? 10 : _airControl) * sprintMultiply;

        _rb.AddForce(direction * _currentSpeed * multiplySpeed * Time.deltaTime, ForceMode.Force);
        float rot = !_anim.GetBool(IsArmed) ? Mathf.Rad2Deg : 0;
        float _targetRotation = Mathf.Atan2(side, forward) * rot + _camera.transform.eulerAngles.y;
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
    void EquipWeapon()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (_anim.GetBool(IsArmed))
            {
                _anim.SetBool(IsArmed, false);
            }
            else
            {
                _anim.SetBool(IsArmed, true);
            }
        }
    }
    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    public Vector3 GrapplingJumpVelocity(Vector3 startPoint, Vector3 end, float trajectory)
    {
        float gravity = Physics.gravity.y;
        float displacementY = end.y - startPoint.y;
        Vector3 dispXY = new Vector3(end.x - startPoint.x, 0f, end.z - startPoint.z);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * trajectory);

        Vector3 velocityXZ = dispXY / (Mathf.Sqrt(-2 * trajectory / gravity) + Mathf.Sqrt(2 * (displacementY - trajectory) / gravity));

        return velocityXZ - velocityY;
    }
    void AimLock(float weight)
    {
        if (targetPosition == null || !_anim.GetBool(IsArmed)) return;
        Vector3 aimDirection = _anim.transform.forward;
        Vector3 targetDirection = targetPosition.position - _anim.transform.position;
        Quaternion aimToward = Quaternion.FromToRotation(aimDirection, targetDirection);
        Quaternion blendedRotation = Quaternion.Slerp(Quaternion.identity, aimToward, weight);
        bone.rotation = blendedRotation * bone.rotation;
    }
    private void LateUpdate()
    {
        AimLock(1);
    }
    void OnAnimatorIK()
    {
        if (ShouldDOIK)
        {
            //footIK
            Vector3 l_foot = _anim.GetBoneTransform(HumanBodyBones.LeftFoot).position;
            Vector3 r_foot = _anim.GetBoneTransform(HumanBodyBones.RightFoot).position;

            l_foot = GetHitPoint(l_foot + Vector3.up, l_foot - Vector3.up * 5) + new Vector3(0, 0.15f, 0);
            r_foot = GetHitPoint(r_foot + Vector3.up, r_foot - Vector3.up * 5) + new Vector3(0, 0.15f, 0);

            root.localPosition = new Vector3(0, Mathf.Abs((l_foot.y - r_foot.y) * 0.5f), 0);

            _anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, _anim.GetFloat("ik_foot_l_weight"));
            _anim.SetIKPosition(AvatarIKGoal.LeftFoot, l_foot);

            _anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, _anim.GetFloat("ik_foot_r_weight"));
            _anim.SetIKPosition(AvatarIKGoal.RightFoot, r_foot);

            //LockAimIK
            if (targetPosition != null && !_anim.GetBool(IsArmed))
            {
                _anim.SetLookAtWeight(1);
                _anim.SetLookAtPosition(targetPosition.position);
            }
        }
        else
        {
            _anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 0);
            _anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, 0);
            _anim.SetLookAtWeight(0);

        }
    }
    private Vector3 GetHitPoint(Vector3 start, Vector3 end)
    {
        RaycastHit hit;
        if (Physics.Linecast(start, end, out hit))
        {
            return hit.point;
        }
        return Vector3.zero;
    }
}

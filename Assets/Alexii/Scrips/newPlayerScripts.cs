using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class newPlayerScripts : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed;

    [SerializeField]
    private float jumpSpeed;

    [SerializeField]
    private float jumpButtonGracePeriod;

    [SerializeField]
    private float jumpHorizontalSpeed;

    [SerializeField]
    private Transform cameraTransform;

    private Animator animator;
    private CharacterController characterController;
    private float ySpeed;
    private float originalStepOffset;
    private float? lastGroundedTime;
    private float? jumpButtonPressedTime;
    private bool isJumping;
    private bool isGrounded;
    [SerializeField] private float speed = 500;
    [SerializeField] private float sensivity;
    private float YawCamera;
    private float pitchCamera;
    [SerializeField] private float minPitch;
    [SerializeField] private float maxPitch;

    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        originalStepOffset = characterController.stepOffset;
    }

    void Update()
    {
        CameraRotation();

        float horizontalInput = InputSystem.self.LocalAxisMove.x;
        float verticalInput = InputSystem.self.LocalAxisMove.y;

        Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);
        float inputMagnitude = Mathf.Clamp01(movementDirection.magnitude);

        if (InputSystem.self.Sprint)
        {
            inputMagnitude = 2;
        }

        animator.SetFloat("speed", inputMagnitude, 0.1f, Time.deltaTime);

        movementDirection = Quaternion.AngleAxis(cameraTransform.rotation.eulerAngles.y, Vector3.up) * movementDirection;
        movementDirection.Normalize();

        ySpeed += Physics.gravity.y * Time.deltaTime;

        if (characterController.isGrounded)
        {
            lastGroundedTime = Time.time;
        }

        if (InputSystem.self.Jump)
        {
            jumpButtonPressedTime = Time.time;
        }

        if (Time.time - lastGroundedTime <= jumpButtonGracePeriod)
        {
            characterController.stepOffset = originalStepOffset;
            ySpeed = -0.5f;
            animator.SetBool("IsGrounded", true);
            isGrounded = true;
            animator.SetBool("IsJumping", false);
            isJumping = false;
            animator.SetBool("FreeFalling", false);

            if (Time.time - jumpButtonPressedTime <= jumpButtonGracePeriod)
            {
                ySpeed = jumpSpeed;
                animator.SetBool("IsJumping", true);
                isJumping = true;
                jumpButtonPressedTime = null;
                lastGroundedTime = null;
            }
        }
        else
        {
            characterController.stepOffset = 0;
            animator.SetBool("IsGrounded", false);
            isGrounded = false;

            if ((isJumping && ySpeed < 0) || ySpeed < -2)
            {
                animator.SetBool("FreeFalling", true);
            }
        }

        Vector3 velocity = movementDirection * speed;
        velocity.y = ySpeed;

        characterController.Move(velocity * Time.deltaTime);

        if (movementDirection != Vector3.zero)
        {
            animator.SetBool("IsMoving", true);

            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            animator.SetBool("IsMoving", false);
        }

        if (isGrounded == false)
        {
            Vector3 velocity2 = movementDirection * inputMagnitude * jumpHorizontalSpeed;
            velocity2.y = ySpeed;

            characterController.Move(velocity2 * Time.deltaTime);
        }
    }

    private void OnAnimatorMove()
    {
        //if (isGrounded)
        //{
        //    Vector3 velocity = animator.deltaPosition;
        //    velocity.y = ySpeed * Time.deltaTime;

        //    characterController.Move(velocity);
        //}
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void CameraRotation()
    {
        YawCamera += InputSystem.self.DeltaMouseValue.x * sensivity * Time.deltaTime;
        pitchCamera -= InputSystem.self.DeltaMouseValue.y * sensivity * Time.deltaTime;
        pitchCamera = Mathf.Clamp(pitchCamera, minPitch, maxPitch);

        cameraTransform.rotation = Quaternion.Euler(pitchCamera, YawCamera, 0);
    }
}

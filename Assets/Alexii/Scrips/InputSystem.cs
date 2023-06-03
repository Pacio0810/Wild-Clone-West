using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.Windows;

public class InputSystem : Singleton<InputSystem>
{
    Move inputSystem;
    public Vector2 DeltaMouseValue;
    public Vector2 AxisMove;
    public Vector2 LocalAxisMove;
    public bool Sprint;
    public bool Jump;

    void Awake()
    {
        inputSystem = new Move();
    }
    private void OnEnable()
    {
        inputSystem.Enable();

        inputSystem.Player.CameraRotationKBM.performed += MoveCamera;
        inputSystem.Player.CameraRotationKBM.canceled += MoveCameraEnd;
        inputSystem.Player.CameraRotationPad.performed += MoveCamera;
        inputSystem.Player.CameraRotationPad.canceled += MoveCameraEnd;

        inputSystem.Player.Movement.performed += OnMovementStart;
        inputSystem.Player.Movement.canceled += OnMovementEnd;

        inputSystem.Player.Sprint.performed += OnSprintStart;
        inputSystem.Player.Sprint.canceled += OnSprintEnd;

        inputSystem.Player.Jump.performed += OnJumpStart;
        inputSystem.Player.Jump.canceled += OnJumpEnd;

    }
    private void OnDisable()
    {
        inputSystem.Disable();

        inputSystem.Player.CameraRotationKBM.performed -= MoveCamera;
        inputSystem.Player.CameraRotationKBM.canceled -= MoveCameraEnd;

        inputSystem.Player.Movement.performed -= OnMovementStart;
        inputSystem.Player.Movement.canceled -= OnMovementEnd;

        inputSystem.Player.Sprint.performed -= OnSprintStart;
        inputSystem.Player.Sprint.canceled -= OnSprintEnd;

        inputSystem.Player.Jump.performed -= OnJumpStart;
        inputSystem.Player.Jump.canceled -= OnJumpEnd;

    }
    void MoveCamera(InputAction.CallbackContext value)
    {
        DeltaMouseValue = inputSystem.Player.CameraRotationKBM.ReadValue<Vector2>();
    }
    void MoveCameraEnd(InputAction.CallbackContext value)
    {
        DeltaMouseValue = Vector2.zero;
    }
    void OnMovementStart(InputAction.CallbackContext value)
    {
        LocalAxisMove = inputSystem.Player.Movement.ReadValue<Vector2>();
    }
    void OnMovementEnd(InputAction.CallbackContext value)
    {
        LocalAxisMove = Vector2.zero;
    }
    void OnSprintStart(InputAction.CallbackContext value)
    {
        Sprint = inputSystem.Player.Sprint.triggered;
    }
    void OnSprintEnd(InputAction.CallbackContext value)
    {
        Sprint = false;
    }
    void OnJumpStart(InputAction.CallbackContext value)
    {
        Jump = inputSystem.Player.Jump.triggered;
    }
    void OnJumpEnd(InputAction.CallbackContext value)
    {
        Jump = false;
    }
}

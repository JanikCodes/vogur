using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class WorldPlayerInput : MonoBehaviour
{
    public static WorldPlayerInput Instance { get; private set; }

    public event Action OnSingleClick;
    public event Action OnCameraZoom;
    public event CameraRotationEnabledDelegate OnCameraEnableRotationChange;
    public delegate void CameraRotationEnabledDelegate(bool enabled);

    private CustomInput input;

    private void Awake()
    {
        input = new CustomInput();

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void OnEnable()
    {
        input.PlayerOverworld.SingleClickToMove.started += SingleClickToMove;
        input.PlayerOverworld.CameraEnableRotation.started += CameraRotationEnabled;
        input.PlayerOverworld.CameraEnableRotation.canceled += CameraRotationEnabled;
        input.PlayerOverworld.CameraZoom.performed += CameraZoom;
        input.Enable();
    }

    private void OnDisable()
    {
        input.PlayerOverworld.SingleClickToMove.started -= SingleClickToMove;
        input.PlayerOverworld.CameraEnableRotation.started -= CameraRotationEnabled;
        input.PlayerOverworld.CameraEnableRotation.canceled -= CameraRotationEnabled;
        input.PlayerOverworld.CameraZoom.performed -= CameraZoom;
        input.Disable();
    }

    public Vector2 GetCameraRotation()
    {
        if (input == null) { return new Vector2(); }

        return input.PlayerOverworld.CameraRotation.ReadValue<Vector2>();
    }

    public Vector3 GetMousePosition()
    {
        if (input == null) { return new Vector2(); }

        return input.PlayerOverworld.MousePosition.ReadValue<Vector2>();
    }

    public Vector2 GetCameraZoom()
    {
        return input.PlayerOverworld.CameraZoom.ReadValue<Vector2>();
    }

    private void SingleClickToMove(CallbackContext context)
    {
        if (!context.started) { return; }
        if (Util.IsMouseOverUI()) { return; }

        OnSingleClick?.Invoke();
    }

    private void CameraRotationEnabled(CallbackContext context)
    {
        if (context.started)
        {
            OnCameraEnableRotationChange?.Invoke(true);
            return;
        }

        if (context.canceled)
        {
            OnCameraEnableRotationChange?.Invoke(false);
            return;
        }
    }

    private void CameraZoom(CallbackContext context)
    {
        OnCameraZoom?.Invoke();
    }
}

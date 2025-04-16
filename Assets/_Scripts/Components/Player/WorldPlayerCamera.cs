using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldPlayerCamera : MonoBehaviour
{
    [SerializeField] private Transform target;
    [Header("Configurations")]
    [SerializeField] private float rotationSpeed = 0.35f;
    [SerializeField] private float zoomStepSize = 1f;
    [SerializeField] private Vector2 minMaxViewClamp = new(15, 75);
    [SerializeField] private Vector2 minMaxZoom = new(20, 65);

    private WorldPlayerInput input;
    private bool cameraRotationEnabled;
    private float cameraDistance;
    private float yaw;
    private float pitch;

    private void Awake()
    {
        input = WorldPlayerInput.Instance;

        // Call this once at start to set the initial cameraDistance
        cameraDistance = minMaxZoom.x + minMaxZoom.y / 2;

        Rotate(true);
    }

    private void OnEnable()
    {
        input.OnCameraEnableRotationChange += CameraRotationEnabledChanged;
        input.OnCameraZoom += Zoom;
    }

    private void OnDisable()
    {
        input.OnCameraEnableRotationChange -= CameraRotationEnabledChanged;
        input.OnCameraZoom -= Zoom;
    }

    private void LateUpdate()
    {
        Rotate();
        FollowTarget();
    }

    public void FollowTarget()
    {
        transform.position = target.position - (transform.forward * cameraDistance);
    }

    public void Rotate(bool force = false)
    {
        if (!cameraRotationEnabled && !force) { return; }

        yaw += input.GetCameraRotation().x * rotationSpeed;
        pitch -= input.GetCameraRotation().y * rotationSpeed;
        pitch = Mathf.Clamp(pitch, minMaxViewClamp.x, minMaxViewClamp.y);

        transform.eulerAngles = new Vector3(pitch, yaw);
    }

    public void Zoom()
    {
        float inputValue = input.GetCameraZoom().y / 100f;

        if (Mathf.Abs(inputValue) > 0.1f)
        {
            cameraDistance = cameraDistance - inputValue * zoomStepSize;
            if (cameraDistance < minMaxZoom.x)
            {
                cameraDistance = minMaxZoom.x;
            }
            else if (cameraDistance > minMaxZoom.y)
            {
                cameraDistance = minMaxZoom.y;
            }
        }
    }

    private void CameraRotationEnabledChanged(bool enabled)
    {
        cameraRotationEnabled = enabled;
    }
}

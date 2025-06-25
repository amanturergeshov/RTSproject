using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private Transform cameraTaret;
    [SerializeField] private CinemachineCamera cinemachineCamera;
    [SerializeField] private float KeyboardPanSpeed = 5;
    [SerializeField] private float zoomSpeed = 5;
    [SerializeField] private float minZoomDistance = 7.5f;
    [SerializeField] private float maxZoomDistance = 15;
    Vector2 moveAmount;

    private CinemachineFollow cinemachineFollow;
    private float zoomStartTime;
    private Vector3 startingFollowOffset;
    private Vector3 targetFollowOffset;


    private void Awake()
    {
        if (!cinemachineCamera.TryGetComponent(out cinemachineFollow))
        {
            Debug.LogError("Cinemachine Camera did not have CinemachineFollow. Zoom functionality will not work!");
        }

        startingFollowOffset = cinemachineFollow.FollowOffset;

    }

    private void Update()
    {
        HandlePanning();
        HandleZooming();
    }
    private void HandlePanning()
    {
        moveAmount = Vector2.zero;

        if (Keyboard.current.upArrowKey.isPressed)
        {
            moveAmount.y += KeyboardPanSpeed;
        }
        if (Keyboard.current.leftArrowKey.isPressed)
        {
            moveAmount.x -= KeyboardPanSpeed;
        }

        if (Keyboard.current.downArrowKey.isPressed)
        {
            moveAmount.y -= KeyboardPanSpeed;
        }
        if (Keyboard.current.rightArrowKey.isPressed)
        {
            moveAmount.x += KeyboardPanSpeed;
        }
        moveAmount *= Time.deltaTime;
        cameraTaret.position += new Vector3(moveAmount.x, 0, moveAmount.y);
    }

    private void HandleZooming()
    {
        if (ShooldSetZoomStartTime())
        {
            zoomStartTime = Time.time;
        }
        float zoomTime = Mathf.Clamp01((Time.time - zoomStartTime) * zoomSpeed);

        if (Keyboard.current.endKey.isPressed)
        {
            targetFollowOffset = new Vector3(cinemachineFollow.FollowOffset.x, minZoomDistance, cinemachineFollow.FollowOffset.z);
            cinemachineFollow.FollowOffset = Vector3.Slerp(startingFollowOffset, targetFollowOffset, zoomTime);
        }
        else
        {
            targetFollowOffset = new Vector3(cinemachineFollow.FollowOffset.x, startingFollowOffset.y, cinemachineFollow.FollowOffset.z);
        }
        cinemachineFollow.FollowOffset = Vector3.Slerp(cinemachineFollow.FollowOffset, targetFollowOffset, zoomTime);
    }

    private bool ShooldSetZoomStartTime()
    {
        return Keyboard.current.endKey.wasPressedThisFrame
        || Keyboard.current.endKey.wasReleasedThisFrame;
    }
}
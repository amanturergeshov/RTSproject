using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;


namespace RTS.Player
{
    public class PlayerInput : MonoBehaviour
    {
        [SerializeField] private Transform cameraTaret;
        [SerializeField] private CinemachineCamera cinemachineCamera;
        [SerializeField] private CameraConfig cameraConfig;

        private Vector2 moveAmount;
        private CinemachineFollow cinemachineFollow;
        private float zoomStartTime;
        private float rotationStartTime;
        private Vector3 startingFollowOffset;
        private float maxRotationAmount;


        private void Awake()
        {
            if (!cinemachineCamera.TryGetComponent(out cinemachineFollow))
            {
                Debug.LogError("Cinemachine Camera did not have CinemachineFollow. Zoom functionality will not work!");
            }

            startingFollowOffset = cinemachineFollow.FollowOffset;
            maxRotationAmount = Mathf.Abs(cinemachineFollow.FollowOffset.z);
        }

        private void Update()
        {
            HandlePanning();
            HandleZooming();
            HandleRotation();
        }
        private void HandleRotation()
        {
            if (ShouldSetRotationStartTime())
            {
                rotationStartTime = Time.time;
            }
            float rotationTime = Mathf.Clamp01((Time.time - rotationStartTime) * cameraConfig.RotationSpeed);
            Vector3 targetFollowOffset;
            if (Keyboard.current.pageDownKey.isPressed)
            {
                targetFollowOffset = new Vector3(maxRotationAmount, cinemachineFollow.FollowOffset.y, 0);
            }
            else if (Keyboard.current.pageUpKey.isPressed)
            {
                targetFollowOffset = new Vector3(-maxRotationAmount, cinemachineFollow.FollowOffset.y, 0);
            }
            else
            {
                targetFollowOffset = new Vector3(startingFollowOffset.x, cinemachineFollow.FollowOffset.y, startingFollowOffset.z);

            }
            if (cinemachineFollow.FollowOffset != targetFollowOffset)
            {
                cinemachineFollow.FollowOffset = Vector3.Slerp(cinemachineFollow.FollowOffset, targetFollowOffset, rotationTime);
            }
        }

        private bool ShouldSetRotationStartTime()
        {
            return Keyboard.current.pageUpKey.wasPressedThisFrame ||
            Keyboard.current.pageDownKey.wasPressedThisFrame ||
            Keyboard.current.pageUpKey.wasReleasedThisFrame ||
            Keyboard.current.pageDownKey.wasReleasedThisFrame;
        }

        private void HandlePanning()
        {
            moveAmount = Vector2.zero;

            if (Keyboard.current.upArrowKey.isPressed)
            {
                moveAmount.y += cameraConfig.KeyboardPanSpeed;
            }
            if (Keyboard.current.leftArrowKey.isPressed)
            {
                moveAmount.x -= cameraConfig.KeyboardPanSpeed;
            }

            if (Keyboard.current.downArrowKey.isPressed)
            {
                moveAmount.y -= cameraConfig.KeyboardPanSpeed;
            }
            if (Keyboard.current.rightArrowKey.isPressed)
            {
                moveAmount.x += cameraConfig.KeyboardPanSpeed;
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
            float zoomTime = Mathf.Clamp01((Time.time - zoomStartTime) * cameraConfig.ZoomSpeed);
            Vector3 targetFollowOffset;
            if (Keyboard.current.endKey.isPressed)
            {
                targetFollowOffset = new Vector3(cinemachineFollow.FollowOffset.x, cameraConfig.MinZoomDistance, cinemachineFollow.FollowOffset.z);
                cinemachineFollow.FollowOffset = Vector3.Slerp(startingFollowOffset, targetFollowOffset, zoomTime);
            }
            else
            {
                targetFollowOffset = new Vector3(cinemachineFollow.FollowOffset.x, startingFollowOffset.y, cinemachineFollow.FollowOffset.z);
            }
            if (cinemachineFollow.FollowOffset != targetFollowOffset)
            {
                cinemachineFollow.FollowOffset = Vector3.Slerp(cinemachineFollow.FollowOffset, targetFollowOffset, zoomTime);
            }
        }

        private bool ShooldSetZoomStartTime()
        {
            return Keyboard.current.endKey.wasPressedThisFrame
            || Keyboard.current.endKey.wasReleasedThisFrame;
        }
    }
}
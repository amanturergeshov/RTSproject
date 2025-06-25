using UnityEngine;

namespace RTS.Player
{
    [System.Serializable]
    public class CameraConfig
    {
        [field: SerializeField] public float KeyboardPanSpeed { get; private set; } = 5;
        [field: SerializeField] public float ZoomSpeed { get; private set; } = 5;
        [field: SerializeField] public float RotationSpeed { get; private set; } = 5;
        [field: SerializeField] public float MinZoomDistance { get; private set; } = 7.5f;

    }
}
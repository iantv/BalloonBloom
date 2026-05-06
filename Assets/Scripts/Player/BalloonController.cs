using UnityEngine;

namespace BalloonBloom.Player
{
    /// <summary>
    /// Handles vertical drag movement for the balloon.
    /// </summary>
    public sealed class BalloonController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private Camera gameplayCamera;
        [SerializeField] private float verticalMin = -3.5f;
        [SerializeField] private float verticalMax = 3.5f;
        [SerializeField] [Min(0.1f)] private float followSpeed = 12f;

        [Header("Input")]
        [SerializeField] [Min(0.01f)] private float dragSensitivity = 0.01f;

        private float _targetY;
        private bool _hasTarget;

        private void Awake()
        {
            _targetY = transform.position.y;
            _hasTarget = true;

            if (gameplayCamera == null)
            {
                gameplayCamera = Camera.main;
            }
        }

        private void Update()
        {
            ReadInput();
            MoveToTarget();
        }

        private void ReadInput()
        {
            if (Input.touchCount > 0)
            {
                var touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                {
                    _targetY += touch.deltaPosition.y * dragSensitivity;
                }

                _targetY = Mathf.Clamp(_targetY, verticalMin, verticalMax);
                return;
            }

            if (Input.GetMouseButton(0))
            {
                _targetY += Input.GetAxis("Mouse Y") * 8f * dragSensitivity;
                _targetY = Mathf.Clamp(_targetY, verticalMin, verticalMax);
            }
        }

        private void MoveToTarget()
        {
            if (!_hasTarget)
            {
                return;
            }

            var current = transform.position;
            var target = new Vector3(current.x, _targetY, current.z);
            transform.position = Vector3.Lerp(current, target, 1f - Mathf.Exp(-followSpeed * Time.deltaTime));
        }
    }
}

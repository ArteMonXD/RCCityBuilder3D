using UnityEngine;

namespace Presentation.Gameplay.Views
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 10f;
        [SerializeField] private float zoomSpeed = 5f;
        [SerializeField] private float rotationSpeed = 90f;
        [SerializeField] private float minZoom = 5f;
        [SerializeField] private float maxZoom = 20f;

        private Vector3 _targetPosition;
        private float _targetZoom = 10f;

        void Update()
        {
            HandleInput();
            SmoothMovement();
        }

        private void HandleInput()
        {
            // WASD движение
            var move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            _targetPosition += transform.rotation * move * moveSpeed * Time.deltaTime;

            // Zoom
            var scroll = Input.GetAxis("Mouse ScrollWheel");
            _targetZoom = Mathf.Clamp(_targetZoom - scroll * zoomSpeed, minZoom, maxZoom);

            // Вращение (R)
            if (Input.GetKey(KeyCode.R))
            {
                transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
            }

            // Удаление (Delete)
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                // Будем обрабатывать через InputHandler
            }
        }

        private void SmoothMovement()
        {
            transform.position = Vector3.Lerp(transform.position, _targetPosition, 0.1f);

            var camera = GetComponent<Camera>();
            if (camera != null)
            {
                camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, _targetZoom, 0.1f);
            }
        }
    }
}

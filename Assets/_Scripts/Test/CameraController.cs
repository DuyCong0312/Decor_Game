using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputManager inputManager;

    [Header("Pan Settings")]
    [SerializeField] private float panSpeed = 10f;

    [Header("Zoom Settings")]
    [SerializeField] private float zoomSpeed = 10f;

    [Header("Rotate Settings")]
    [SerializeField] private float rotateSpeed = 2f;

    private Vector2 panInput;
    private float zoomInput;
    private Vector2 rotateInput;

    private void OnEnable()
    {
        if (inputManager != null)
        {
            inputManager.OnPanDetected += HandlePan;
            inputManager.OnZoomDetected += HandleZoom;
            inputManager.OnRotateDetected += HandleRotate;
        }
    }

    private void OnDisable()
    {
        if (inputManager != null)
        {
            inputManager.OnPanDetected -= HandlePan;
            inputManager.OnZoomDetected -= HandleZoom;
            inputManager.OnRotateDetected -= HandleRotate;
        }
    }

    private void Update()
    {
        Move();
        Rotate();
    }

    private void HandlePan(Vector2 delta)
    {
        panInput = delta;
    }

    private void HandleZoom(float delta)
    {
        zoomInput = delta;
    }

    private void HandleRotate(Vector2 drag)
    {
        rotateInput = drag;
    }

    private void Move()
    {
        Vector3 move = Vector3.zero;

        move += transform.right * (panInput.x * panSpeed * Time.deltaTime);
        move += transform.up * (panInput.y * panSpeed * Time.deltaTime);
        move += transform.forward * (zoomInput * zoomSpeed * Time.deltaTime);

        transform.position += move;

        panInput = Vector2.zero;
        zoomInput = 0f;
    }

    private void Rotate()
    {
        float yaw = rotateInput.x * rotateSpeed;
        float pitch = -rotateInput.y * rotateSpeed;

        transform.eulerAngles += new Vector3(pitch, yaw, 0);

        rotateInput = Vector2.zero;
    }
}
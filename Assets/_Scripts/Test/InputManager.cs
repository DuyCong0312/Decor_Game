using System;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class InputManager : MonoBehaviour
{
    public event Action<Vector2> OnPanDetected;
    public event Action<float> OnZoomDetected;
    public event Action<Vector2> OnRotateDetected; 
    
    [Header("Debug")]
    [SerializeField] private bool useKeyboard;

    [Header("Mouse Settings")]
    [SerializeField] private float deadZoneWidth = 300f;
    [SerializeField] private float deadZoneHeight = 200f;

    [Header("Touch Settings")]
    [SerializeField] private float zoomThreshold = 5f;
    [SerializeField] private float touchDeadZone = 0.1f;
    [SerializeField] private float gestureDotThreshold = 0.7f;

    private InputActionsSystem inputActions; 

    private void Awake()
    {
        inputActions = new InputActionsSystem();
    }

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable(); 
        inputActions.MainCamera.Enable();
    }

    private void OnDisable()
    {
        EnhancedTouchSupport.Disable();
        inputActions.MainCamera.Disable();
    }

    private void Update()
    {
        if (useKeyboard)
        {
            HandleKeyboardInput();
            HandleMouseInput();
        }
        else
        {
            HandleTouchInput();
        }
    }

    private void HandleKeyboardInput()
    {
        Vector3 move = inputActions.MainCamera.Move.ReadValue<Vector3>().normalized; 
        OnPanDetected?.Invoke(new Vector2(move.x, move.y));
        OnZoomDetected?.Invoke(move.z);
    }
    private bool IsMouseInWindow()
    {
        Vector2 mousePos = inputActions.MainCamera.Rotate.ReadValue<Vector2>();
        return mousePos.x >= 0 && mousePos.x <= Screen.width &&
               mousePos.y >= 0 && mousePos.y <= Screen.height &&
               mousePos != Vector2.zero;
    }

    private void HandleMouseInput()
    {
        if (!IsMouseInWindow()) return;
        Vector2 mousePos = inputActions.MainCamera.Rotate.ReadValue<Vector2>();
        Debug.Log(mousePos);

        if (mousePos.x < 0 || mousePos.x > Screen.width ||
            mousePos.y < 0 || mousePos.y > Screen.height)
            return;

        float screenCenterX = Screen.width / 2f;
        float screenCenterY = Screen.height / 2f;

        float halfW = deadZoneWidth / 2f;
        float halfH = deadZoneHeight / 2f;

        float deltaX = 0f;
        float deltaY = 0f;

        if (mousePos.x < screenCenterX - halfW)
            deltaX = mousePos.x - (screenCenterX - halfW);
        else if (mousePos.x > screenCenterX + halfW)
            deltaX = mousePos.x - (screenCenterX + halfW);

        if (mousePos.y < screenCenterY - halfH)
            deltaY = mousePos.y - (screenCenterY - halfH);
        else if (mousePos.y > screenCenterY + halfH)
            deltaY = mousePos.y - (screenCenterY + halfH);

        if (deltaX != 0 || deltaY != 0)
        {
            OnRotateDetected?.Invoke(new Vector2(deltaX, deltaY));
        }
    }

    void OnGUI()
    {
        float x = (Screen.width - deadZoneWidth) / 2;
        float y = (Screen.height - deadZoneHeight) / 2;

        GUI.Box(new Rect(x, y, deadZoneWidth, deadZoneHeight), "");
    }

    private void HandleTouchInput()
    {
        var touches = Touch.activeTouches;

        if (touches.Count == 1)
        {
            HandlePan(touches[0]);
        }
        else if (touches.Count == 2)
        {
            HandleTwoFingerGesture(touches[0], touches[1]);
        }
    }

    private void HandlePan(Touch touch)
    {
        Vector2 delta = touch.delta;

        if (delta.magnitude < touchDeadZone)
        {
            OnPanDetected?.Invoke(Vector2.zero);
            return;
        }

        OnPanDetected?.Invoke(delta);
    }

    private void HandleTwoFingerGesture(Touch t0, Touch t1)
    {
        Vector2 d0 = t0.delta;
        Vector2 d1 = t1.delta;

        if (d0.magnitude < touchDeadZone || d1.magnitude < touchDeadZone)
            return;

        float dot = Vector2.Dot(d0.normalized, d1.normalized);

        if (dot > gestureDotThreshold)
        {
            HandleRotate(d0, d1);
        }
        else if (dot < -gestureDotThreshold)
        {
            HandleZoom(t0, t1);
        }
    }

    private void HandleRotate(Vector2 d0, Vector2 d1)
    {
        Vector2 avg = (d0 + d1) * 0.5f;
        OnRotateDetected?.Invoke(avg);
    }

    private void HandleZoom(Touch t0, Touch t1)
    {
        float prevDist = Vector2.Distance(
            t0.screenPosition - t0.delta,
            t1.screenPosition - t1.delta
        );
        float currDist = Vector2.Distance(
            t0.screenPosition,
            t1.screenPosition
        );
        float zoomDelta = currDist - prevDist;

        if (Mathf.Abs(zoomDelta) > zoomThreshold)
        {
            OnZoomDetected?.Invoke(zoomDelta);
        }
    }
}

using System;
using Unity.Cinemachine;
using UnityEditor.Rendering;
using UnityEngine;

public enum PanDirection
    {
        None,
        Left,
        Right,
        Up,
        Down
    } 

public class PanZoomRotate : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private CinemachineOrbitalFollow orbitalFollow;

    [Header("Zoom")]
    [SerializeField] private float zoomSpeed = 0.1f;
    [SerializeField] private float minZoom = 20.0f;
    [SerializeField] private float maxZoom = 50.0f; 
    private float zoomThreshold = 5f;

    [Header("Pan")]
    [SerializeField] private float panSpeed = 0.5f;
    [SerializeField] private float panLimitX = 10.0f;
    [SerializeField] private float panLimitY = 10.0f; 
    private PanDirection panDirection = PanDirection.None;


    [Header("Rotate")]
    [SerializeField] private float rotateSpeed = 0.5f;
    [SerializeField] private float minPitch = -20f;
    [SerializeField] private float maxPitch = 80f;
    private float rotateThreshold = 1.5f;

    private Vector2 touchStart;

    private void Update()
    {
        DetectPanDirection();

        if (panDirection != PanDirection.None)
        {
            Pan(panDirection);
        }
        else if (Input.touchCount == 2)
        {
            panDirection = PanDirection.None;
            HandleTwoFingerGesture();
        }
    }
    private void HandleTwoFingerGesture()
    {
        Touch t0 = Input.GetTouch(0);

        if (t0.phase == TouchPhase.Began)
        {
            touchStart = t0.position;
        }

        Touch t1 = Input.GetTouch(1);

        Vector2 t0Prev = t0.position - t0.deltaPosition;
        Vector2 t1Prev = t1.position - t1.deltaPosition;

        float prevDist = Vector2.Distance(t0Prev, t1Prev);
        float currentDist = Vector2.Distance(t0.position, t1.position);
        float delta = currentDist - prevDist;

        Vector2 avgDelta = (t0.deltaPosition + t1.deltaPosition) * 0.5f;

        if (Mathf.Abs(delta) > zoomThreshold && Mathf.Abs(delta) > avgDelta.magnitude)
        {
            Zoom(delta);
            return;
        }
        if (avgDelta.magnitude > rotateThreshold)
        {
            Rotate(avgDelta);
        }

    }
    private void Zoom(float delta)
    {
        orbitalFollow.RadialAxis.Value -= delta * zoomSpeed;
    }

    private void Rotate(Vector2 drag)
    {
        orbitalFollow.HorizontalAxis.Value += drag.x * rotateSpeed;
        orbitalFollow.VerticalAxis.Value += drag.y * rotateSpeed;
        //if (Mathf.Abs(drag.x) > Mathf.Abs(drag.y))
        //{
        //    orbitalFollow.HorizontalAxis.Value += drag.x * rotateSpeed;
        //}
        //else
        //{
        //    orbitalFollow.VerticalAxis.Value -= drag.y * rotateSpeed;
        //}
    }

    private void DetectPanDirection()
    {
        if (Input.touchCount != 1) return;

        Touch t = Input.GetTouch(0);

        if (t.phase == TouchPhase.Began)
        {
            touchStart = t.position;
            panDirection = PanDirection.None;
        }
        else if (t.phase == TouchPhase.Moved)
        {
            Vector2 delta = t.position - touchStart;

            if (delta.magnitude < 30f) return;

            if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
            {
                panDirection = delta.x > 0 ? PanDirection.Right : PanDirection.Left;
            }
            else
            {
                panDirection = delta.y > 0 ? PanDirection.Up : PanDirection.Down;
            }
        }
        else if (t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled || t.phase == TouchPhase.Stationary) 
        {
            panDirection = PanDirection.None;
        }
    }

    private void Pan(PanDirection dir)
    {
        Vector3 move = Vector3.zero;

        switch (dir)
        {
            case PanDirection.Right:
                move = -cameraTarget.transform.right;
                break;
            case PanDirection.Left:
                move = cameraTarget.transform.right;
                break;
            case PanDirection.Up:
                move = -cameraTarget.transform.up;
                break;
            case PanDirection.Down:
                move = cameraTarget.transform.up;
                break;
        }

        cameraTarget.position += move * panSpeed * Time.deltaTime;
    }

}

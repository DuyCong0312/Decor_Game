using UnityEngine;

public class ResizeArrow : MonoBehaviour
{
    [SerializeField] private ResizeOneDirection target;
    [SerializeField] private Vector3 direction;
    [SerializeField] private Transform[] anotherArrow;

    private Vector2 lastInputPosition;
    private bool isDragging = false;
    private float sensitivity = 0.01f; 
    private Vector3 screenDirection;
    private Camera cam; 

    void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (Input.touchCount == 0) return;

        Touch touch = Input.GetTouch(0);
        switch (touch.phase)
        {
            case TouchPhase.Began:
                Ray ray = cam.ScreenPointToRay(touch.position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform == transform)
                    {
                        isDragging = true;

                        screenDirection =
                            cam.WorldToScreenPoint(transform.position + direction) -
                            cam.WorldToScreenPoint(transform.position);

                        screenDirection.Normalize();
                    }
                }

                lastInputPosition = touch.position;
                break;

            case TouchPhase.Moved:
                if (isDragging)
                {
                    Vector3 delta = touch.position - lastInputPosition;

                    float dragAmount = Vector3.Dot(delta, screenDirection);

                    float movement = dragAmount * sensitivity;

                    if (target.Resize(direction, movement))
                    {
                        transform.Translate(direction.normalized * movement, Space.World);
                        MoveAnotherArrow(direction, movement);
                    }

                    lastInputPosition = touch.position;
                }
                break;

            case TouchPhase.Ended:
            case TouchPhase.Canceled:
                isDragging = false;
                break;
        }
    }

    private void MoveAnotherArrow(Vector3 direction, float delta)
    {
        for (int i = 0; i < anotherArrow.Length; i++)
        {
            anotherArrow[i].localPosition += direction * (delta / 2f);
        }
    }
}

using UnityEngine;

public class ObjFollowMouse : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;

    private Vector3 mousePosition;

    private void Update()
    {
        GetMousePosition();

        if(Input.touchCount > 0)
        {
            this.transform.position = mousePosition + new Vector3(0, 0.25f, 0);
        }
    }

    private void GetMousePosition()
    {
        if (Input.touchCount == 0) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);

        if (Physics.Raycast(ray, out RaycastHit hit , 100f, groundLayer))
        {   
            mousePosition = hit.point;
            //if (hit.transform == transform)
            //{
                
            //}
        }
    }

    public void MoveDown(float desiredY)
    {
        float smoothSpeed = 5f;
        if (transform.position.y > desiredY)
        {
            Vector3 pos = transform.position;
            pos.y = Mathf.Lerp(pos.y, desiredY, smoothSpeed * Time.deltaTime);
            transform.position = pos;
        }
        else
        {
            return;
        }
    }
}

using UnityEngine;
using UnityEngine.UI;

public class ResizeOneDirection : MonoBehaviour
{
    [SerializeField] private float minScale = 2f;

    public bool Resize(Vector3 direction, float delta)
    {
        Vector3 scaleChange = new Vector3(Mathf.Abs(direction.x) * delta, Mathf.Abs(direction.y) * delta, Mathf.Abs(direction.z) * delta);

        Vector3 newScale = transform.localScale + scaleChange;

        if (newScale.x < minScale || newScale.y < minScale || newScale.z < minScale / 8f)
            return false;

        transform.localPosition += direction * (delta / 2f);
        transform.localScale = newScale;

        return true;
    }
}
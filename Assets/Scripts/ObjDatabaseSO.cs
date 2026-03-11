using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObjectData", menuName = "Scriptable Object/ObjectData")]
public class ObjDatabaseSO : ScriptableObject
{
    [SerializeField] private int objectID;
    [SerializeField] private string objectName;
    [SerializeField] private Vector3 objectSize;
    [SerializeField] private bool canPutObjectOnThis = false;
    [SerializeField] private GameObject objectPrefab;
    
    [Header("Placement Rules")]
    [SerializeField] private LayerMask allowedSurfaces;

    public int ObjectID => objectID;
    public Vector3 ObjectSize => objectSize;
    public bool CanPutObjectOnThis => canPutObjectOnThis; 
    public GameObject ObjectPrefab => objectPrefab; 
    public LayerMask AllowedSurfaces => allowedSurfaces;


    [ContextMenu("Calculate Size From Colliders")]
    private void CalculateSizeFromColliders()
    {
#if UNITY_EDITOR
        if (objectPrefab == null) { Debug.LogWarning("No prefab assigned"); return; }

        // Temporarily spawn it to get real bounds
        GameObject temp = UnityEditor.PrefabUtility.InstantiatePrefab(objectPrefab) as GameObject;

        Collider[] colliders = temp.GetComponentsInChildren<Collider>();
        if (colliders.Length == 0)
        {
            Debug.LogWarning("No colliders found");
            DestroyImmediate(temp);
            return;
        }

        Bounds combined = colliders[0].bounds;
        for (int i = 1; i < colliders.Length; i++)
            combined.Encapsulate(colliders[i].bounds);

        objectSize = combined.size;

        DestroyImmediate(temp); // clean up immediately

        UnityEditor.EditorUtility.SetDirty(this);
        UnityEditor.AssetDatabase.SaveAssets();
        Debug.Log($"Size calculated and saved: {objectSize}");
#endif
    }
}
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private ObjDatabaseListSO objList;

    private Camera cam;
    private GameObject targetObject;
    private ObjDatabaseSO objDatabaseGetHit;
    private ObjDatabaseSO selectedObjDatabase;

    private bool hitObjectLayer = false;
    private bool hitWallLayer = false;
    private bool hitGroundLayer = false;

    private int objectLayer;
    private int wallLayer;
    private int groundLayer;

    private float distanceToTarget = 0.01f;

    private void Start()
    {
        cam = Camera.main;

        objectLayer = LayerMask.NameToLayer("Object");
        wallLayer = LayerMask.NameToLayer("Wall");
        groundLayer = LayerMask.NameToLayer("Ground");
    }

    private void Update()
    {
        CheckTypeOfObject();
        CheckLegit();
    }

    private void CheckLegit()
    {
        if (!hitObjectLayer || targetObject == null || selectedObjDatabase == null) return;

        Debug.Log("Check0");
        float maxDistance = 10f;
        if (!Physics.Raycast(targetObject.transform.position, Vector3.down, out RaycastHit hitInfo, maxDistance)) return;

        Debug.Log("Check1");
        if (!CheckObjectHasInObjDatabase(hitInfo.collider.gameObject, out objDatabaseGetHit)){ Debug.Log("Bug"); return; }

        Debug.Log("Check2");

        if (!objDatabaseGetHit.CanPutObjectOnThis) return;

        Debug.Log("Check3");
        bool surfaceAllowed = (selectedObjDatabase.AllowedSurfaces & (1 << hitInfo.collider.gameObject.layer)) != 0;
        if (!surfaceAllowed) return;

        Debug.Log("Check4");
        bool fitsOnSurface = selectedObjDatabase.ObjectSize.x <= objDatabaseGetHit.ObjectSize.x
              && selectedObjDatabase.ObjectSize.z <= objDatabaseGetHit.ObjectSize.z;
        if (!fitsOnSurface) return;

        Debug.Log("Check5");

        Vector3 halfExtents = selectedObjDatabase.ObjectSize * 0.5f;
        Collider[] hits = Physics.OverlapBox(targetObject.transform.position, halfExtents, targetObject.transform.rotation);
        foreach (Collider col in hits)
        {
            if (col.gameObject == targetObject) continue;

            if (col.gameObject != hitInfo.collider.gameObject)
            {
                Debug.Log("Position occupied by: " + col.gameObject.name);
                return;
            }
        }

        Debug.Log("Check6");
        ObjFollowMouse selectedObj = targetObject.GetComponent<ObjFollowMouse>();

        float distance = hitInfo.point.y + distanceToTarget;
        selectedObj.MoveDown(distance);
    }

    private void CheckTypeOfObject()
    {
        hitObjectLayer = false;
        hitWallLayer = false;
        hitGroundLayer = false;
        targetObject = null;
        selectedObjDatabase = null;

        if (Input.touchCount == 0) return;

        Touch touch = Input.GetTouch(0);
        Ray ray = cam.ScreenPointToRay(touch.position);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit))
        {
            int layer = hit.collider.gameObject.layer;
            if (layer == objectLayer)
            {
                hitObjectLayer = true;
                CheckObjectHasInObjDatabase(hit.collider.gameObject, out selectedObjDatabase);
                targetObject = hit.collider.gameObject;
            }
            else if (layer == wallLayer)
            {
                hitWallLayer = true;
                CheckObjectHasInObjDatabase(hit.collider.gameObject, out selectedObjDatabase);
                targetObject = hit.collider.gameObject;
            }
            else if (layer == groundLayer)
            {
                hitGroundLayer = true;
                CheckObjectHasInObjDatabase(hit.collider.gameObject, out selectedObjDatabase);
                targetObject = hit.collider.gameObject;
            }
        }
    }

    private bool CheckObjectHasInObjDatabase(GameObject obj, out ObjDatabaseSO objData)
    {
        ObjID id = obj.GetComponent<ObjID>();
        if (id == null)
        {
            objData = null;
            return false;
        }

        foreach (ObjDatabaseSO objDatabase in objList.ObjectsData)
        {
            if (id.objID == objDatabase.ObjectID)
            {
                objData = objDatabase;
                return true;
            }
        }
        objData = null;
        return false;
    }
}


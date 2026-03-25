using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObjectDataList", menuName = "Scriptable Object/ObjectDataList")]
public class ObjDatabaseListSO : ScriptableObject
{
    [SerializeField] private List<ObjDatabaseSO> objectsData;
    private ObjDatabaseSO selectedObject;

    public List<ObjDatabaseSO> ObjectsData => objectsData;

    public void SetSelectedObjDatabase(ObjDatabaseSO objDatabase)
    {
        selectedObject = objDatabase;
    }

    public ObjDatabaseSO GetSelectedObjDatabase()
    {
        return selectedObject;
    }
}

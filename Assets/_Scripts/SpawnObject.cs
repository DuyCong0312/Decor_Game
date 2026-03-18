using UnityEngine;
using UnityEngine.UI;

public class SpawnObject : MonoBehaviour
{
    [SerializeField] private ObjDatabaseListSO ObjDatabaseList;
    [SerializeField] private Button[] spawnButtons;
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;

    private int currentPage = 0;
    private int itemsPerPage = 5;

    private void Start()
    {
        leftButton.onClick.AddListener(PageLeft);
        rightButton.onClick.AddListener(PageRight);

        for (int i = 0; i < spawnButtons.Length; i++)
        {
            int index = i;
            spawnButtons[i].onClick.AddListener(() => Spawn(index));
        }

        RefreshButtons();
    }

    private void Spawn(int buttonIndex)
    {
        int dataIndex = currentPage * itemsPerPage + buttonIndex;
        if (dataIndex >= ObjDatabaseList.ObjectsData.Count) return;
        Instantiate(ObjDatabaseList.ObjectsData[dataIndex].ObjectPrefab);
    }

    private void PageLeft()
    {
        if (currentPage <= 0) return;
        currentPage--;
        RefreshButtons();
    }

    private void PageRight()
    {
        int totalPages = Mathf.CeilToInt((float)ObjDatabaseList.ObjectsData.Count / itemsPerPage);
        if (currentPage >= totalPages - 1) return;
        currentPage++;
        RefreshButtons();
    }

    private void RefreshButtons()
    {
        for (int i = 0; i < spawnButtons.Length; i++)
        {
            int dataIndex = currentPage * itemsPerPage + i;
            bool hasData = dataIndex < ObjDatabaseList.ObjectsData.Count;

            spawnButtons[i].gameObject.SetActive(hasData);
        }

        leftButton.interactable = currentPage > 0;
        int totalPages = Mathf.CeilToInt((float)ObjDatabaseList.ObjectsData.Count / itemsPerPage);
        rightButton.interactable = currentPage < totalPages - 1;
    }
}
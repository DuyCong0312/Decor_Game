using UnityEngine;
using UnityEngine.UI;

public class ShopUIController : MonoBehaviour
{
    [SerializeField] private ObjDatabaseListSO database;
    [SerializeField] private CartUI cartUI;
    [SerializeField] private ItemPreviewPanel previewPanel;

    [SerializeField] private Button[] itemButtons;
    [SerializeField] private Button prevButton;
    [SerializeField] private Button nextButton;

    private int currentPage = 0;
    private int itemsPerPage = 5;

    private void Start()
    {
        prevButton.onClick.AddListener(GoToPreviousPage);
        nextButton.onClick.AddListener(GoToNextPage);

        for (int i = 0; i < itemButtons.Length; i++)
        {
            int index = i;
            itemButtons[i].onClick.AddListener(() => OnItemButtonClicked(index));
        }

        RefreshUI();
    }

    private void OnItemButtonClicked(int buttonIndex)
    {
        int dataIndex = currentPage * itemsPerPage + buttonIndex;
        if (dataIndex >= database.ObjectsData.Count) return;
        previewPanel.OpenPreviewUI(database.ObjectsData[dataIndex], cartUI);
    }

    private void GoToPreviousPage()
    {
        if (currentPage <= 0) return;
        currentPage--;
        RefreshUI();
    }

    private void GoToNextPage()
    {
        int totalPages = Mathf.CeilToInt((float)database.ObjectsData.Count / itemsPerPage);
        if (currentPage >= totalPages - 1) return;
        currentPage++;
        RefreshUI();
    }

    private void RefreshUI()
    {
        for (int i = 0; i < itemButtons.Length; i++)
        {
            int dataIndex = currentPage * itemsPerPage + i;
            bool hasData = dataIndex < database.ObjectsData.Count;

            itemButtons[i].gameObject.SetActive(hasData);
        }

        prevButton.interactable = currentPage > 0;
        int totalPages = Mathf.CeilToInt((float)database.ObjectsData.Count / itemsPerPage);
        nextButton.interactable = currentPage < totalPages - 1;
    }
}


using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemPreviewPanel : MonoBehaviour
{
    [Header("Preview")]
    [SerializeField] private Transform previewAnchor;
    private GameObject currentPreviewInstance = null;

    [Header("UI elements")]
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemPrice;
    [SerializeField] private TextMeshProUGUI itemLength;
    [SerializeField] private TextMeshProUGUI itemWidth;
    [SerializeField] private TextMeshProUGUI itemHeight;
    [SerializeField] private Button addButton;
    [SerializeField] private Button cancelButton;

    private CartUI cartUI;
    private ObjDatabaseSO item;

    private void Start()
    {
        Hide();
        addButton.onClick.AddListener(() => OnAddButtonClicked());
        cancelButton.onClick.AddListener(() => OnCancelButtonClicked());
    }

    public void OpenPreviewUI(ObjDatabaseSO item, CartUI cart)
    {
        this.cartUI = cart;
        this.item = item;

        if (currentPreviewInstance == null)
        {
            currentPreviewInstance = Instantiate(item.ObjectPrefab, previewAnchor);
        }
        UpdateUI();
        Show();
    }

    private void UpdateUI()
    {
        itemName.text = item.ObjectName;
        itemPrice.text = item.ObjectPrice.ToString();
        itemLength.text = "Length :" + "" + item.ObjectSize.z + "(m)";
        itemWidth.text = "Width :" + "" + item.ObjectSize.x + "(m)";
        itemHeight.text = "Height :" + "" + item.ObjectSize.y + "(m)";
    }

    private void OnAddButtonClicked()
    {
        cartUI.AddItem(item);
        Hide();
        RefreshCurrentPreview();
    }

    private void OnCancelButtonClicked()
    {
        Hide();
        RefreshCurrentPreview();
    }

    private void RefreshCurrentPreview()
    {
        if (currentPreviewInstance != null)
        {
            currentPreviewInstance.SetActive(false);
        }
        currentPreviewInstance = null;
    }

    private void Show()
    {
        this.gameObject.SetActive(true);
    }

    private void Hide()
    {
        this.gameObject.SetActive(false);
    }
}

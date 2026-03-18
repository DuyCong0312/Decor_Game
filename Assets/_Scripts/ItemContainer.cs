using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemContainer : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private float itemPrice;
    [SerializeField] private Button getItemButton;
    [SerializeField] private Button cancelItemButton;

    private ObjDatabaseSO item;
    private ItemListBar itemList;

    public void Initialize(ObjDatabaseSO item, ItemListBar list)
    {
        this.item = item;
        this.itemList = list;
        UpdateUI();
    }

    private void Start()
    {
        getItemButton.onClick.AddListener(() => GetItem());
        cancelItemButton.onClick.AddListener(() => CancelItem());
    }

    private void UpdateUI()
    {
        itemName.text = item.ObjectName;
    }

    private void GetItem()
    {
        itemList.RemoveItem(item);
        Instantiate(item.ObjectPrefab);
        itemList.Hide();
        this.gameObject.SetActive(false);
    }

    private void CancelItem()
    {
        itemList.RemoveItem(item);
        this.gameObject.SetActive(false);
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CartItemUI : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemPrice;
    [SerializeField] private Button spawnButton;
    [SerializeField] private Button removeButton;

    private ObjDatabaseSO itemData;
    private CartUI cartUI;
    private ExpenseTrackerUI expenseTrackerUI;

    public void Initialize(ObjDatabaseSO item, CartUI cart, ExpenseTrackerUI expenseTracker)
    {
        this.itemData = item;
        this.cartUI = cart;
        this.expenseTrackerUI = expenseTracker;
        UpdateUI();
    }

    private void Start()
    {
        spawnButton.onClick.AddListener(() => OnSpawnClicked());
        removeButton.onClick.AddListener(() => OnRemoveClicked());
    }

    private void UpdateUI()
    {
        itemNameText.text = itemData.ObjectName;
        itemPrice.text = itemData.ObjectPrice.ToString();
    }

    private void OnSpawnClicked()
    {
        cartUI.RemoveItem(itemData);
        Instantiate(itemData.ObjectPrefab);
        expenseTrackerUI.AddExpense(itemData.ObjectPrice);
        cartUI.Hide();
        this.gameObject.SetActive(false);
    }

    private void OnRemoveClicked()
    {
        cartUI.RemoveItem(itemData);
        this.gameObject.SetActive(false);
    }
}

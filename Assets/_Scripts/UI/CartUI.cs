using System.Collections.Generic;
using UnityEngine;

public class CartUI : MonoBehaviour
{
    [SerializeField] private List<ObjDatabaseSO> cartItems;
    [SerializeField] private RectTransform contentRoot;
    [SerializeField] private GameObject cartItemPrefab;
    [SerializeField] private ExpenseTrackerUI expenseTrackerUI;

    public void AddItem(ObjDatabaseSO item)
    {
        cartItems.Add(item);
        GameObject newItem = Instantiate(cartItemPrefab, contentRoot);
        CartItemUI container = newItem.GetComponent<CartItemUI>();
        container.Initialize(item, this, expenseTrackerUI);
    }

    public void RemoveItem(ObjDatabaseSO item)
    {
        cartItems.Remove(item);
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }
}

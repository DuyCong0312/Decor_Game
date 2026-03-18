using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ItemListBar : MonoBehaviour
{
    [SerializeField] private List<ObjDatabaseSO> items;
    [SerializeField] private RectTransform itemContainerParent;
    [SerializeField] private GameObject itemContainer;

    public void AddItem(ObjDatabaseSO item)
    {
        items.Add(item); 
        GameObject newItem = Instantiate(itemContainer, itemContainerParent);
        ItemContainer container = newItem.GetComponent<ItemContainer>();
        container.Initialize(item, this);
    }

    public void RemoveItem(ObjDatabaseSO item)
    {
        items.Remove(item);
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

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IDeselectHandler
{

    [SerializeField] private ItemSO item;

    [SerializeField] private int id;
    [SerializeField] private string itemName;
    [SerializeField] private int amount;
    [SerializeField] private string description;


    private void Start()
    {
        //id = item.id;

        itemName = item.name;

        description = item.description;
    }


    public void GetInfo(ItemSO _item, bool stackable, int _amount)
    {
        item = _item;
        if (stackable)
        {
            amount = _amount;
        }
    }

    public void ShowInfo()
    {
        UiManager.Instance.ShowItemInfo(itemName, description);
    }

    public void OnDeselect(BaseEventData data)
    {
        Debug.Log("Deslect");
        UiManager.Instance.HideItemInfo();
    }

}

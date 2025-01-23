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
    [SerializeField] private string description;


    private void Start()
    {
        //id = item.id;

        itemName = item.name;

        description = item.description;
    }


    public void GetInfo(ItemSO _item)
    {
        item = _item;
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

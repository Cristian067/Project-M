using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IDeselectHandler
{

    [SerializeField] public ItemSO item;

    [SerializeField] private int id;
    [SerializeField] private string itemName;
    [SerializeField] private int amount = 1;
    [SerializeField] private string description;
    [SerializeField] private GameObject amountTextPanel;
    [SerializeField] private TextMeshProUGUI amountText;


    private void Start()
    {
        transform.localScale = new Vector3(1, 1, 1);
        //id = item.id;

        itemName = item.name;

        description = item.description;
        amountText.text = $"{amount}";
        
        if (amount <= 1)
        {
            amountTextPanel.SetActive(false);
        }
    }


    public void GetInfo(ItemSO _item, bool stackable, int _amount)
    {
        item = _item;
        /*
        if (stackable)
        {
            amount = _amount;
        }
        */
    }

    public void ShowInfo()
    {
        UiManager.Instance.ShowItemInfo(itemName, description);
    }
    public void AddAmount()
    {
        amount++;
    }
    public int GetAmount()
    {
        return amount;
    }

    public void OnDeselect(BaseEventData data)
    {
        //Debug.Log("Deslect");
        UiManager.Instance.HideItemInfo();
    }

}

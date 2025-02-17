using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


using UnityEngine;

using UnityEngine.UI;


public class Inventory : MonoBehaviour
{
    static public Inventory Instance { get; private set; }

    private int souls;

    [SerializeField] private GameObject itemSlot;
    [SerializeField] private Transform itemContainer;
    [SerializeField] private Transform specialItemContainer;

    //Las imagenes donde van a estar los trozos de almas
    [SerializeField] private Image[] soulContainer;

    //Los toggles donde van a estar las marcas con los objetos. //TODO: Hacer que no se vea hasta conseguir el objecto y hacer que el inventario se vea mas bonito
    [SerializeField] private Toggle[] habilitiesCheck;

    [SerializeField] private List<ItemSO> items;
    [SerializeField] private List<ItemSO> specialItems;

    private ItemSO previousItem;
    private GameObject container;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        
    }
    //Mirar cuantos trozos de almas tienes
    private void CheckSouls()
    {
        souls = GameManager.Instance.GetSouls();

        for (int i = 0; i < soulContainer.Length; i++)
        {
            soulContainer[i].color = Color.black;
        }
        for (int i = 0; i < souls; i++)
        {
            soulContainer[i].color = Color.red;
        }
    }
    //Mirar si tiene las habilidades para mostrarlas en el inventario
    private void CheckHabilities()
    {
        habilitiesCheck[0].isOn = GameManager.Instance.GetHabilities("basic");
        habilitiesCheck[1].isOn = GameManager.Instance.GetHabilities("hook");
        habilitiesCheck[2].isOn = GameManager.Instance.GetHabilities("fireball");
        habilitiesCheck[3].isOn = GameManager.Instance.GetHabilities("doblejump");
        habilitiesCheck[4].isOn = GameManager.Instance.GetHabilities("walljump");
    }

    private void CheckItems()
    {
        
        items = items.OrderByDescending(x => x.name).ToList();
        

        for (int i = 0;i < items.Count;i++)
        {

            if (items[i] == previousItem)
            {
                ItemSlot info = container.GetComponent<ItemSlot>();
                info.AddAmount();
            }
            else
            {
                container = Instantiate(itemSlot);
                container.name = items[i].name;
                container.transform.parent = itemContainer.transform;
                ItemSlot slotInfo = container.GetComponent<ItemSlot>();
                //
                //Debug.Log(items[i].name);
                slotInfo.GetInfo(items[i], items[i].stackable, 0);
                Image itemImage = container.GetComponent<Image>();
                itemImage.sprite = items[i].image;
                previousItem = slotInfo.item;
            }
            
        }
        for (int i = 0;i<specialItems.Count;i++)
        {
            GameObject container = Instantiate(itemSlot);
            container.name = specialItems[i].name;
            container.transform.parent = specialItemContainer.transform;
            ItemSlot slotInfo = container.GetComponent<ItemSlot>();
            slotInfo.GetInfo(specialItems[i], false,0);
            Image itemImage = container.GetComponent<Image>();
            itemImage.sprite = specialItems[i].image;
        }
    }
    private void DeleteItemsInUI()
    {
        for (int i = 0; i < itemContainer.childCount;i++)
        {
            Destroy(itemContainer.GetChild(i).gameObject);
        }
        for (int i = 0; i < specialItemContainer.childCount; i++)
        {
            Destroy(specialItemContainer.GetChild(i).gameObject);
        }


    }
    public void GetItem(ItemSO item,bool isSpecial)
    {
        if (isSpecial)
        {
            specialItems.Add(item);
        }
        else
        {
            items.Add(item);
        }

    }

    public List<ItemSO> GetItemsForSave(bool special)
    {
        if (special)
        {
            return specialItems;
        }
        else
        {
            return items;
        }
    }

    

    //Que mire las cosas del inventario cada vez que se active el panel
    private void OnEnable()
    {
        CheckItems();
        CheckSouls();
        CheckHabilities();
    }
    private void OnDisable()
    {
        DeleteItemsInUI();
    }


}

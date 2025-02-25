using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsList : MonoBehaviour
{
    static public ObjectsList Instance { get;private set; }

    private void Awake()
    {
        Instance = this;
    }

    public ItemSO[] items;
    //public ItemSO specialItems;

    public ItemSO GetItemByName(string name)
    {
        foreach (ItemSO item in items)
        {
            if (item.name == name)
            {
                return item;
            }
        }
        return null;
    }



}

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

}

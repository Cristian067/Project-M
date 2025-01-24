using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName ="ItemSO/Item")]

public class ItemSO : ScriptableObject
{

    public Sprite image;
    //public int id;
    //public string itemName;
    public string description;
    public bool stackable;
    public int amount;

    public bool specialItem;

    //[Header("If Special Item:")]

    public string ifSpecialItem;
    public enum Hability
    {
        Melee,
        Hook,
        Fireball,
        Walljump,
        DobleJump
    };
    public Hability habilityToGive;


    


}

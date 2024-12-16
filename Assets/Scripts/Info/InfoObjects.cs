using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoObjects 
{
    static public List<string> objects = new List<string>();
    static public List<int> dead = new List<int>();


    static public void AddToList(string _name, int _dead)
    {
        objects.Add(_name);
        dead.Add(_dead);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerOcjectKilling : MonoBehaviour
{
    [SerializeField] private GameObject[] target;

    [SerializeField] private bool[] active;

    private Stats stats;


    private void Start()
    {
        stats = GetComponent<Stats>();
    }




    private void OnDestroy()
    {
        
            for (int i = 0; i < target.Length; i++)
            {
                target[i].SetActive(active[i]);
            }
        
    }


}

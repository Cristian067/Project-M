using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerOcjectKilling : MonoBehaviour
{
    [SerializeField] private GameObject[] target;

    [SerializeField] private bool[] active;

   


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        for (int i = 0; i < target.Length; i++)
        {
            target[i].SetActive(active[i]);
        }
        
    }


}

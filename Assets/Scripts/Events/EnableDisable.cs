using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableDisable : MonoBehaviour
{
    [SerializeField] private Event1 mainEvent;

    [SerializeField] private GameObject target;
    [SerializeField] private bool enable;



    private void Function()
    {
        
        target.SetActive(enable);
        
        
    }

    public void Use()
    {
        //mainEvent.InProcces(true);
        Function();
        //PlayerMovementV2.Instance.ChangeInteracting(true);
        Debug.Log("Funciona!!!!!!!!!!!!!!!!!!!!!!!!");
    }
}

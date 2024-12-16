using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event1 : MonoBehaviour
{
    //Script en pruebas

    //Pensamiento: Modificar el script para añadir otros scripts en una rray y que hagan lo que tiene dentro de los otros scripts en orden


    [SerializeField] private LayerMask layerToInteract;
    //[SerializeField] private Layer;

    [SerializeField] private PlayerMovementV2 playerMov;


    private enum WhatToDo
    {
        Dialogue,
        CamMove,
        CharacterMove,
        TargetMove,

    }

    [SerializeField] private WhatToDo whatWillDo;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision.gameObject.layer);
        //Debug.Log(layerToInteract.ToString());
        if ((layerToInteract & 1 << collision.gameObject.layer) == 1 << collision.gameObject.layer) //if(collision.gameObject.layer.CompareTo(layerToInteract) << 0 )
        {
            Debug.Log(name);
        }
    }
}

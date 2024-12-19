using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Event1 : MonoBehaviour
{
    //Script en pruebas

    //Pensamiento: Modificar el script para añadir otros scripts en una rray y que hagan lo que tiene dentro de los otros scripts en orden

    private bool alreadyUsed;
    [SerializeField] private LayerMask layerToInteract;
    //[SerializeField] private Layer;

    [SerializeField] private PlayerMovementV2 playerMov;

    private int idx;
    private bool isInProcess;
    //public List<Cutscene> cutscene;
    public MonoBehaviour[] scripts;
    [SerializeField] private float time;

    private void Start()
    {
        if (scripts.Length == 0)
        {
            Debug.LogError($"{name} no puede tener 0 scripts en la array");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (alreadyUsed)
        {
            return;
        }

        idx = 0;
        //Debug.Log(collision.gameObject.layer);
        //Debug.Log(layerToInteract.ToString());
        if ((layerToInteract & 1 << collision.gameObject.layer) == 1 << collision.gameObject.layer) //if(collision.gameObject.layer.CompareTo(layerToInteract) << 0 )
        {
            //PlayerMovementV2.Instance.ChangeInteracting(true);
            if(!isInProcess)
            {
                PlayerMovementV2.Instance.ChangeInteracting(true);
                PlayerMovementV2.Instance.ForceStop();
                isInProcess = true;
                Next();
            }
            
            
        }

    }
    public void Next()
    {
        if (idx < scripts.Length)
        {
            scripts[idx].Invoke("Use", 0f);
            idx++;
            if (idx >= scripts.Length)
            {
                PlayerMovementV2.Instance.ChangeInteracting(false);
                alreadyUsed = true;
                isInProcess = false;
            }
        }
    }
    public void InProcces(bool start)
    {
        isInProcess = start;
    }
}

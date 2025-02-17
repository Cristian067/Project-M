using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Event1 : MonoBehaviour
{
    //Script en pruebas

    //Pensamiento: Modificar el script para añadir otros scripts en una rray y que hagan lo que tiene dentro de los otros scripts en orden

    [SerializeField]private bool alreadyUsed;
    [SerializeField] private int layerToInteractId;
    //[SerializeField] private Layer;

    [SerializeField] private PlayerMovement playerMov;

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
        Debug.Log(collision.gameObject.layer);
        if (alreadyUsed)
        {
            return;
        }

        idx = 0;
        //Debug.Log(collision.gameObject.layer);
        //Debug.Log(layerToInteract.ToString());
        if (collision.gameObject.layer == layerToInteractId) //if(collision.gameObject.layer.CompareTo(layerToInteract) << 0 )
        {
            //PlayerMovementV2.Instance.ChangeInteracting(true);
            if(!isInProcess)
            {
                PlayerMovement.Instance.ChangeInteracting(true);
                Debug.Log("a1");
                PlayerMovement.Instance.ForceStop();
                isInProcess = true;
                Next();
            }
            
            
        }

    }
    public void Next()
    {
        idx++;
        if (idx > scripts.Length)
            {
                PlayerMovement.Instance.ChangeInteracting(false);
                alreadyUsed = true;
                isInProcess = false;
            }
        else
        {
            scripts[idx - 1].Invoke("Use", 0f);
        }
        
            
            
        
        
    }
    public void InProcces(bool start)
    {
        isInProcess = start;
    }
}

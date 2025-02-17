using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TriggerAnimation : MonoBehaviour
{

    [SerializeField] private bool isOnTrigger;
    [SerializeField] private bool isOnCollision;

    [SerializeField] private Animator animator;
    [SerializeField] private Animator targetAnimator;

    [SerializeField] private string tagString;
    [SerializeField] private int layerIdToUse;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Burn()
    {

        Destroy(gameObject);
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision.gameObject.name + "tr");
        if (collision.gameObject.layer == layerIdToUse)
        {
            //Burn();
            animator.SetTrigger("burn");
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log(collision.gameObject.layer + "col  " + layerIdToUse);
        if (collision.gameObject.layer == layerIdToUse)
        {
            //Burn();
            animator.SetTrigger("burn");
        }

    }

    private void OnDestroy()
    {
        if (isOnTrigger && targetAnimator != null)
        {
            targetAnimator.SetTrigger("trigger");
            
        }
        
        if (isOnCollision && targetAnimator != null)
        {
            targetAnimator.SetTrigger("trigger");
            
        }
        
    }




}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Jump : MonoBehaviour
{
    [SerializeField] private Event1 mainEvent;

    [SerializeField] private GameObject target;
    [SerializeField]private Rigidbody2D targetRb;
    [SerializeField] private float jumpForce;

    [SerializeField] private Vector2 posToJump;

    private void Start()
    {
        //mainEvent = FindAnyObjectByType<Event1>();
        targetRb = target.GetComponent<Rigidbody2D>();
    }

    /*
    Jump(Event1 mainEvent, Vector2 moveTo, GameObject target, Rigidbody2D targetRb, float jumpForce, float offset, bool move)
    {
        this.mainEvent = mainEvent;
        this.moveTo = moveTo;
        this.target = target;
        this.targetRb = targetRb;
        this.jumpForce = jumpForce;
        this.offset = offset;
        this.move = move;
    }
    */
    private void JumpTo()
    {   
        targetRb.velocity = Vector2.zero;
        targetRb.velocity = new Vector2(targetRb.velocity.x,jumpForce);
        //PlayerMovementV2.Instance.ChangeInteracting(false);
        mainEvent.Next();

        //StartCoroutine(Finish());
    }
    private IEnumerator Finish()
    {
        yield return new WaitForSeconds(0.05f);
        
        
    }
    public void Use()
    {
        //mainEvent.InProcces(true);
        //move = true;
        //PlayerMovementV2.Instance.ChangeInteracting(true);
        JumpTo();
        Debug.Log("Funciona!!!!!!!!!!!!!!!!!!!!!!!!");
    }

    
}

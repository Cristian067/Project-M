using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField] private Event1 mainEvent;

    [SerializeField] private Vector2 moveTo;

    [SerializeField] private GameObject target;
    [SerializeField]private Rigidbody2D targetRb;
    [SerializeField] private float speed;

    [SerializeField] private float offset = 1;

    private bool move;
    private void Start()
    {
        //mainEvent = FindAnyObjectByType<Event1>();
        targetRb = target.GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        if (move)
        {
            if (target.transform.position.x != moveTo.x)
            {
                MoveToPos(moveTo);
            }
            if (target.transform.position.x >= moveTo.x -offset && target.transform.position.x <= moveTo.x + offset)
            {
                move = false;
                targetRb.velocity = Vector3.zero;
                //PlayerMovementV2.Instance.ChangeInteracting(false);
                mainEvent.Next();
            }

        }
        
    }

    private void MoveToPos(Vector2 posToMove)
    {
        Vector2 pos = new Vector2(target.transform.position.x, 0);
        Vector2 goTo = new Vector2(posToMove.x - pos.x,0);

        
        //Debug.Log(goTo.x + " " + goTo.y);
        //Debug.Log(pos.x + " " + pos.y);
        targetRb.velocity = new Vector2(goTo.normalized.x * speed, targetRb.velocity.y); ; //new Vector2((goTo.normalized) *speed, transform.position.y);
    }

    public void Use()
    {
        //mainEvent.InProcces(true);
        move = true;
        //PlayerMovementV2.Instance.ChangeInteracting(true);
        Debug.Log("Funciona!!!!!!!!!!!!!!!!!!!!!!!!");
    }

}

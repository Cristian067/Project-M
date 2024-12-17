using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField] private Vector2 moveTo;

    [SerializeField] private GameObject target;

    [SerializeField]private Rigidbody2D targetRb;

    [SerializeField] private float speed;

    private bool move;

    private void Start()
    {
        targetRb = target.GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {

        
        
        if (move)
        {
            if (target.transform.position.x != moveTo.x || target.transform.position.y != moveTo.y)
            {
                MoveToPos(moveTo);
            }
            
        }
        /*
        if (target.transform.position.x == moveTo.x || target.transform.position.y == moveTo.y)
        {
            move = false;
            PlayerMovementV2.Instance.ChangeInteracting(false);
        }
        */


    }

    private void MoveToPos(Vector2 posToMove)
    {
        Vector2 pos = new Vector2(target.transform.position.x, target.transform.position.y);
        Vector2 goTo = posToMove - pos;

        targetRb.velocity = Vector2.zero;
        Debug.Log(goTo.x + " " + goTo.y);
        Debug.Log(pos.x + " " + pos.y);
        targetRb.velocity = new Vector2(goTo.normalized.x * speed, targetRb.velocity.y); ; //new Vector2((goTo.normalized) *speed, transform.position.y);
    }

    private void MoveTo(Vector2 posToMove)
    {
        
    }
    public void Use()
    {
        move = true;
        PlayerMovementV2.Instance.ChangeInteracting(true);
        Debug.Log("Funciona!!!!!!!!!!!!!!!!!!!!!!!!");
    }

}

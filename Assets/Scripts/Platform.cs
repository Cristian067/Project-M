using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{

    private BoxCollider2D boxC;

    // Start is called before the first frame update
    void Start()
    {
        boxC = GetComponent<BoxCollider2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Activate(Collider2D player)
    {

        Physics2D.IgnoreCollision(player, boxC);
        //boxC.isTrigger = true;
        StartCoroutine(Desactivate(player));
    }

    private IEnumerator Desactivate(Collider2D player)
    {
        yield return new WaitForSeconds(0.25f);
        Physics2D.IgnoreCollision(player, boxC, false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {/*
        Rigidbody2D collisionRb = collision.GetComponent<Rigidbody2D>();
        if(collisionRb.velocity.y <= 0 && collision.gameObject.tag == "Player")
        {
            boxC.isTrigger = false;
        }
        if (collisionRb.velocity.y >= 0 && collision.gameObject.tag == "Player")
        {
            boxC.isTrigger = true;
        }
        */
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
     //   boxC.isTrigger = true;
    }
}

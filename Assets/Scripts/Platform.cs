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

    public void Activate()
    {
        boxC.isTrigger = true;
        StartCoroutine("Desactivate");
    }

    private IEnumerator Desactivate()
    {
        yield return new WaitForSeconds(0.5f);
        boxC.isTrigger = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Rigidbody2D collisionRb = collision.GetComponent<Rigidbody2D>();
        if(collisionRb.velocity.y <= 0 && collision.gameObject.tag == "Player")
        {
            boxC.isTrigger = false;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        boxC.isTrigger = true;
    }
}

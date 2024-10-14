using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("Disapear");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Rigidbody2D collisionRb = collision.GetComponent<Rigidbody2D>();
        
        if(gameObject.transform.parent.gameObject.tag == "Player")
        {
            if (collision.gameObject.tag == "Enemy")
            {
                Enemy enemy = collision.gameObject.GetComponent<Enemy>();
                enemy.LoseLive(GameManager.Instance.GetPlayerDamage());
                collisionRb.velocity = Vector3.zero;
                collisionRb.AddForce((collision.transform.position - transform.position).normalized * 4000);
            }

            if (collision.gameObject.tag == "Boss")
            {
                Stats stats = collision.gameObject.GetComponent<Stats>();
                stats.LoseLive(GameManager.Instance.GetPlayerDamage());
                collisionRb.velocity = Vector3.zero;
                collisionRb.AddForce((collision.transform.position - transform.position).normalized * 1000);
            }

            PlayerMovement playerMovement = gameObject.transform.parent.gameObject.GetComponent<PlayerMovement>();

            if (playerMovement.GetDirectionLook() == "down")
            {
                Rigidbody2D playerRb = gameObject.transform.parent.gameObject.GetComponent<Rigidbody2D>();
                playerRb.velocity = new Vector2(playerRb.velocity.x, 0);
                playerRb.AddForce(transform.up * 420);
            }
        }

        

        if(collision.gameObject.tag == "Player")
        {
            //Debug.Log("hit");
            GameManager.Instance.LoseLive(1);
            collisionRb.velocity = Vector3.zero;
            collisionRb.AddForce((collision.transform.position - transform.position).normalized * 1000);

        }


    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    private IEnumerator Disapear()
    {
        yield return new WaitForSeconds(0.05f);
        //Destroy(gameObject.transform.parent.gameObject);
        Destroy(gameObject);
    }

    

}

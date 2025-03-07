using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerMovement;

public class Attack : MonoBehaviour
{

    [SerializeField] private float dissapearTime;

    [SerializeField] private float knockback;

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
                collisionRb.AddForce((collision.transform.position - transform.position).normalized * knockback * 4);
                PlayerMovement playerMovement = gameObject.transform.parent.gameObject.GetComponent<PlayerMovement>();

                if (playerMovement.GetDirectionLook() == LookDirection.Down)
                {
                    Rigidbody2D playerRb = gameObject.transform.parent.gameObject.GetComponent<Rigidbody2D>();
                    playerRb.velocity = new Vector2(playerRb.velocity.x, 0);
                    playerRb.AddForce((-transform.right) * 150);
                }
            }

            if (collision.gameObject.tag == "Boss")
            {
                Stats stats = collision.gameObject.GetComponent<Stats>();
                stats.LoseLive(GameManager.Instance.GetPlayerDamage());
                collisionRb.velocity = Vector3.zero;
                collisionRb.AddForce((collision.transform.position - transform.position).normalized * knockback);
                PlayerMovement playerMovement = gameObject.transform.parent.gameObject.GetComponent<PlayerMovement>();

                if (playerMovement.GetDirectionLook() == LookDirection.Down)
                {
                    Rigidbody2D playerRb = gameObject.transform.parent.gameObject.GetComponent<Rigidbody2D>();
                    playerRb.velocity = new Vector2(playerRb.velocity.x, 0);
                    playerRb.AddForce((-transform.right) * 150);
                }
            }

            
        }

        

        if(collision.gameObject.tag == "Player")
        {
            //Debug.Log("hit");
            //GameManager.Instance.LoseLive(1);
            collisionRb.velocity = Vector3.zero;
            //collisionRb.AddForce(
            Vector3 dir = (collision.transform.position - transform.position);
            PlayerMovement.Instance.Damaged(1, dir);

        }


    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    private IEnumerator Disapear()
    {
        yield return new WaitForSeconds(dissapearTime);
        //Destroy(gameObject.transform.parent.gameObject);
        Destroy(gameObject);
    }

    

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerMovement;

public class AttackV2 : MonoBehaviour
{

    [SerializeField] private float dissapearTime;

    [SerializeField] private float knockback;

    private SpriteRenderer sr;


    // Start is called before the first frame update

    private void Awake()
    {
        //sr = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        StartCoroutine(Desactivate());
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Rigidbody2D collisionRb = collision.GetComponent<Rigidbody2D>();
        

        if(collision.gameObject.tag == "Player")
        {
            //Debug.Log("hit");
            //GameManager.Instance.LoseLive(1);
            collisionRb.velocity = Vector3.zero;
            //collisionRb.AddForce(
            Vector3 dir = (collision.transform.position - transform.position);
            PlayerMovement.Instance.Damaged(1, dir);

        }

        if (gameObject.transform.parent.gameObject.tag == "Player")
        {
            if (collision.gameObject.tag == "Enemy")
            {
                EnemyBasics enemy = collision.gameObject.GetComponent<EnemyBasics>();
                //enemy.LoseLive(GameManager.Instance.GetPlayerDamage());
                collisionRb.velocity = Vector3.zero;
                //TODO: Arreglar el porque el enemigo sale disparado hacia arriba en de de un poco atras
                //Debug.Log((collision.transform.position - transform.position).normalized * knockback * 4);
                //collisionRb.velocity = new Vector2((collision.transform.position - transform.position).normalized.x * knockback * 200,3);
                Vector3 dir = (collision.transform.position - PlayerMovement.Instance.GetPosition());
                enemy.Damaged(GameManager.Instance.GetPlayerDamage(), dir);
                PlayerMovement playerMovement = gameObject.transform.parent.gameObject.GetComponent<PlayerMovement>();

                if (playerMovement.GetDirectionLook() == LookDirection.Down)
                {
                    Rigidbody2D playerRb = gameObject.transform.parent.gameObject.GetComponent<Rigidbody2D>();
                    playerRb.velocity = new Vector2(playerRb.velocity.x, 0);
                    playerRb.AddForce((-transform.right) * 250);
                }
            }

            if (collision.gameObject.tag == "Boss")
            {
                //Stats stats = collision.gameObject.GetComponent<Stats>();
                ControlUnit cu = collision.gameObject.GetComponent<ControlUnit>();
               
                collisionRb.velocity = Vector3.zero;
                Vector3 dir = (collision.transform.position - PlayerMovement.Instance.GetPosition());
                cu.Damaged(GameManager.Instance.GetPlayerDamage(), dir);
                

                if (PlayerMovement.Instance.GetDirectionLook() == LookDirection.Down)
                {
                    Rigidbody2D playerRb = gameObject.transform.parent.gameObject.GetComponent<Rigidbody2D>();
                    playerRb.velocity = new Vector2(playerRb.velocity.x, 0);
                    playerRb.AddForce((-transform.right) * 150);
                }
            }


        }


    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    private IEnumerator Desactivate()
    {
        yield return new WaitForSeconds(dissapearTime);
        //Destroy(gameObject.transform.parent.gameObject);
        Destroy(gameObject);
    }

    

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Enemy : MonoBehaviour
{

    [SerializeField] private int lives;

    [SerializeField] private GameObject attack;

    [SerializeField] private string lookDirection;

    private bool attackInCooldown;
    [SerializeField] private float attackCooldown;

    [SerializeField] private float speed;
    [SerializeField] private float maxSpeed;

    [SerializeField] private Vector2 _distance;

    [SerializeField] private float jumpForce;

    [SerializeField] private bool isOnGround;

    public LayerMask layerToJump;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        attackInCooldown = false;
        rb = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        Flip();
        if (lives <= 0)
        {
            Destroy(gameObject);
        }

        
        if (!attackInCooldown)
        {
            Attack();
        }

        

        RaycastHit2D hitGround1 = Physics2D.Raycast(transform.position + new Vector3(-0.5f, -0.51f, 0), Vector3.down, 0.1f, layerToJump);
        RaycastHit2D hitGround2 = Physics2D.Raycast(transform.position + new Vector3(0.5f, -0.51f, 0), Vector3.down, 0.1f, layerToJump);
        //Debug.DrawLine(transform.position + new Vector3(0,-0.51f,0), transform.position + new Vector3(0,-0.61f,0));

        if (hitGround1 || hitGround2)
        {
            isOnGround = true;
        }
        else if (!hitGround1 || !hitGround2)
        {
            isOnGround = false;
        }
        Debug.DrawLine(transform.position + new Vector3(0.5f,0,0), transform.position + transform.right, Color.blue);


        rb.velocity = new Vector2(Mathf.MoveTowards(rb.velocity.x, 0f, 15f * Time.deltaTime), rb.velocity.y);

    }

    private void FixedUpdate()
    {
        RaycastHit2D hitWall = Physics2D.Raycast(transform.position, transform.right, 0.8f, layerToJump);
        if (hitWall)
        {
            Debug.Log(hitWall.collider.gameObject.name);
        }

        if (hitWall && _distance.x != 0 && isOnGround)
        {
            Debug.Log("jenye");
            Jump();
        }
        else if (!hitWall)
        {

        }
        /*
        GameObject target = FindAnyObjectByType<PlayerMovement>().gameObject;


        Vector2 distance = target.transform.position - transform.position;

        if(distance.x > -5 && distance.x < 5)
        {
            rb.velocity = distance;
        }
        */

        //Debug.Log(distance);
    }

    public void LoseLive(int damage)
    {
        lives -= damage;
    }

    public void Attack()
    {
        //yield return new WaitForSeconds(1);

        Vector3 attackDirection = new Vector3(0, 0, 0);
        Vector3 offset = new Vector3(0, 0, 0);
        Quaternion attackRotation = Quaternion.identity;

        if (lookDirection == "left")
        {
            //attackDirection = new Vector3(-1f,0,0);
            offset = new Vector3(-1f, 0, 0);
            attackRotation = transform.rotation;
        }
        else if (lookDirection == "right")
        {
            //attackDirection = new Vector3(1f,0,0);
            offset = new Vector3(1f, 0, 0);
            attackRotation = transform.rotation;
        }
        

        Instantiate(attack, transform.position + attackDirection + offset, attackRotation, this.transform);
        StartCoroutine("AttackCooldown");

    }


    private IEnumerator AttackCooldown()
    {
        attackInCooldown = true;
        yield return new WaitForSeconds(attackCooldown);
        attackInCooldown = false;
    }

    private void Flip()
    {

        if (rb.velocity.x < 0f)
        {
            
                transform.rotation = Quaternion.Euler(transform.rotation.x, 180, transform.rotation.z);
                lookDirection = "left";
            

        }

        if (rb.velocity.x > 0f)
        {
            
            
                transform.rotation = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z);
                lookDirection = "right";
            
        }
        
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            _distance = collision.gameObject.transform.position - transform.position;

            if (_distance != new Vector2(0, 0))
            {
                rb.velocity = new Vector2(_distance.x/2, rb.velocity.y);
            }
            
            

            //Debug.Log(rb.velocity.x);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _distance = new Vector2(0, 0);
            
        }
    }
    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(transform.up * jumpForce);
        isOnGround = false;
    }
}

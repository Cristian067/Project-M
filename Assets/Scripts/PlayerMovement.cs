using System;
using System.Collections;
using System.Collections.Generic;
//using System.Drawing;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField]private Camera cam;

    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;

    [SerializeField] private bool inHook;
    [SerializeField] private bool isOnGround;


    [SerializeField] private float hookDistance;
    [SerializeField] private float hookForce;

    [SerializeField] private GameObject attack;
    private bool attackInCooldown;
    [SerializeField] private float attackCooldown;

    [SerializeField] private string lookDirection;

    //[SerializeField] private float currentSpeed;

    public LayerMask layerToHit;

    private float horizontal;

    private Vector3 hookedPos;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        inHook = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKey("s"))
        {
            lookDirection = "down";
        }
        */
        Flip();

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
        horizontal = Input.GetAxis("Horizontal");

        //horizontalInt = (int)horizontal;

        RaycastHit2D hitGround = Physics2D.Raycast(transform.position + new Vector3(0, -0.51f, 0), Vector3.down,0.1f);
        //Debug.DrawLine(transform.position + new Vector3(0,-0.51f,0), transform.position + new Vector3(0,-0.61f,0));

        if (hitGround)
        {
            isOnGround = true;
        }
        else if(!hitGround)
        {
            isOnGround= false;
        }

        Vector3 direction = (mousePos - transform.position).normalized;

        //DrawLine(transform.position, mousePos, Color.blue);

        if (Input.GetKeyDown("w") && isOnGround)
        {

            //transform.Translate(new Vector3(0,1,0) * jumpForce);
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(transform.up * jumpForce);
            isOnGround = false;

        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && !attackInCooldown)
        {
            Attack();
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Debug.DrawLine(transform.position, mousePos, Color.green);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, hookDistance,layerToHit);
            Debug.DrawLine(transform.position, mousePos, Color.red);
            Debug.Log(hit.collider.name);
            if (hit.collider.gameObject.tag == "Hookable")
            {
                Debug.Log(hit.collider.name);
                Hook(hit, direction);
            }
        }
        /*
        if(currentSpeed > speed)
        {
            currentSpeed= speed;
        }
        if (currentSpeed < -speed)
        {
            currentSpeed = -speed;
        }
        */
        if (!inHook )
        {
            rb.velocity = new Vector2(Mathf.MoveTowards(rb.velocity.x,0f,15f * Time.deltaTime), rb.velocity.y); 
            //currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, 15f * Time.deltaTime);

        }
           //new Vector2(Mathf.Lerp(minimum, maximum, 0), rb.velocity.y);
           /*
        if (currentSpeed != 0f)
        {
            rb.velocity = new Vector2(currentSpeed, rb.velocity.y);
        }
           */

        

    }
    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.A) && !inHook || Input.GetKey(KeyCode.D) && !inHook)
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }
        /*
        if(Input.GetKey("a") && !inHook)
        {
            currentSpeed += -0.5f;
        }
        if (Input.GetKey("d") && !inHook)
        {
            currentSpeed += 0.5f;
        }
        */
    }


    private void Hook(RaycastHit2D hit, Vector2 direction)
    {
        inHook = true;
        rb.velocity = new Vector2(0,0);
        rb.AddForce(direction * hookForce, ForceMode2D.Impulse);
        Debug.Log(hit.collider.gameObject.transform.position);
        hookedPos = hit.collider.transform.position;
        isOnGround = false;
        StartCoroutine("CooldownHook");
        //transform.Translate(hit.transform.position);
    }


    private IEnumerator CooldownHook()
    {
        yield return new WaitForSeconds(0.3f);
        inHook = false;
    }

    private void Attack()
    {
        Vector3 attackDirection = new Vector3(0,0,0);
        Quaternion attackRotation = Quaternion.identity;

        if(lookDirection == "left")
        {
            //attackDirection = new Vector3(-1f,0,0);
            attackRotation = transform.rotation;
        }
        else if(lookDirection == "right")
        {
            //attackDirection = new Vector3(1f,0,0);
            attackRotation = transform.rotation;
        }
        else if(lookDirection == "down")
        {
            attackDirection = new Vector3(0f, 0, 0);
            attackRotation = Quaternion.Euler(0,0,-90);
        }
        
        Instantiate(attack, transform.position + attackDirection, attackRotation, this.transform);
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
            if (Input.GetKey("s") && !isOnGround)
            {
                lookDirection = "down";
            }
            else
            {
                transform.rotation = Quaternion.Euler(transform.rotation.x, 180, transform.rotation.z);
                lookDirection = "left";
            }
            
        }

        if (rb.velocity.x > 0f)
        {
            if (Input.GetKey("s") && !isOnGround)
            {
                lookDirection = "down";
            }
            else
            {
                transform.rotation = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z);
                lookDirection = "right";
            }
        }
        if(rb.velocity.x == 0f && !isOnGround)
        {
            if (Input.GetKey("s"))
            {
                lookDirection = "down";
            }
        }
    }

}

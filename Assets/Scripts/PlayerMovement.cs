using System;
using System.Collections;
using System.Collections.Generic;
//using System.Drawing;
using System.Reflection;

using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance { get; private set; }

    private Rigidbody2D rb;
    //[SerializeField]private Camera cam;

    private bool interacting;

    [Header("Velocidad y salto")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;

    [Header("Booleanas")]
    [SerializeField] private bool inHook;
    [SerializeField] private bool isOnGround;
    public bool canJump;

    [Header("Velocidades")]
    [SerializeField] private float maxSpeed;
    [SerializeField] private float maxTotalSpeed;

    [Header("Gancho")]
    [SerializeField] private float hookDistance;
    [SerializeField] private float hookForce;

    private float inHookSpeed;

    [Header("Ataque basico")]
    [SerializeField] private GameObject attack;
    private bool attackInCooldown;
    [SerializeField] private float attackCooldown;

    [Header("Fireball")]
    [SerializeField] private GameObject fireball;
    [SerializeField] private float fireballCooldown;
    private bool fireballInCooldown;

    [SerializeField] private string lookDirection;

    [Header("Saltos")]
    [SerializeField] private int maxJumps;
    [SerializeField] private int remainingJumps;

    //[SerializeField] private float currentSpeed;

    [Header("Capas de golpe")]
    public LayerMask whatIsHook;
    public LayerMask whatIsGround;

    private float horizontal;

    private Vector3 hookedPos;

    public Transform groundCheck;
    public float groundCheckRadius;


    

    private CapsuleCollider2D cc;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Hay mas de un player");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CapsuleCollider2D>();
        inHook = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.IsPaused() && !interacting)
        {
            Flip();

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            horizontal = Input.GetAxisRaw("Horizontal");

            //Detectar suelo
            //RaycastHit2D hitGround1 = Physics2D.Raycast(transform.position + new Vector3(0, -0.1f, 0), Vector3.down, 0.5f, whatIsGround);
            //RaycastHit2D hitGround2 = Physics2D.Raycast(transform.position + new Vector3(0.5f, -0.51f, 0), Vector3.down, 0.1f, layerToJump);
            //Debug.DrawLine(transform.position + new Vector3(0,-0.51f,0), transform.position + new Vector3(0,-0.61f,0));
            /*
            if (hitGround1 )
            {

                isOnGround = true;
                //inHookSpeed = 0;
                extraJumps = 1;
                //if (rb.gravityScale == 1)
                //{
                 //   rb.gravityScale = 6;
                ////}
            }
            else if (!hitGround1 )
            {
                isOnGround = false;
                //if(rb.gravityScale != 1)
                //{
               //     rb.gravityScale = 1;
                //}
            }
            */


            Vector3 direction = (mousePos - transform.position).normalized;

            //DrawLine(transform.position, mousePos, Color.blue);

            //Saltar
            if (Input.GetKeyDown("space"))
            { 
                Jump();
            }

            //Atacar
            if (Input.GetKeyDown(KeyCode.Mouse0) && !attackInCooldown && GameManager.Instance.GetHabilities("basic"))
            {
                Attack();
            }

            //Usar gancho
            if (Input.GetKeyDown(KeyCode.Mouse1) && !inHook && GameManager.Instance.GetHabilities("hook"))
            {
                Debug.DrawLine(transform.position, mousePos, Color.green);
                RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, hookDistance, whatIsHook);
                Debug.DrawLine(transform.position, mousePos, Color.red);
                Debug.Log(hit.collider.name);
                if (hit.collider.gameObject.tag == "Hookable")
                {
                    Debug.Log(hit.collider.name);
                    Hook(hit, direction);
                }
            }

            //Usar Bola de fuego
            if (Input.GetKeyDown("q") && GameManager.Instance.GetHabilities("fireball"))
            {
                if (!fireballInCooldown)
                {
                    Firevall();
                }
                
            }

            if (Input.GetKeyDown("s"))
            {
                DownActions();
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
            if (!inHook)
            {
                rb.velocity = new Vector2(Mathf.MoveTowards(rb.velocity.x, 0f, 20f * Time.deltaTime), rb.velocity.y);
                inHookSpeed = Mathf.MoveTowards(inHookSpeed, 0f, 15f * Time.deltaTime);
                //currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, 15f * Time.deltaTime);

            }
            //new Vector2(Mathf.Lerp(minimum, maximum, 0), rb.velocity.y);
            /*
         if (currentSpeed != 0f)
         {
             rb.velocity = new Vector2(currentSpeed, rb.velocity.y);
         }
            */

            CheckIfCanJump();
        }

    }
    private void FixedUpdate()
    {
        float currentSpeed = 0;

        if (!interacting)
        {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                currentSpeed += horizontal * speed;

                if (currentSpeed > maxSpeed)
                {
                    currentSpeed = maxSpeed;
                }
                if (currentSpeed < -maxSpeed)
                {
                    currentSpeed = -maxSpeed;
                }
            }  
        }
        
        if (rb.velocity.x > maxTotalSpeed)
        {
            rb.velocity = new Vector2(maxTotalSpeed,rb.velocity.y);
            inHookSpeed = maxTotalSpeed - maxSpeed;
        }
        if (rb.velocity.x < -maxTotalSpeed)
        {
            rb.velocity = new Vector2(-maxTotalSpeed, rb.velocity.y);

            inHookSpeed = -maxTotalSpeed + maxSpeed;
        }
        
        //Debug.Log($"{currentSpeed} hook: {inHookSpeed}. Total {rb.velocity.x}");

        rb.velocity = new Vector2(currentSpeed + inHookSpeed, rb.velocity.y);

        CheckSurrondings();
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

    private void CheckSurrondings()
    {
        isOnGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
    }

    private void CheckIfCanJump()
    {
        if (isOnGround && rb.velocity.y <= 0)
        {
            remainingJumps = maxJumps;
        }
        if(remainingJumps <= 0)
        {
            canJump = false;
        }
        else
        {
            
            canJump = true;
        }
    }
    private void Hook(RaycastHit2D hit, Vector2 direction)
    {
        inHook = true;
        rb.velocity = new Vector2(rb.velocity.x,0);
        rb.AddForce((hit.collider.transform.position - transform.position ).normalized* (hookForce), ForceMode2D.Impulse);
        inHookSpeed = (hit.collider.transform.position.x - transform.position.x);
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
        Vector3 offset = new Vector3(0, 0, 0);
        Quaternion attackRotation = Quaternion.identity;

        if(lookDirection == "left")
        {
            //attackDirection = new Vector3(-1f,0,0);
            offset = new Vector3(-1f, 0, 0);
            attackRotation = transform.rotation;
        }
        else if(lookDirection == "right")
        {
            //attackDirection = new Vector3(1f,0,0);
            offset = new Vector3(1f, 0, 0);
            attackRotation = transform.rotation;
        }
        else if (lookDirection == "up")
        {
            attackDirection = new Vector3(0f, 0, 0);
            offset = new Vector3(0f, 1, 0);
            attackRotation = Quaternion.Euler(0, 0, -90);
        }
        else if(lookDirection == "down")
        {
            attackDirection = new Vector3(0f, 0, 0);
            offset = new Vector3(0f, -1, 0);
            attackRotation = Quaternion.Euler(0,0,-90);
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
        if (Input.GetKey("a"))
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

        if (Input.GetKey("d"))
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
        if (Input.GetKey("w"))
        {
            lookDirection = "up";
        }
    }

    private void Jump()
    {

        if (canJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            remainingJumps--;
        }
        
        //rb.AddForce(transform.up * jumpForce);
        //isOnGround = false;
    }


    private void Firevall()
    {
        Quaternion attackRotation = Quaternion.identity;

        if (lookDirection == "left")
        {
            //attackDirection = new Vector3(-1f,0,0);
            //offset = new Vector3(-1f, 0, 0);
            attackRotation = transform.rotation;
        }
        else if (lookDirection == "right")
        {
            //attackDirection = new Vector3(1f,0,0);
            //offset = new Vector3(1f, 0, 0);
            attackRotation = transform.rotation;
        }
        else if (lookDirection == "down")
        {
            //attackDirection = new Vector3(0f, 0, 0);
            //offset = new Vector3(0f, -1, 0);
            attackRotation = Quaternion.Euler(0, 0, -90);
        }

        if (GameManager.Instance.GetOil() >= 25)
        {
            Instantiate(fireball,transform.transform.transform.transform.transform.transform.transform.transform.transform.transform.position, attackRotation);
            GameManager.Instance.UseFuel(25);
            StartCoroutine(FireballCooldown());
        }

    }
    private IEnumerator FireballCooldown()
    {
        fireballInCooldown = true;
        yield return new WaitForSeconds(fireballCooldown);
        fireballInCooldown = false;
    }
    private void DownActions()
    {

        RaycastHit2D hitDown = Physics2D.Raycast(transform.position + new Vector3(0, -0.6f, 0), Vector3.down,0.3f);
        

        if(hitDown.transform.gameObject.tag == "Platform")
        {
            
            Platform platform = hitDown.transform.gameObject.GetComponent<Platform>();
            platform.Activate(cc);
        }

    }

    public string GetDirectionLook()
    {
        return lookDirection;
    }

    public void changeInteracting(bool condition)
    {
        interacting = condition;
    }
    public bool isInteracting()
    {
        return interacting;
    }

    private void UpdateAnimations()
    {

    }




    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

}

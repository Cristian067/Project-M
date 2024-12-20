using System;
using System.Collections;
using System.Collections.Generic;
//using System.Drawing;
using UnityEngine;


public class PlayerMovementV2 : MonoBehaviour
{
    public static PlayerMovementV2 Instance { get; private set; }

    private Rigidbody2D rb;

    private bool interacting;

    [Header("Debug Check")]
    public float velocityX;
    public float velocityY;


    [Header("Velocidad y salto")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;

    [Header("Booleanas")]
    private bool isDead;
    private bool inHook;
    private bool isOnGround;
    private bool isTouchingWall;
    private bool isWallSliding;
    private bool canJump;
    private bool hookUsed;

    [Header("Habilidades")]
    private bool melee;
    private bool hook;
    private bool fireball;
    private bool dobleJump;
    private bool wallJump;

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
    [SerializeField] private GameObject fireballObject;
    private bool fireballInCooldown;
    [SerializeField] private float fireballCooldown;

    [SerializeField] private string lookDirection;

    [Header("Saltos")]
    [SerializeField] private int maxJumps;
    private int remainingJumps;

    [Header("Capas de golpe")]
    [SerializeField] private LayerMask whatIsHook;
    [SerializeField] private LayerMask whatIsGround;

    [Header("Ground and Wall check")]

    public Transform wallCheck;
    public float wallCheckDistance;
    public float wallSilidingSpeed;
    private Vector2 wallPosition;
    public Transform groundCheck;
    public float groundCheckRadius;
    
    private float horizontal;

    private Vector3 hookedPos;

    private Vector3 mousePos;

    private bool knockback;

    private Animator animator;

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
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        inHook = false;
        CheckHabilities();
    }

    // Update is called once per frame
    void Update()
    {
        velocityX = rb.velocity.x;
        velocityY = rb.velocity.y;
        if (!GameManager.Instance.IsPaused() && !interacting)
        {
            Flip();

            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            horizontal = Input.GetAxisRaw("Horizontal");

            Vector3 direction = (mousePos - transform.position).normalized;

            //DrawLine(transform.position, mousePos, Color.blue);

            //Saltar
            if (Input.GetKeyDown("space"))
            { 
                Jump();
            }

            //Atacar
            if (Input.GetKeyDown(KeyCode.Mouse0) && !attackInCooldown && melee)
            {
                Attack();
            }

            //Usar gancho
            try
            {
                if (Input.GetKeyDown(KeyCode.Mouse1) && !inHook && hook)
                {
                    RaycastHit2D hitHook = Physics2D.Raycast(transform.position, direction, hookDistance, whatIsHook);
                    Debug.DrawLine(transform.position, mousePos, Color.red);
                    RaycastHit2D hitBetween = Physics2D.Raycast(transform.position, direction, hookDistance, whatIsGround);
                    Debug.Log(hitHook.collider.name);

                    if (hitHook)
                    {
                        Debug.Log(hitHook.collider.name);
                        Hook(hitHook, direction);
                    }
                }
            }
            catch { Debug.Log("Gancho no ganchado"); }

            //Usar Bola de fuego
            if (Input.GetKeyDown("q") && fireball)
            {
                if (!fireballInCooldown)
                {
                    Fireball();
                }
            }

            //Usar acciones hacia abajo
            try
            {
                if (Input.GetKeyDown("s"))
                {
                    DownActions();
                }
            } 
            catch 
            { 
                Debug.Log(""); 
            }

            /*
            if (!inHook)
            {
                rb.velocity = new Vector2(Mathf.MoveTowards(rb.velocity.x, 0f, 20f * Time.deltaTime), rb.velocity.y);
                inHookSpeed = Mathf.MoveTowards(inHookSpeed, 0f, 15f * Time.deltaTime);
                //currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, 15f * Time.deltaTime);
            }
            */

            CheckIfCanJump();
            CheckIfWallSliding();
        }

        if (isDead)
        {
            cc.enabled = false;
        }

    }
    private void FixedUpdate()
    {
        if (!GameManager.Instance.IsPaused() && !interacting)
        {
            ApplyMovement();
        }
            
        CheckSurrondings();
    }
    private void Attack()
    {
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
        else if (lookDirection == "up")
        {
            attackDirection = new Vector3(0f, 0, 0);
            offset = new Vector3(0f, 1, 0);
            attackRotation = Quaternion.Euler(0, 0, -90);
        }
        else if (lookDirection == "down")
        {
            attackDirection = new Vector3(0f, 0, 0);
            offset = new Vector3(0f, -1, 0);
            attackRotation = Quaternion.Euler(0, 0, -90);
        }

        Instantiate(attack, transform.position + attackDirection + offset, attackRotation, this.transform);
        StartCoroutine(AttackCooldown());
    }

    private void CheckIfWallSliding()
    {
        if (wallJump)
        {
            if (isTouchingWall && !isOnGround && rb.velocity.y < 0)
            {
                isWallSliding = true;
            }
            else
            {
                isWallSliding = false;
            }
        }
        
    }

    private void ApplyMovement()
    {
        if (!knockback)
        {
            if (!inHook)
            {
                if (!isWallSliding && isOnGround)
                {
                    rb.velocity = new Vector2(speed * horizontal, rb.velocity.y);
                }

                if (!isOnGround && !isWallSliding)
                {
                    rb.velocity = new Vector2(rb.velocity.x + (horizontal * speed), rb.velocity.y);
                }
                if (!isOnGround && isWallSliding)
                {
                    rb.velocity = new Vector2((speed * horizontal)/6, rb.velocity.y);
                }
            }
        
            if (!hookUsed)
            {
                if (rb.velocity.x > maxSpeed)
                {
                    rb.velocity = new Vector2(maxSpeed, rb.velocity.y);
                }

                if (rb.velocity.x < -maxSpeed)
                {
                    rb.velocity = new Vector2(-maxSpeed, rb.velocity.y);
                }
                if (!isOnGround)
                {
                    rb.velocity = new Vector2(Mathf.MoveTowards(rb.velocity.x, 0f, 40f * Time.deltaTime), rb.velocity.y);
                }
            }

            else if (hookUsed)
            {
                if (rb.velocity.x > maxTotalSpeed)
                {
                    rb.velocity = new Vector2(maxTotalSpeed, rb.velocity.y);
                }

                if (rb.velocity.x < -maxTotalSpeed)
                {
                    rb.velocity = new Vector2(-maxTotalSpeed, rb.velocity.y);
                }
            }
        
            if (isWallSliding)
            {
                if (rb.velocity.y < -wallSilidingSpeed)
                {
                    rb.velocity = new Vector2(rb.velocity.x, -wallSilidingSpeed);
                } 

           
            }

        }



    }
    private void CheckSurrondings()
    {
        isOnGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, whatIsGround);

        
        wallPosition = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, whatIsGround).point;
        //Debug.Log(transform.position.x - wallPosition.x);
    }

    private void CheckIfCanJump()
    {
        if (isOnGround && rb.velocity.y <= 0 || isWallSliding)
        {
            remainingJumps = maxJumps;
            hookUsed = false;
        }
        /*
        if(!isOnGround || GameManager.Instance.GetHabilities("doblejump"))
        {
            canJump = true;
        }
        */
        if(remainingJumps <= 0 || !isOnGround && !isWallSliding && !GameManager.Instance.GetHabilities("doblejump"))
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
        hookUsed = true;
        inHook = true;
        rb.velocity = new Vector2(rb.velocity.x,0);
        rb.AddForce((hit.collider.transform.position - transform.position ).normalized* (hookForce), ForceMode2D.Impulse);
        inHookSpeed = (hit.collider.transform.position.x - transform.position.x);
        Debug.Log(hit.collider.gameObject.transform.position);
        hookedPos = hit.collider.transform.position;
        isOnGround = false;
        StartCoroutine(CooldownHook());
        //transform.Translate(hit.transform.position);
    }
    
    private IEnumerator CooldownHook()
    {
        yield return new WaitForSeconds(0.2f);
        inHook = false;
    }
    /*
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
    */
    private IEnumerator AttackCooldown()
    {
        attackInCooldown = true;
        yield return new WaitForSeconds(attackCooldown);
        attackInCooldown = false;
    }
    private void Flip()
    {
        if (!isWallSliding)
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
        else
        {

        }
    }

    private void Flipv2()
    {
        if (!isWallSliding)
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
            if (rb.velocity.x == 0f && !isOnGround)
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
        else
        {

        }
    }

    private void Jump()
    {
        if (canJump && isWallSliding)
        {
            rb.velocity = new Vector2((transform.position.x - wallPosition.x) *10, jumpForce);
            remainingJumps--;
        }

        else if (canJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            remainingJumps--;
        }
    }
    private void Fireball()
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
            Instantiate(fireballObject,transform.transform.transform.transform.transform.transform.transform.transform.transform.transform.position, attackRotation);
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

    public void ChangeInteracting(bool condition)
    {
        interacting = condition;
        horizontal = 0;
        //rb.velocity = new Vector2 (0,0);
    }
    public void ForceStop()
    {
        rb.velocity = Vector2.zero;
    }
    public bool isInteracting()
    {
        return interacting;
    }

    private void UpdateAnimations()
    {

    }

    public void SetAnimationBool(string animName, bool activation)
    {
        animator.SetBool(animName, activation);
    }
    public void SetBools(string boolName, bool activation)
    {

    }
    public void KillPlayer()
    {
        isDead = true;
    }


    public void CheckHabilities()
    {

        melee = GameManager.Instance.GetHabilities("basic");
        hook = GameManager.Instance.GetHabilities("hook");
        fireball = GameManager.Instance.GetHabilities("fireball");
        dobleJump = GameManager.Instance.GetHabilities("doblejump");
        wallJump = GameManager.Instance.GetHabilities("walljump");

        if (dobleJump)
        {
            maxJumps = 2;
        }
    }

    

    public void Damaged(int damage, Vector3 knockbackDir)
    {
        
            GameManager.Instance.LoseLive(damage);
            animator.SetTrigger("isDamaged");

            //Debug.Log(knockbackDir.normalized);
            rb.velocity = (new Vector3(0f, 0.5f, 0) + knockbackDir.normalized) * 20;

            StartCoroutine(KnockbackTime());
            //animator.ResetTrigger("isDamaged");
        
    }

    private IEnumerator KnockbackTime()
    {
        knockback = true;
        yield return new WaitForSeconds(0.5f);
        knockback = false;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);

        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y, wallCheck.position.z));

        Gizmos.DrawLine(transform.position, mousePos);
    }

}

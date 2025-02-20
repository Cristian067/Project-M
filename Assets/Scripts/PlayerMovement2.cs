using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

//using System.Drawing;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance { get; private set; }

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

    public enum LookDirection
    {
        Left,
        Right,
        Up,
        Down
    }
    private LookDirection lookDirection;

    private (int, int) directionCoord;

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
    private float vertical;

    private Vector3 hookedPos;

    private Vector3 mousePos;

    private bool knockback;

    private Animator animator;

    private CapsuleCollider2D cc;

    [SerializeField] private CinemachineVirtualCamera cam;

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
        cam = FindFirstObjectByType<CinemachineVirtualCamera>();
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
            Flipv2();

            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");

            Vector3 direction = (mousePos - transform.position).normalized;

            //DrawLine(transform.position, mousePos, Color.blue);

            //Saltar
            if (InputControl.Jump())
            { 
                Jump();
            }

            //Atacar
            if (InputControl.Attack() && !attackInCooldown && melee)
            {
                Attack();
            }

            //Usar gancho
            try
            {
                /*
                if (Input.GetKeyDown(KeyCode.Mouse1) && !inHook && hook)
                {
                    RaycastHit2D hitHook = Physics2D.Raycast(transform.position, direction, hookDistance, whatIsHook);
                    Debug.DrawLine(transform.position, mousePos, Color.red);
                    RaycastHit2D hitBetween = Physics2D.Raycast(transform.position, direction, hookDistance, whatIsGround);
                    Debug.Log(hitHook.collider.name);

                    if (hitHook)
                    {
                        Debug.Log(hitHook.collider.name);
                        Hook(direction);
                    }
                }*/
            }
            catch { Debug.Log("Gancho no ganchado"); }

            //Usar Bola de fuego
            if (InputControl.Fireball() && fireball)
            {
                if (!fireballInCooldown)
                {
                    Fireball();
                }
            }

            //Usar acciones hacia abajo
            try
            {
                if (InputControl.Down())
                {
                    DownActions();
                }
            } 
            catch 
            { 
                //Debug.Log(""); 
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
            cam.Follow = null;
            // cam.transform.position = cam;
        }

    }
    private void FixedUpdate()
    {
        if (!GameManager.Instance.IsPaused() && !interacting)
        {
            StartCoroutine(ApplyMovement());
        }
            
        CheckSurrondings();
    }
    private void Attack()
    {
        Vector3 attackDirection = new Vector3(0, 0, 0);
        Vector3 offset = new Vector3(directionCoord.Item1, directionCoord.Item2, 0);
        Quaternion attackRotation = Quaternion.identity;
        //offset = new Vector3(directionCoord.Item1, 0, 0);
        attackRotation = transform.rotation;
        
        if (directionCoord.Item2 != 0)
        {
            //directionCoord.Item1 = 0;
            if (isOnGround)
            {
                if (lookDirection == LookDirection.Up)
                {
                    attackRotation = Quaternion.Euler(0, 0, -90);
                    offset = new Vector3(0, 1, 0);
                }
                if (lookDirection == LookDirection.Down)
                {
                    return;
                }
            }
            else
            {
                if (lookDirection == LookDirection.Up)
                {
                    attackRotation = Quaternion.Euler(0, 0, -90);
                    offset = new Vector3(0, 1, 0);
                }
                if (lookDirection == LookDirection.Down)
                {
                    attackRotation = Quaternion.Euler(0, 0, -90);
                    offset = new Vector3(0, -1, 0);
                }
            }
            
            
        }
        if (directionCoord.Item1 == 0 && directionCoord.Item2 == 0)
        {
            offset = new Vector3(transform.right.x, 0, 0);
            if (lookDirection == LookDirection.Right)
            {
                offset = new Vector3(1, 0, 0);
            }
            if (lookDirection == LookDirection.Left)
            {
                offset = new Vector3(-1f, 0, 0);
            }

        }

        Instantiate(attack, transform.position + attackDirection + offset, attackRotation, this.transform);
        StartCoroutine(AttackCooldown());
    }
    private void Flipv2()
    {
        if (!isWallSliding)
        {
            directionCoord = (horizontal.ConvertTo<int>(), vertical.ConvertTo<int>());
            if (directionCoord == (1, directionCoord.Item2))
            {
                transform.rotation = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z);
                lookDirection = LookDirection.Right;
            }

            if (directionCoord == (-1, directionCoord.Item2))
            {
                transform.rotation = Quaternion.Euler(transform.rotation.x, 180, transform.rotation.z);
                lookDirection = LookDirection.Left;
            }

            if (directionCoord == (directionCoord.Item1, 1))
            {
                lookDirection = LookDirection.Up;
            }

            if (directionCoord == (directionCoord.Item1, -1))
            {
                lookDirection = LookDirection.Down;
            }

           


        }
        else
        {
            if (directionCoord == (1, directionCoord.Item2))
            {
                transform.rotation = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z);
                lookDirection = LookDirection.Left;
            }

            if (directionCoord == (-1, directionCoord.Item2))
            {
                transform.rotation = Quaternion.Euler(transform.rotation.x, 180, transform.rotation.z);
                lookDirection = LookDirection.Right;
            }

        }
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

    private IEnumerator ApplyMovement()
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
            else if (inHook)
            {
               // interacting = true;

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
                    yield return new WaitForSeconds(0.4f);
                    hookUsed = false;
                }

                if (rb.velocity.x < -maxTotalSpeed)
                {
                    rb.velocity = new Vector2(-maxTotalSpeed, rb.velocity.y);
                    yield return new WaitForSeconds(0.4f);
                    hookUsed = false;
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
        if (isOnGround && rb.velocity.y <= 0)
        {
            remainingJumps = maxJumps;
            hookUsed = false;
        }
        if (isWallSliding)
        {
            remainingJumps = 1;
            hookUsed = false;
        }
        
        if (remainingJumps <= 0 || !isOnGround && !isWallSliding)
        {
            if (dobleJump && remainingJumps > 0)
            {
                remainingJumps = 1;
                canJump = true;
            }
            else
            {
                canJump = false;
            }
        }
        else
        {
            canJump = true;
        }
    }
    
    public void Hook(Vector2 destiny)
    {
        hookUsed = true;
        inHook = true;
        ChangeInteracting(true);
        rb.velocity = new Vector2(rb.velocity.x,0);
        /*
        rb.AddForce((hit.collider.transform.position - transform.position ).normalized* (hookForce), ForceMode2D.Impulse);
        inHookSpeed = (hit.collider.transform.position.x - transform.position.x);
        Debug.Log(hit.collider.gameObject.transform.position);
        hookedPos = hit.collider.transform.position;
        */
        rb.AddForce((new Vector3(destiny.x, destiny.y,0) - transform.position).normalized * (hookForce), ForceMode2D.Impulse);
        inHookSpeed = (destiny.x - transform.position.x);
        
        hookedPos = new Vector3(destiny.x, destiny.y, 0);
        isOnGround = false;
        
        StartCoroutine(CooldownHook());
        //transform.Translate(hit.transform.position);
    }
    
    private IEnumerator CooldownHook()
    {
        yield return new WaitForSeconds(0.25f);
        inHook = false;
        interacting = false;
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
    /*
    private void Flip()
    {
        if (!isWallSliding)
        {
            while (Input.GetKey("w"))
            {
                lookDirection = LookDirection.Up;
            }

            if (Input.GetKey("a"))
            {
                if (Input.GetKey("s") && !isOnGround)
                {
                    lookDirection = LookDirection.Down;
                }
                else
                {
                    transform.rotation = Quaternion.Euler(transform.rotation.x, 180, transform.rotation.z);
                    lookDirection = LookDirection.Left;
                }
            }

            if (Input.GetKey("d"))
            {
                if (Input.GetKey("s") && !isOnGround)
                {
                    lookDirection = LookDirection.Down;
                }
                else
                {
                    transform.rotation = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z);
                    lookDirection = LookDirection.Right;
                }
            }
            if(rb.velocity.x == 0f && !isOnGround)
            {
                if (Input.GetKey("s"))
                {
                    lookDirection = LookDirection.Down;
                }
            }
        
        }
        else
        {

        }
    }
    */
    

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

        if (lookDirection == LookDirection.Left)
        {
            //attackDirection = new Vector3(-1f,0,0);
            //offset = new Vector3(-1f, 0, 0);
            attackRotation = transform.rotation;
        }
        else if (lookDirection == LookDirection.Right)
        {
            //attackDirection = new Vector3(1f,0,0);
            //offset = new Vector3(1f, 0, 0);
            attackRotation = transform.rotation;
        }
        else if (lookDirection == LookDirection.Down)
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

    public LookDirection GetDirectionLook()
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
    public void KillPlayer()
    {
        isDead = true;
    }


    public void CheckHabilities()
    {
        /*
        melee = GameManager.Instance.GetHabilities("basic");
        hook = GameManager.Instance.GetHabilities("hook");
        fireball = GameManager.Instance.GetHabilities("fireball");
        dobleJump = GameManager.Instance.GetHabilities("doblejump");
        wallJump = GameManager.Instance.GetHabilities("walljump");
        */

        List<ItemSO> specialItems = Inventory.Instance.GetItemsForSave(true);

        foreach (ItemSO item in specialItems)
        {
            if (item.habilityToGive == ItemSO.Hability.Melee)
            {
                melee = true;
            }
            if (item.habilityToGive == ItemSO.Hability.Hook)
            {
                hook = true;
            }
            if (item.habilityToGive == ItemSO.Hability.Fireball)
            {
                fireball = true;
            }
            if (item.habilityToGive == ItemSO.Hability.DobleJump)
            {
                dobleJump = true;
            }
            if (item.habilityToGive == ItemSO.Hability.Walljump)
            {
                wallJump = true;
            }

        }

        if (dobleJump)
        {
            maxJumps = 2;
        }
    }

    public bool HaveHability(string whatHab)
    {
        if (whatHab == "melee")
        {
            return melee;
        }
        if (whatHab == "hook")
        {
            return hook;
        }
        if (whatHab == "fireball")
        {
            return fireball;
        }
        if (whatHab == "walljump")
        {
            return wallJump;
        }
        if (whatHab == "doblejump")
        {
            return dobleJump;
        }
        return false;
    }
    

    public void Damaged(int damage, Vector3 knockbackDir)
    {
        
            GameManager.Instance.LoseLive(damage);
            animator.SetTrigger("isDamaged");

            //Debug.Log(knockbackDir.normalized);
            rb.velocity = (new Vector3(knockbackDir.normalized.x/2, 1f,0 )) * 10;

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

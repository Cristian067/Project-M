using System;
using System.Collections;
using UnityEngine;





public class ControlUnit : MonoBehaviour
{
    
    [SerializeField] private bool thinking;

    private Stats stats;

    private Rigidbody2D rb;

    private string lookingR;

    [SerializeField] private float jumpForce;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius;

    [SerializeField] private GameObject attackGO;

    [SerializeField] private float speed;
    private Vector2 posToMove;

    private Vector3 targetPoint;

    private Animator animator;

    private enum Phases
    {
        Phase1,
        Phase2

    }


    private Phases currentFase;
    

    private bool isOnGround;
    public LayerMask whatIsGround;

    private GameObject player;

    private bool flip;

    private int idx;
    private bool walking;
    


    // Start is called before the first frame update
    void Start()
    {

        stats = GetComponent<Stats>();
        
        animator = GetComponent<Animator>();

        rb = GetComponent<Rigidbody2D>();

        player = FindAnyObjectByType<PlayerMovement>().gameObject;

        StartCoroutine(FirstWait());
    }

    // Update is called once per frame
    void Update()
    {
        CheckSurrondings();

        targetPoint = player.transform.position - transform.position;

        if (!thinking)
        {
            switch (currentFase)
            {
                case Phases.Phase1:
                    int ran = UnityEngine.Random.Range(1, 8);
                    //Debug.Log(ran);
                    StartCoroutine($"Moveset{ran}");
                    break;
                case Phases.Phase2:

                    break;
            }

        }

        rb.velocity = new Vector2(Mathf.MoveTowards(rb.velocity.x, 0f, 20f * Time.deltaTime), rb.velocity.y);
        if (!flip)
        {
            if(targetPoint.x < 0)
            {
                transform.rotation = Quaternion.Euler(0,180,0);
            }
            if(targetPoint.x > 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }

            if (transform.rotation.eulerAngles.y == 180)
            {
                lookingR = "left";
            }
            else
            {
                lookingR = "right";
            }
        }

        if (walking)
        {
            Vector2 pos = new Vector2(transform.position.x, transform.position.y);
            Vector2 goTo = posToMove - pos;
            while (transform.position.x != pos.x)
            {
                rb.velocity = new Vector2(goTo.normalized.x * speed, rb.velocity.y);
            }
        }
    }

    private void FixedUpdate()
    {/*
        if (walking)
        {

            Walk();
        
        }*/
    }
   
    private void MoveTo(Vector2 posToMove)
    {
        Vector2 pos = new Vector2(transform.position.x, transform.position.y);
        Vector2 goTo = posToMove - pos;
        walking = true;


    }

    private Vector2 FindTarget()
    {
        return PlayerMovement.Instance.GetPosition();
        
    }
    private void Attack()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        GameObject attackInv = Instantiate(attackGO, transform.position, Quaternion.identity);
        attackInv.transform.parent = transform;
        rb.velocity = Vector3.zero;
        attackInv.transform.localScale = new Vector3(2, 0.7f, 0);
    }
    private void Jump(Vector2 direction)
    {
        rb.velocity = new Vector2(direction.x * jumpForce, direction.y * jumpForce);
    }

    private void Dash(float force, Vector2 dir, bool isInVertical)
    {
        if(!isInVertical)
        {
            rb.constraints = RigidbodyConstraints2D.FreezePositionY;
        }
        rb.velocity = dir.normalized * force;
    }
    private IEnumerator ForceStop(float time)
    {
        yield return new WaitForSeconds(time);
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
 
    private IEnumerator SelfCooldown(float cooldown)
    {
        
            yield return new WaitForSeconds(cooldown);
            thinking = false;
        
        
    }
    private void CheckSurrondings()
    {
        isOnGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

    }
    public void Damaged(int damage, Vector3 knockbackDir)
    {
        animator.SetTrigger("damaged");
        stats.LoseLive(damage);
        //rb.velocity = (new Vector3(0f, 0.5f, 0) + knockbackDir.normalized) * 10;

    }

    // Primera fase -------------------------------------------------------------------------------------

    public IEnumerator FirstWait()
    {
        thinking = true;
        yield return new WaitForSeconds(2);
        thinking = false;
    }
    public IEnumerator Moveset1()
    {
        thinking = true;
        //Debug.Log("a");
        Dash(40, PlayerMovement.Instance.GetPosition() - transform.position, false);
        //Wait(5);
        yield return new WaitForSeconds(2);
        Dash(40, PlayerMovement.Instance.GetPosition() - transform.position, false);
        yield return new WaitForSeconds(0.4f);
        ForceStop(0);
        yield return new WaitForSeconds(2);
        
        thinking=false;
        
    }

    public IEnumerator Moveset2()
    {
        thinking = true;
        ForceStop(0);
        Jump(new Vector2(transform.right.x, 1));
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        yield return new WaitForSeconds(0.3f);
        rb.constraints = RigidbodyConstraints2D.FreezePositionX;
        Dash(30, PlayerMovement.Instance.GetPosition() - transform.position, true);
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        yield return new WaitForSeconds(4);
        thinking = false;

    }

    public IEnumerator Moveset3()
    {
        thinking = true;

        for(int i =0; i < 5; i++)
        {
            yield return new WaitForSeconds(1);
            transform.position = new Vector3(58.4f, UnityEngine.Random.Range(6, 12), 0);
            rb.constraints = RigidbodyConstraints2D.FreezePositionY;
            yield return new WaitForSeconds(0.5f);
            Dash(40, PlayerMovement.Instance.GetPosition() - transform.position, false);
        }
        yield return new WaitForSeconds(0.5f);
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        thinking = false;
    }

    public IEnumerator Moveset4()
    {
        thinking = true;

        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(1);
            transform.position = new Vector3(PlayerMovement.Instance.GetPosition().x,13.5f, 0);
            rb.constraints = RigidbodyConstraints2D.FreezePositionY;
            yield return new WaitForSeconds(0.5f);
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            Dash(50, new Vector2(0,-1), true);
        }
        yield return new WaitForSeconds(0.5f);
        transform.position = new Vector3(57, 8.5f, 0);

        thinking = false;
    }

    public IEnumerator Moveset5()
    {
        thinking = true;

        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(1);
            rb.constraints = RigidbodyConstraints2D.FreezePositionY;
            if (PlayerMovement.Instance.GetDirectionLook() == PlayerMovement.LookDirection.Left)
            {
                transform.position = new Vector3(PlayerMovement.Instance.GetPosition().x + 3, PlayerMovement.Instance.GetPosition().y, 0);
            }

            if (PlayerMovement.Instance.GetDirectionLook() == PlayerMovement.LookDirection.Right)
            {
                transform.position = new Vector3(PlayerMovement.Instance.GetPosition().x + -3, PlayerMovement.Instance.GetPosition().y, 0);
            }

            
            yield return new WaitForSeconds(0.8f);
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            
            Dash(40, PlayerMovement.Instance.GetPosition() - transform.position, false);
            ForceStop(0);
        }
        yield return new WaitForSeconds(0.5f);
        transform.position = new Vector3(57, 8.5f, 0);

        thinking = false;
    }

    public IEnumerator Moveset6()
    {
        
        thinking = true;

        for (int i = 0; i < 5; i++)
        {
            flip = true;
            yield return new WaitForSeconds(1);
            flip = false;
            if (PlayerMovement.Instance.GetDirectionLook() == PlayerMovement.LookDirection.Left)
            {
                transform.position = new Vector3(PlayerMovement.Instance.GetPosition().x + 3, PlayerMovement.Instance.GetPosition().y, 0);
            }

            if (PlayerMovement.Instance.GetDirectionLook() == PlayerMovement.LookDirection.Right)
            {
                transform.position = new Vector3(PlayerMovement.Instance.GetPosition().x + -3, PlayerMovement.Instance.GetPosition().y, 0);
            }


            yield return new WaitForSeconds(0.5f);
            

            Attack();
            ForceStop(0);

        }
        yield return new WaitForSeconds(0.5f);
        transform.position = new Vector3(57, 5.5f, 0);

        thinking = false;
        
    }
    public IEnumerator Moveset7()
    {
        thinking = true;
        transform.position = new Vector3(57, 5.5f, 0);

        yield return new WaitForSeconds(1f);
        

        thinking = false;
    }

    // Segunda fase -------------------------------------------------------------------------------------

    private void EnterPhase()
    {

    }
    private void FirstAttack()
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if (!GameManager.Instance.GetInvencibility())
            {
                Vector3 dir = collision.transform.position - transform.position;

                PlayerMovement.Instance.Damaged(1, dir);
            }
            
        }
    }


}

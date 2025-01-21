using System;
using System.Collections;
using UnityEngine;




public class ControlUnit : MonoBehaviour
{
    [SerializeField] private bool thinking;

    private Stats stats;
    private SpinAttack spinAttack;

    private Rigidbody2D rb;

    private string lookingR;

    [SerializeField] private float jumpForce;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius;

    [SerializeField] private GameObject attackGO;

    [SerializeField] private float speed;

    private Vector3 targetPoint;

    private enum Attacks
    {
        Wait,
        Walk,
        Attack,
        Dash,
        jump,
        Backjump,
        StepBack,
        SpinAttack,

    }

    private enum Phases
    {
        Phase1,
        Phase2

    }

    private Attacks attack;
    private Phases currentFase;
    

    private bool isOnGround;
    public LayerMask whatIsGround;

    private GameObject player;

    private bool ultimate;

    private int idx;
    private bool walking;
    private bool knockback;


    // Start is called before the first frame update
    void Start()
    {

        stats = GetComponent<Stats>();
        spinAttack = GetComponent<SpinAttack>();

        rb = GetComponent<Rigidbody2D>();

        attack = Attacks.Wait;

        player = FindAnyObjectByType<PlayerMovementV2>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        CheckSurrondings();

        targetPoint = player.transform.position - transform.position;

        if (!thinking && !knockback)
        {
            switch (currentFase)
            {
                case Phases.Phase1:
                    switch (attack)
                    {
                        case Attacks.Wait:
                            StartCoroutine(Wait(3));
                            Next();
                            break;

                        case Attacks.Walk:
                            if (isOnGround)
                            {
                                //walking = true;
                                //WaitAfterWalk();
                                Debug.Log(Attacks.Walk);
                                Next();
                            }

                            break;

                        case Attacks.Attack:
                            Debug.Log(Attacks.Attack);
                            //StartCoroutine(Wait(1));
                            Attack();
                            //Next();
                            break;

                        case Attacks.Dash:
                            Debug.Log(Attacks.Dash);
                            //StartCoroutine(Wait(1));
                            Dash(30);
                            //Next();
                            break;

                        case Attacks.jump:
                            if (isOnGround)
                            {
                                //StartCoroutine(Wait(1.5f));
                                Debug.Log(Attacks.jump);
                                Jump(new Vector2(0, 1), true);
                                //Next();
                            }
                            break;

                        case Attacks.Backjump:
                            if (isOnGround)
                            {
                                //StartCoroutine(Wait(1.5f));
                                Debug.Log(Attacks.Backjump);
                                Jump(new Vector2(-targetPoint.normalized.x, 1), false);
                                //Next();
                            }

                            break;
                        case Attacks.StepBack:
                            Debug.Log(Attacks.StepBack);
                            //StartCoroutine(Wait(1.5f));
                            //Next();
                            break;
                        case Attacks.SpinAttack:
                            Debug.Log(Attacks.SpinAttack);

                            break;
                    }
                    break; 
                case Phases.Phase2:
                    break;
            }
            
            StartCoroutine(Wait(1f));
            Next();
        }

        

        rb.velocity = new Vector2(Mathf.MoveTowards(rb.velocity.x, 0f, 20f * Time.deltaTime), rb.velocity.y);

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

    private void FixedUpdate()
    {
        if (walking)
        {

            Walk();

        }
    }


    private IEnumerator WaitAfterWalk()
    {
        float idxn = UnityEngine.Random.Range(0f, 1f);

        yield return new WaitForSeconds(idxn);
        StopWalk();

    }
    private IEnumerator Wait(float seconds)
    {
        thinking = true;
        Debug.Log(thinking);
        yield return new WaitForSeconds(seconds);
        thinking = false;
    }

    private void Walk()
    {
        thinking = true;
        Vector2 moveToPos = FindTarget();
        MoveTo(moveToPos); 
    }

    private void MoveTo(Vector2 posToMove)
    {
        Vector2 pos = new Vector2(transform.position.x, transform.position.y);
        Vector2 goTo = posToMove - pos;
        rb.velocity = new Vector2(goTo.normalized.x * speed, rb.velocity.y); //new Vector2((goTo.normalized) *speed, transform.position.y);

    }
    private void StopWalk()
    {
        walking = false;
        Debug.Log(walking);
        rb.velocity = Vector2.zero;
        Wait(1);
    }

    private Vector2 FindTarget()
    {
        return PlayerMovementV2.Instance.GetPosition();
        
    }
    private void Attack()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        GameObject attackInv = Instantiate(attackGO, transform.position, Quaternion.identity);
        attackInv.transform.parent = transform;
        //Debug.Log(transform.rotation.eulerAngles.y);
        if (transform.rotation.eulerAngles.y == 180)
        {

            rb.velocity = Vector3.zero;
            

            //attackInv.transform.localPosition = new Vector3(2.5f, 0, 0); //= Instantiate(attack, transform.position - new Vector3(-5,0,0), Quaternion.identity);

        }
        else if (transform.rotation.y == 0)
        {
            rb.velocity = Vector3.zero;
            
            //attackInv.transform.localPosition = new Vector3(2.5f, 0, 0);
            //attackInv = Instantiate(attack, transform.position - new Vector3(5, 0, 0), Quaternion.identity);
        }
        
        attackInv.transform.localScale = new Vector3(2, 0.7f, 0);
    }
    private void Jump(Vector2 direction, bool wait)
    {
        rb.velocity = new Vector2(direction.x * jumpForce, direction.y * jumpForce);
        if (wait)
        {
            StartCoroutine(StayInAir());
        }
        
    }

    private IEnumerator StayInAir()
    {
        StartCoroutine(Wait(1));
        yield return new WaitForSeconds(0.2f);
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        yield return new WaitForSeconds(0.3f);
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

    }

    private void Dash(float force)
    {
        rb.constraints = RigidbodyConstraints2D.FreezePositionY;
        Vector2 dir = PlayerMovementV2.Instance.GetPosition() - transform.position;

        rb.velocity = new Vector2(dir.normalized.x,0) * force;
        StartCoroutine(ForceStop(0.3f));
        
    
    }

    private IEnumerator ForceStop(float time)
    {
        yield return new WaitForSeconds(time);
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
    private void Next()
    {
        idx = UnityEngine.Random.Range(0, Enum.GetValues(typeof(Attacks)).Length);

        attack = (Attacks)idx;
    }

    private IEnumerator SelfCooldown(float cooldown)
    {
        if (!ultimate)
        {
            yield return new WaitForSeconds(cooldown);
            thinking = false;
        }
        
    }

    /*
    private IEnumerator SpinAttack()
    {

        Jump(new Vector2(0, 2), 0.5);
        yield return new WaitForSeconds(0.2f);
        spinAttack.Use(rb);
        StartCoroutine(SelfCooldown(1.4f));

    }
    */
    private void CheckSurrondings()
    {
        isOnGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

    }

    public void Damaged(int damage, Vector3 knockbackDir)
    {

        stats.LoseLive(damage);
        //animator.SetTrigger("isDamaged");

        Debug.Log(knockbackDir.normalized);
        rb.velocity = (new Vector3(0f, 0.5f, 0) + knockbackDir.normalized) * 10;

        StartCoroutine(KnockbackTime());
        //animator.ResetTrigger("isDamaged");

    }

    private IEnumerator KnockbackTime()
    {
        knockback = true;
        yield return new WaitForSeconds(0.5f);
        knockback = false;
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

                PlayerMovementV2.Instance.Damaged(1, dir);
            }
            
        }
    }


}

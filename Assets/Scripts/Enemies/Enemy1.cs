using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyV2 : MonoBehaviour
{

    private enum States
    {
        Roaming,
        Chase,
        Attacking,
    }

    private States currentState;

    private Vector2 startingPos;
    private Vector2 roamPos;


    

    [SerializeField] private GameObject attack;
    [SerializeField] private float attackLenght;

    [SerializeField] private string lookDirection;

    private bool attackInCooldown;
    [SerializeField] private float attackCooldown;

    [SerializeField] private float speed;
    [SerializeField] private float maxSpeed;

    [SerializeField] private Vector2 _distance;

    [SerializeField] private float jumpForce;
    private bool isOnGround;

    private Rigidbody2D rb;

    private bool thinking;

    private bool knockback;

    public LayerMask whatIsGround;

    private RaycastHit2D hitGround1;
    private RaycastHit2D hitGround2;

    private Animator animator;
    private EnemyBasics basicStats;

    [SerializeField] private float chasingRadius;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        basicStats = GetComponent<EnemyBasics>();
    }
    // Start is called before the first frame update
    void Start()
    {
        attackInCooldown = false;
        rb = GetComponent<Rigidbody2D>();
        currentState = States.Roaming;

        startingPos.x = transform.position.x;
        roamPos.x = GetRoamingPos();

        startingPos.y = transform.position.y;
        roamPos.y = transform.position.y;
        
    }

    // Update is called once per frame
    void Update()
    {
        Flip();
        

         hitGround1 = Physics2D.Raycast(transform.position + new Vector3(-0.5f, -0.51f, 0), Vector3.down, 0.1f, whatIsGround);
        hitGround2 = Physics2D.Raycast(transform.position + new Vector3(0.5f, -0.51f, 0), Vector3.down, 0.1f, whatIsGround);
        //Debug.DrawLine(transform.position + new Vector3(0,-0.51f,0), transform.position + new Vector3(0,-0.61f,0));
        if (!basicStats.IsKnockbacked())
        {
            if (hitGround1 || hitGround2)
            {
                isOnGround = true;
            }
            else if (!hitGround1 && !hitGround2)
            {
                isOnGround = false;
            }

            Debug.DrawLine(transform.position + new Vector3(0.5f, 0, 0), transform.position + transform.right, Color.blue);

            rb.velocity = new Vector2(Mathf.MoveTowards(rb.velocity.x, 0f, 15f * Time.deltaTime), rb.velocity.y);
            /*
            if (_distance.x < attackLenght && !thinking && _distance.x != 0)
            {
                StartCoroutine("ThinkAttack");
            }*/
        }
    }

    private void FixedUpdate()
    {
        if (!basicStats.IsKnockbacked())
        {
            RaycastHit2D hitWall = Physics2D.Raycast(transform.position, transform.right, 0.8f, whatIsGround);
            if (hitWall)
            {
                //Debug.Log(hitWall.collider.gameObject.name);
            }

            if (hitWall && _distance.x != 0 && isOnGround)
            {
                //Debug.Log("jenye");
                Jump();
            }
            else if (!hitWall)
            {

            }

            switch (currentState)
            {
                case States.Roaming:
                    MoveTo(roamPos);

                    RaycastHit2D hitWall2 = Physics2D.Raycast(transform.position + new Vector3(0f, 0, 0), transform.right, 1f, whatIsGround);
                    float reachedPositionDistance = 1f;
                    if (Vector2.Distance(transform.position, roamPos) < reachedPositionDistance || hitWall2)
                    {
                        roamPos.x = GetRoamingPos();
                    }
                    FindTarget();
                    break;

                case States.Chase:
                    if (!knockback)
                    {
                        MoveTo(PlayerMovement.Instance.GetPosition());
                        if (!attackInCooldown && !animator.GetBool("preattack"))
                        {
                            
                            if (Vector2.Distance(transform.position, PlayerMovement.Instance.GetPosition()) < attackLenght)
                            {
                                StartCoroutine(PreAttack());
                            }
                            //animator.SetBool(0, true);

                        }

                    }

                    float stopChasingDistance = chasingRadius;
                    RaycastHit2D hit2D = Physics2D.Raycast(transform.position, PlayerMovement.Instance.GetPosition() - transform.position, Vector3.Distance(transform.position, PlayerMovement.Instance.GetPosition()), whatIsGround);
                    //Debug.Log(hit2D.collider.name);
                    if (Vector2.Distance(transform.position, PlayerMovement.Instance.GetPosition()) > stopChasingDistance || hit2D)
                    {
                        currentState = States.Roaming;
                    }
                    break;


            }
            //Debug.Log(Vector2.Distance(transform.position, PlayerMovement.Instance.GetPosition()));

            if (_distance != new Vector2(0, 0))
            {
                rb.velocity = new Vector2(speed * (_distance.x / 10), rb.velocity.y);
            }

            //Debug.Log(_distance);
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
    }

    
    
    
    private void MoveTo(Vector2 posToMove)
    {
        Vector2 pos = new Vector2(transform.position.x, transform.position.y);
        Vector2 goTo = posToMove - pos; 
        

        rb.velocity = new Vector2(goTo.normalized.x * speed,rb.velocity.y); //new Vector2((goTo.normalized) *speed, transform.position.y);
    }

    private IEnumerator PreAttack()
    {
        animator.SetBool("preattack", true);

        yield return new WaitForSeconds(0.5f);
        Attack();
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
        animator.SetBool("preattack", false);
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
    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(transform.up * jumpForce);
        isOnGround = false;
    }
    private IEnumerator ThinkAttack ()
    {
        thinking = true;
        yield return new WaitForSeconds(Random.Range(0.3f, 1));
        if (!attackInCooldown)
        {
            Attack();
        }
        
        thinking = false;
    }

    public void GetDistance(Vector2 distance)
    {
        _distance = distance;
    }


    private float GetRoamingPos()
    {
        float randomDirection = Random.Range(-1f, 1f);

        return startingPos.x + randomDirection * Random.Range(5f,15f);
    }

    private void FindTarget()
    {
        float targetRange = 5f;
        if(Vector3.Distance(transform.position,PlayerMovement.Instance.GetPosition()) < targetRange)
        {
            currentState = States.Chase;
        }
    }


}

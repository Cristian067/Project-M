using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;
//using Random from UnityiEngine;

public class ControlUnit : MonoBehaviour
{
    private Dash dash;
    private Jump jump;
    private Stats stats;
    private SpinAttack spinAttack;
    private BeamAttack beamAttack;

    private Rigidbody2D rb;

    private string lookingR;

    [SerializeField] private float jumpForce;

    [SerializeField] private string[] actions;

    [SerializeField] private bool thinking;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius;

    [SerializeField] private GameObject attackGO;

    private enum Attacks
    {
        Wait,
        Attack,
        Dash,
        jump,
        Backjump,
        StepBack,
        SpinAttack,

    }

    private Attacks attack;
    

    private bool isOnGround;
    public LayerMask whatIsGround;

    private GameObject player;

    private bool ultimate;

    private int idx;

    // Start is called before the first frame update
    void Start()
    {

        dash = GetComponent<Dash>();
        jump = GetComponent<Jump>();
        stats = GetComponent<Stats>();
        spinAttack = GetComponent<SpinAttack>();
        beamAttack = GetComponent<BeamAttack>();

        rb = GetComponent<Rigidbody2D>();

        attack = Attacks.Wait;

        //StartCoroutine(SelfCooldown());

        //dash.Use(rb, 1200);

        player = FindAnyObjectByType<PlayerMovement>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        CheckSurrondings();

        Vector3 targetPoint = player.transform.position - transform.position;

        if (!thinking)
        {

        

            switch (attack)
            {
                case Attacks.Wait:
                    StartCoroutine(Wait(3));
                    Next();
                    break;
                case Attacks.Attack:
                    Debug.Log(Attacks.Attack);

                    Attack();
                    Next();
                    break;

                case Attacks.Dash:
                    Debug.Log(Attacks.Dash);
                    Next();
                    break;

                case Attacks.jump:
                    if (isOnGround)
                    {
                        Debug.Log(Attacks.jump);
                        Jump(new Vector2(0, 1));
                        Next();
                    }
                    break;

                case Attacks.Backjump:
                    Debug.Log(Attacks.Backjump);

                    Jump(new Vector2(-1, 1));
                    Next();
                    break;
                case Attacks.StepBack:
                    Debug.Log(Attacks.StepBack);
                    Next();
                    break;
                case Attacks.SpinAttack:
                    Debug.Log(Attacks.SpinAttack);
                    Next();
                    break;
            }
        }
        /*
        //Detectar suelo
        RaycastHit2D hitGround1 = Physics2D.Raycast(transform.position + new Vector3(-0.5f, -0.51f, 0), Vector3.down, 0.1f, whatIsGround);
        RaycastHit2D hitGround2 = Physics2D.Raycast(transform.position + new Vector3(0.5f, -0.51f, 0), Vector3.down, 0.1f, whatIsGround);
        //Debug.DrawLine(transform.position + new Vector3(0,-0.51f,0), transform.position + new Vector3(0,-0.61f,0));

        if (hitGround1 || hitGround2)
        {
            isOnGround = true;
            
        }
        else if (!hitGround1 || !hitGround2)
        {
            isOnGround = false;
        }
        
        */
        rb.velocity = new Vector2(Mathf.MoveTowards(rb.velocity.x, 0f, 20f * Time.deltaTime), rb.velocity.y);
        int random = UnityEngine.Random.Range(0, actions.Length);
        /*
        if (!thinking)
        {
            if (actions[random] == "jump" && isOnGround)
            {
                thinking = true;
                jump.Use(rb, new Vector2(0,3), 400);
                StartCoroutine(SelfCooldown(1));
            }
            if (actions[random] == "dash")
            {
                thinking = true;
                dash.Use(rb, 1200,false);
                StartCoroutine(SelfCooldown(1));
            }
            if (actions[random] == "dashdown" && !isOnGround)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0,-0.55f,0), Vector2.down, 3.2f);
                //Debug.Log(hit);
                if (!hit)
                {
                    dash.Use(rb, 1200, true);
                    StartCoroutine(SelfCooldown(0));
                }
                
            }
            if (actions[random] == "backjump" && isOnGround)
            {
                thinking = true;
                jump.Use(rb, new Vector2(2, 3), 400);
                //jump.Use(rb, new Vector2(2, 3), 400);
                StartCoroutine(SelfCooldown(0.3f));
            }
            if (actions[random] == "wait")
            {
                thinking = true;
                StartCoroutine(SelfCooldown(1));
            }
            if (actions[random] == "stepback")
            {
                if (lookingR == "left")
                {
                    thinking = true;
                    jump.Use(rb, new Vector2(3, 0), 300);
                    StartCoroutine(SelfCooldown(0.5f));
                }
                else if (lookingR == "right")
                {
                    thinking = true;
                    jump.Use(rb, new Vector2(-3, 0), 300);
                    StartCoroutine(SelfCooldown(0.5f));
                }
            }
            if (actions[random] == "spinattack" && isOnGround)
            {
                thinking = true;
                StartCoroutine(SpinAttack());
                
            }
            
        }


        
        */

        

        

        if(targetPoint.x < 0)
        {
            transform.rotation = Quaternion.Euler(0,180,0);
        }
        if(targetPoint.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        //transform.right = targetPoint;
        //transform.rotation = Quaternion.Euler(0,transform.right.y,0);

        //Debug.Log(transform.right);

        //transform.rotation = Quaternion.Euler(transform.rotation.x,targetPoint.y ,transform.rotation.z);

        //Quaternion targetRotation = Quaternion.LookRotation(-targetPoint);
        //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 2.0f);
        



        if (transform.rotation.eulerAngles.y == 180)
        {
            lookingR = "left";
        }
        else
        {
            lookingR = "right";
        }
        

    }


    private IEnumerator Wait(float seconds)
    {
        thinking = true;
        yield return new WaitForSeconds(seconds);
        thinking = false;
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
            

            attackInv.transform.localPosition = new Vector3(2.5f, 0, 0); //= Instantiate(attack, transform.position - new Vector3(-5,0,0), Quaternion.identity);

        }
        else if (transform.rotation.y == 0)
        {
            rb.velocity = Vector3.zero;
            
            attackInv.transform.localPosition = new Vector3(2.5f, 0, 0);
            //attackInv = Instantiate(attack, transform.position - new Vector3(5, 0, 0), Quaternion.identity);
        }
        
        attackInv.transform.localScale = new Vector3(2, 0.7f, 0);
    }
    private void Jump(Vector2 direction)
    {
        rb.velocity = new Vector2(direction.x * jumpForce, direction.y * jumpForce);
        StartCoroutine(StayInAir());
    }

    private IEnumerator StayInAir()
    {
        StartCoroutine(Wait(1));
        yield return new WaitForSeconds(0.2f);
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        yield return new WaitForSeconds(0.7f);
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

    private IEnumerator SpinAttack()
    {

        jump.Use(rb, new Vector2(0, 2), 300);
        yield return new WaitForSeconds(0.2f);
        spinAttack.Use(rb);
        StartCoroutine(SelfCooldown(1.4f));

    }

    public void BeamAttack()
    {
        ultimate = true;
        thinking = true;
        beamAttack.Use(rb, new Vector2(68,9.3f));
    }

    private void CheckSurrondings()
    {
        isOnGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);

    }
}

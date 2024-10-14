using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ControlUnit : MonoBehaviour
{
    private Dash dash;
    private Jump jump;
    private Stats stats;

    private Rigidbody2D rb;

    private string lookingR;

    [SerializeField] private string[] actions;

    [SerializeField] private bool thinking;

    private bool isOnGround;
    public LayerMask layerToJump;

    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {

        dash = GetComponent<Dash>();
        jump = GetComponent<Jump>();
        stats = GetComponent<Stats>();
        rb = GetComponent<Rigidbody2D>();

        //StartCoroutine(SelfCooldown());

        //dash.Use(rb, 1200);

        player = FindAnyObjectByType<PlayerMovement>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {

        //Detectar suelo
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
        rb.velocity = new Vector2(Mathf.MoveTowards(rb.velocity.x, 0f, 20f * Time.deltaTime), rb.velocity.y);

        int random = Random.Range(0, actions.Length);

        if (!thinking)
        {
            if (actions[random] == "jump" && isOnGround)
            {
                jump.Use(rb, new Vector2(0,3), 400);
                StartCoroutine(SelfCooldown());
            }
            if (actions[random] == "dash")
            {
                dash.Use(rb, 1200,false);
                StartCoroutine(SelfCooldown());
            }
            if (actions[random] == "dashdown" && !isOnGround)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0,-0.55f,0), Vector2.down, 3.2f);
                //Debug.Log(hit);
                if (!hit)
                {
                    dash.Use(rb, 1200, true);
                    StartCoroutine(SelfCooldown());
                }
                
            }
            if (actions[random] == "backjump" && isOnGround)
            {
                jump.Use(rb, new Vector2(2, 3), 400);
                //jump.Use(rb, new Vector2(2, 3), 400);
                StartCoroutine(SelfCooldown());
            }
            if (actions[random] == "wait")
            {
                
                StartCoroutine(SelfCooldown());
            }
            if (actions[random] == "stepback")
            {
                if (lookingR == "left")
                {
                    jump.Use(rb, new Vector2(3, 0), 300);
                    StartCoroutine(SelfCooldown());
                }
                else if (lookingR == "right")
                {
                    jump.Use(rb, new Vector2(-3, 0), 300);
                    StartCoroutine(SelfCooldown());
                }
            }
        }


        

        Vector3 targetPoint = player.transform.position - transform.position;

        

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

    private IEnumerator SelfCooldown()
    {
        thinking = true;
        yield return new WaitForSeconds(1);
        thinking = false;
    }
}
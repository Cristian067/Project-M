using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlUnit : MonoBehaviour
{
    private Dash dash;
    private Jump jump;

    private Rigidbody2D rb;

    private string lookingR;

    [SerializeField] private string[] actions;

    [SerializeField] private bool thinking;

    private bool isOnGround;
    public LayerMask layerToJump;

    // Start is called before the first frame update
    void Start()
    {

        dash = GetComponent<Dash>();
        jump = GetComponent<Jump>();
        rb = GetComponent<Rigidbody2D>();

        //StartCoroutine(SelfCooldown());

        //dash.Use(rb, 1200);

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
                dash.Use(rb, 1200,true);
                StartCoroutine(SelfCooldown());
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

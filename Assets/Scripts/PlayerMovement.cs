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

    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;

    [SerializeField] private bool inHook;


    [SerializeField] private float hookDistance;

    [SerializeField] private float hookForce;


    public LayerMask layerToHit;

    private float horizontal;
    private int horizontalInt;


    private Rigidbody2D rb;

    [SerializeField]private Camera cam;


    private Vector3 hookedPos;

    public float minimum = -1.0F;
    public float maximum = 1.0F;

    // starting value for the Lerp
    static float t = 0.0f;





    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        inHook = false;
        
    }




    // Update is called once per frame
    void Update()
    {

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
        horizontal = Input.GetAxis("Horizontal");

        horizontalInt = (int)horizontal;


        

        /*
        if (horizontal != 0)
        {
            transform.Translate(new Vector3(1,0,0) * horizontal * speed * Time.deltaTime);

            
        }

        */

        Vector3 direction = (mousePos - transform.position).normalized;

        //DrawLine(transform.position, mousePos, Color.blue);


        if (Input.GetKeyDown("w"))
        {

            //transform.Translate(new Vector3(0,1,0) * jumpForce);
            rb.AddForce(transform.up * jumpForce);

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


        t += 0.5f * Time.deltaTime;
        if (!inHook)
        {
            rb.velocity = new Vector2(Mathf.MoveTowards(rb.velocity.x,0f,15f * Time.deltaTime), rb.velocity.y); 

        }
           //new Vector2(Mathf.Lerp(minimum, maximum, 0), rb.velocity.y);

        


    }
    private void FixedUpdate()
    {
        
        if (Input.GetKey(KeyCode.A) && !inHook || Input.GetKey(KeyCode.D) && !inHook)
        {


            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }
        /*
        if (rb.velocity.x >= 0)
        {
            rb.velocity = new Vector2(rb.velocity.x -0.05f, rb.velocity.y);
        }
        else if (rb.velocity.x <= 0)
        {
            rb.velocity = new Vector2(rb.velocity.x + 0.05f, rb.velocity.y);
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
        StartCoroutine("CooldownHook");

        //transform.Translate(hit.transform.position);

    }


    private IEnumerator CooldownHook()
    {
        yield return new WaitForSeconds(0.2f);
        inHook = false;
    }


}

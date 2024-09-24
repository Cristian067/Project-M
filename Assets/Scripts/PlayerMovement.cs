using System;
using System.Collections;
using System.Collections.Generic;
//using System.Drawing;
using System.Reflection;
using Unity.VisualScripting;
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

        
        
    }
    private void FixedUpdate()
    {
        if (!inHook)
        {


            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }
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

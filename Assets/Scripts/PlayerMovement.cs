using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;


    [SerializeField] private float hookDistance;

    

    private float horizontal;


    private Rigidbody2D rb;

    [SerializeField]private Camera cam;





    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
    }




    // Update is called once per frame
    void Update()
    {

        Vector3 mousePos = Input.mousePosition;  // Set(transform.position.x,transform.position.y,transform.position.z);

        Debug.Log(transform.localPosition);
        
            
        horizontal = Input.GetAxis("Horizontal");


        
        /*
        
        
        if (horizontal != 0)
        {
            transform.Translate(new Vector3(1,0,0) * horizontal * speed * Time.deltaTime);

            
        }

        */
        Vector3 relativePosition;

        relativePosition = cam.transform.TransformDirection(transform.position - cam.transform.position);

        Vector3 originPos = cam.ScreenPointToRay(relativePosition).origin;
        Vector3 destinyPos = cam.ScreenPointToRay(mousePos).origin;

        Debug.DrawLine(originPos, destinyPos);


        if (Input.GetKeyDown("w"))
        {

            //transform.Translate(new Vector3(0,1,0) * jumpForce);
            rb.AddForce(transform.up * jumpForce);

        }


        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            
            RaycastHit2D hit = Physics2D.Raycast(destinyPos, originPos, hookDistance);
            Debug.DrawLine(originPos, hit.point);
            if (hit.collider != null)
            {
                //Debug.Log("sha");
                //Hook(hit);
            }

        }

        
        
    }
    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);

    }


    private void Hook(RaycastHit2D hit)
    {
        transform.Translate(hit.transform.position);

    }


}

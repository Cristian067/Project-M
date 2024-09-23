using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;


    private Rigidbody2D rb;
    
    
    


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        
        if (horizontal != 0)
        {
            transform.Translate(new Vector3(1,0,0) * horizontal * speed * Time.deltaTime);
        }


        if (Input.GetKeyDown("w"))
        {

            //transform.Translate(new Vector3(0,1,0) * jumpForce);
            rb.AddForce(transform.up * jumpForce);

        }

        
        
    }
}

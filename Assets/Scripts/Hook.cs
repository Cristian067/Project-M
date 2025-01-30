using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{

    private bool canHook;

    [SerializeField] private LayerMask whatGround;
    [SerializeField] private CircleCollider2D cc;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        cc = GetComponent<CircleCollider2D>();
        

        if (canHook && Input.GetKeyDown(KeyCode.Mouse1))
        {
            RaycastHit2D hit2D = Physics2D.Raycast(transform.position, PlayerMovement.Instance.GetPosition() - transform.position, cc.radius / 4, whatGround);
            //Debug.Log(hit2D.collider.gameObject.name);
            Debug.DrawRay(transform.position, PlayerMovement.Instance.GetPosition() - transform.position);
            if (hit2D)
            {
                return;
            }
            else
            {
                PlayerMovement.Instance.Hook(transform.position);
            }
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //PlayerMovementV2 player = collision.gameObject.GetComponent<PlayerMovementV2>();

            if (PlayerMovement.Instance.HaveHability("hook"))
            {
                

                
                
                
                canHook = true;
                
                //if (PlayerMovement.Instance.GetPosition() )
                //Debug.Log("hook");
                
            }
            
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            canHook = false;
        }
    }

    private void OnDrawGizmos()
    {
        if (canHook)
        {
            Gizmos.DrawLine(transform.position, PlayerMovement.Instance.GetPosition()  );
            
        }
        //Gizmos.DrawSphere(transform.position,cc.radius/4);
        
    }

}

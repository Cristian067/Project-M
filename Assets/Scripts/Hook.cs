using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{

    private bool canHook;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canHook && Input.GetKeyDown(KeyCode.Mouse1))
        {
            PlayerMovementV2.Instance.Hook(transform.position);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //PlayerMovementV2 player = collision.gameObject.GetComponent<PlayerMovementV2>();

            if (PlayerMovementV2.Instance.HaveHability("hook"))
            {
                //Debug.Log("hook");
                canHook = true;
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
        //Gizmos.DrawLine(transform.position, PlayerMovementV2.Instance.GetPosition());
    }

}

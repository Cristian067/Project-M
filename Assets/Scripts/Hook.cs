using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Hook : MonoBehaviour
{

    private bool canHook;

    
    [SerializeField] private CircleCollider2D cc;

    [SerializeField]private static int hooksInRange;

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
            RaycastHit2D hit2D = Physics2D.Raycast(transform.position, PlayerMovement.Instance.GetPosition() - transform.position);
            
            //Debug.Log(hit2D.collider.gameObject.name);
            Debug.DrawRay(transform.position, PlayerMovement.Instance.GetPosition() - transform.position);
            if (hit2D)
            {
                if (hit2D.collider.gameObject.tag == "Player")
                {
                    if (hooksInRange > 1)
                    {
                        if (PlayerMovement.Instance.GetDirectionLook() == PlayerMovement.LookDirection.Up && PlayerMovement.Instance.GetPosition().y - transform.position.y < 0)
                        {
                            PlayerMovement.Instance.Hook(transform.position);
                        }
                        if (PlayerMovement.Instance.GetDirectionLook() == PlayerMovement.LookDirection.Down && PlayerMovement.Instance.GetPosition().y - transform.position.y > 0)
                        {
                            PlayerMovement.Instance.Hook(transform.position);
                        }
                        if (PlayerMovement.Instance.GetDirectionLook() == PlayerMovement.LookDirection.Left && PlayerMovement.Instance.GetPosition().x - transform.position.x > 0)
                        {
                            PlayerMovement.Instance.Hook(transform.position);
                        }
                        if (PlayerMovement.Instance.GetDirectionLook() == PlayerMovement.LookDirection.Right && PlayerMovement.Instance.GetPosition().x - transform.position.x < 0)
                        {
                            PlayerMovement.Instance.Hook(transform.position);
                        }
                    }
                    else
                    {
                        PlayerMovement.Instance.Hook(transform.position);
                    }
                }
                else if (hit2D.collider.gameObject.tag == "Ground")
                {
                    return;
                }
                
            }
            else
            {
                //Debug.Log("Hay suelo en medio");

                return;

                //Debug.Log(PlayerMovement.Instance.GetPosition().y - transform.position.y);


            }
            
        }

        SetVisuals();
    }


    private void SetVisuals()
    {
        if (hooksInRange > 1)
        {
            if (PlayerMovement.Instance.GetDirectionLook() == PlayerMovement.LookDirection.Up && PlayerMovement.Instance.GetPosition().y - transform.position.y < 0)
            {
                //PlayerMovement.Instance.Hook(transform.position);
            }
            if (PlayerMovement.Instance.GetDirectionLook() == PlayerMovement.LookDirection.Down && PlayerMovement.Instance.GetPosition().y - transform.position.y > 0)
            {
                //PlayerMovement.Instance.Hook(transform.position);
            }
            if (PlayerMovement.Instance.GetDirectionLook() == PlayerMovement.LookDirection.Left && PlayerMovement.Instance.GetPosition().x - transform.position.x > 0)
            {
                //PlayerMovement.Instance.Hook(transform.position);
            }
            if (PlayerMovement.Instance.GetDirectionLook() == PlayerMovement.LookDirection.Right && PlayerMovement.Instance.GetPosition().x - transform.position.x < 0)
            {
               // PlayerMovement.Instance.Hook(transform.position);
            }
        }
        else
        {
            //PlayerMovement.Instance.Hook(transform.position);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //PlayerMovementV2 player = collision.gameObject.GetComponent<PlayerMovementV2>();

            hooksInRange++;
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
            hooksInRange--;
            canHook = false;
        }
    }

    private void OnDrawGizmos()
    {
        if (canHook)
        {
            Gizmos.DrawLine(transform.position, PlayerMovement.Instance.GetPosition()  );
            
        }
        //Gizmos.DrawSphere(transform.position,cc.radius/5);
        
    }

}

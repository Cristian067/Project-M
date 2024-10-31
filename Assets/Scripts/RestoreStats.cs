using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestoreStats : MonoBehaviour
{
    private bool isInteract;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isInteract)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                GameManager.Instance.FullRestore();
            }
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            isInteract = true;
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isInteract = false;
        }
    }

}

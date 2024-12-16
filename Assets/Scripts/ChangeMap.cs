using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ChangeMap : MonoBehaviour
{

    [SerializeField] private int sceneId;
    [SerializeField] private bool haveToInteract;
    private bool canInteract;

    private void Update()
    {
        if (canInteract && Input.GetKeyDown(KeyCode.W))
        {
            GameManager.Instance.GoTo(sceneId);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (haveToInteract)
            {
                canInteract = true;
            }
            else
            {
                GameManager.Instance.GoTo(sceneId);
            }
            
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (haveToInteract)
            {
                canInteract = false;
            }
            

        }
    }
}

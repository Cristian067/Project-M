using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc : MonoBehaviour
{
    private bool canInteract;

   

    [SerializeField] private bool isInteract;

    private enum Actions
    {
        None,
        Shop,
        Talk
    }

    private enum SpecialInteraction
    {
        None,
        Heal,
        SpawnEnemy,
        BeBad,

    }

    [SerializeField]private Actions action;

    [SerializeField] private string[] talk;

    [SerializeField]private int idx = 0;
        
        
        

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canInteract) 
        { 
            if (action == Actions.Talk)
            {
               if (idx < talk.Length)
                {
                    PlayerMovement.Instance.changeInteracting(true);
                    UiManager.Instance.DialogueDisplay(talk[idx]);
                    idx++;
                }
                else if (PlayerMovement.Instance.isInteracting())
                {
                    idx = 0;
                    PlayerMovement.Instance.changeInteracting(false);
                    UiManager.Instance.DialogueUndisplay();

                }
            }

            //Hacer shopmenu

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && isInteract)
        {
            canInteract = true;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && isInteract)
        {
            canInteract = false;
        }
    }
}

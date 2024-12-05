using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc : MonoBehaviour
{
    private bool canInteract;

   

    [SerializeField] private bool isInteract;
    [SerializeField] private bool randomLines;

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
    [SerializeField] private string[] afterTalkText;

    [SerializeField] private int afterTalk;

    [SerializeField] private bool willAfterTalk;

    private int idx = 0;
        
        

   
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canInteract) 
        { 
            if (action == Actions.Talk)
            {
               if (idx < talk.Length && afterTalk == 0)
                {
                    PlayerMovementV2.Instance.ChangeInteracting(true);
                    UiManager.Instance.DialogueDisplay(talk[idx]);
                    idx++;
                }
                else if (PlayerMovementV2.Instance.isInteracting())
                {
                    idx = 0;
                    
                    
                    if(willAfterTalk)
                    {
                        afterTalk++;
                    }
                    
                    PlayerMovementV2.Instance.ChangeInteracting(false);
                    UiManager.Instance.DialogueUndisplay();

                }
               else if (willAfterTalk && afterTalk < afterTalkText.Length +1)
                {
                    PlayerMovementV2.Instance.ChangeInteracting(true);
                    UiManager.Instance.DialogueDisplay(afterTalkText[idx]);
                    
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDialogue : MonoBehaviour
{


    [SerializeField] private string[] talk;

    private int idx = 0;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {

            if (idx < talk.Length)
            {
                PlayerMovementV2.Instance.changeInteracting(true);
                UiManager.Instance.DialogueDisplay(talk[idx]);
                idx++;
            }
            else if (PlayerMovementV2.Instance.isInteracting())
            {
                idx = 0;

                PlayerMovementV2.Instance.changeInteracting(false);
                UiManager.Instance.DialogueUndisplay();
                this.gameObject.SetActive(false);

            }

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (idx < talk.Length)
        {
            PlayerMovementV2.Instance.changeInteracting(true);
            UiManager.Instance.DialogueDisplay(talk[idx]);
            idx++;
        }
    }
}

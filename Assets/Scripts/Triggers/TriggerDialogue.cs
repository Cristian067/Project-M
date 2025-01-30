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
                PlayerMovement.Instance.ChangeInteracting(true);
                UiManager.Instance.ShowDialogue(talk[idx]);
                idx++;
            }
            else if (PlayerMovement.Instance.isInteracting())
            {
                idx = 0;

                PlayerMovement.Instance.ChangeInteracting(false);
                UiManager.Instance.HideDialogue();
                this.gameObject.SetActive(false);

            }

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (idx < talk.Length)
        {
            PlayerMovement.Instance.ChangeInteracting(true);
            UiManager.Instance.ShowDialogue(talk[idx]);
            idx++;
        }
    }
}

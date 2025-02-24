using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetItem : MonoBehaviour
{
    [SerializeField] private ItemSO item;

    [SerializeField] private int id;

    [SerializeField] private bool isSpecialItem;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clip;

    [SerializeField] private GameObject itemPoint;

    private Tutorial tutorialScript;
    [SerializeField] private bool haveTutorial;

    private bool canInteract;
    [SerializeField]private bool isTaken;
    // Start is called before the first frame update
    void Start()
    {
        if (haveTutorial)
        {
            tutorialScript = GetComponent<Tutorial>();
        }
        
        audioSource = GameObject.Find("Sfx").GetComponent<AudioSource>();
        if(Save.Instance.LoadItemData(GameManager.Instance.GetFileNum()).Contains(id))
        {
            Destroy(gameObject);
        }
            
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!audioSource.isPlaying && isTaken)
        {
            if (haveTutorial)
            {
                tutorialScript.DeployTutorial();
            }
            
            Destroy(gameObject);
        }

        if (canInteract && Input.GetKeyDown(KeyCode.E))
        {
            Get();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            canInteract = true;
            itemPoint.SetActive(true);
        }
        //Destroy(gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            canInteract = false;
            itemPoint.SetActive(false);
        }
    }

    private void Get()
    {
        if (isSpecialItem)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
        
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.enabled = false;
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;

        //PlayerMovementV2 player = collision.gameObject.GetComponent<PlayerMovementV2>();

        Inventory.Instance.GetItem(item, isSpecialItem);
        Save.Instance.AddTempData("Item", id);
        PlayerMovement.Instance.CheckHabilities();

        isTaken = true;
    }

}

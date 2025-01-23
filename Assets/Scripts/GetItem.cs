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

    [SerializeField]private bool isTaken;
    // Start is called before the first frame update
    void Start()
    {
        //id = item.id;
        /*
        List<ItemSO> itemsInInventory = Inventory.Instance.GetItemsForSave(true);

        itemsInInventory.Contains(item)*/
        if(Save.Instance.LoadItemData(GameManager.Instance.GetFileNum()).Contains(id))
        {
            Destroy(gameObject);
        }
            
        //audioSource = GetComponent<AudioSource>();
        /*
        if (GameManager.Instance.GetHabilities(hability.ToString()))
        {
            isTaken = true;
        }
        */

        //StartCoroutine(CheckHabilities());
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!audioSource.isPlaying && isTaken)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            audioSource.clip = clip;
            audioSource.Play();
            BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
            boxCollider.enabled = false;
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.enabled = false;

            PlayerMovementV2 player = collision.gameObject.GetComponent<PlayerMovementV2>();

            Inventory.Instance.GetItem(item, isSpecialItem);
            Save.Instance.AddTempData("Item", id);
            player.CheckHabilities();

            isTaken = true;

        }
        //Destroy(gameObject);
    }
    /*
    private IEnumerator CheckHabilities()
    {
        yield return new WaitForSeconds(0.1f);

        if (GameManager.Instance.GetHabilities(hability.ToString()))
        {
            isTaken = true;
        }
    }
    */
}

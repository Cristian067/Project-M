using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveGame : MonoBehaviour
{
    [SerializeField] private bool canSave;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canSave && Input.GetKeyDown(KeyCode.E))
        {
            Save.Instance.saveData(GameManager.Instance.GetFileNum(), GameManager.Instance.GetSouls(), GameManager.Instance.GetPlayerHealth(), GameManager.Instance.GetOil(), GameManager.Instance.GetPlayerDamage(), SceneManager.GetActiveScene().name, transform.position, GameManager.Instance.GetHabilities("basic"), GameManager.Instance.GetHabilities("hook"), GameManager.Instance.GetHabilities("fireball"), GameManager.Instance.GetHabilities("doblejump"), GameManager.Instance.GetHabilities("walljump"), GameManager.Instance.GetTimePlayed(), Inventory.Instance.GetItemsForSave(false), Inventory.Instance.GetItemsForSave(true));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            canSave = true;
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            canSave = false;
        }
        
    }
}

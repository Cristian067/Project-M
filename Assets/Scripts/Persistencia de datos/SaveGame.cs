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
        if (canSave && Input.GetKeyDown(KeyCode.R))
        {
            Save.Instance.saveData(GameManager.Instance.GetFileNum(), GameManager.Instance.GetPlayerHealth(), GameManager.Instance.GetOil(), GameManager.Instance.GetPlayerDamage(), SceneManager.GetActiveScene().name, transform.position, GameManager.Instance.GetHabilities("basic"), GameManager.Instance.GetHabilities("hook"), GameManager.Instance.GetHabilities("fireball"), GameManager.Instance.GetHabilities("doblejump"));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        canSave = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        canSave = false;
    }
}

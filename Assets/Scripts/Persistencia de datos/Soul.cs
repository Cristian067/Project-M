using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soul : MonoBehaviour
{
    [SerializeField] private int soulId;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameManager.Instance.GetASoul();

            //PlayerPrefs.SetInt(name + "_" + GameManager.Instance.GetFileNum(), 1);
            Save.Instance.AddTempData("Soul", soulId);
            Destroy(gameObject);
        }
    }
    private void OnEnable()
    {
        if((Save.Instance.LoadSoulsData(GameManager.Instance.GetFileNum()).Contains(soulId)))
        {
            Save.Instance.AddTempData("Soul", soulId);
            Destroy(gameObject);
        }
    }
}

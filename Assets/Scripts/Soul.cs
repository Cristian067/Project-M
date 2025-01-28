using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soul : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameManager.Instance.GetASoul();
            
            //PlayerPrefs.SetInt(name + "_" + GameManager.Instance.GetFileNum(), 1);
            Destroy(gameObject);
        }
    }
    private void OnEnable()
    {
        if(PlayerPrefs.GetInt(name +"_"+ GameManager.Instance.GetFileNum()) == 1)
        {
            Destroy(gameObject);
        }
    }
}

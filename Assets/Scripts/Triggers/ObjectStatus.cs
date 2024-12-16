using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectStatus : MonoBehaviour
{
    private int dead;


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(PlayerPrefs.GetInt(name + "_" + GameManager.Instance.GetFileNum()));
        dead = PlayerPrefs.GetInt(name + "_" + GameManager.Instance.GetFileNum());
        

        if (dead == 1)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void KillObject()
    {
        InfoObjects.AddToList(name, 1);

        //Debug.Log(InfoObjects.objects.Count);
    }

    private void OnDestroy()
    {
        KillObject();
    }

}

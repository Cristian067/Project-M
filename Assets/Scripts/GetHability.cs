using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetHability : MonoBehaviour
{

    private enum test
    {
        Hook,
        Fireball
    }

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
            Debug.Log("Ahora tienes el gancho :D");
            Destroy(gameObject);
        }
    }

}

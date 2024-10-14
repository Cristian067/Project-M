using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetHability : MonoBehaviour
{

    
    public enum habilities
    {
        BasicAtq,
        Hook,
        Fireball,
        DobleJump
    }

    public habilities hability;

    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        audioSource.Play();

        if (collision.gameObject.tag == "Player")
        {
            if (hability == habilities.BasicAtq)
            {
                GameManager.Instance.SetHabilities("basic");
            }
            if (hability == habilities.Hook)
            {
                GameManager.Instance.SetHabilities("hook");
            }
            if (hability == habilities.Fireball)
            {
                GameManager.Instance.SetHabilities("fireball");
            }
            if (hability == habilities.DobleJump)
            {
                GameManager.Instance.SetHabilities("doblejump");
            }
        }

        Destroy(gameObject);
    }



}

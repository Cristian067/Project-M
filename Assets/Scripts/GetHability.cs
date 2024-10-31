using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetHability : MonoBehaviour
{

    
    public enum habilities
    {
        basic,
        hook,
        fireball,
        doblejump
    }

    public habilities hability;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clip;

    [SerializeField]private bool isTaken;
    // Start is called before the first frame update
    void Start()
    {
        //audioSource = GetComponent<AudioSource>();

        if (GameManager.Instance.GetHabilities(hability.ToString()))
        {
            isTaken = true;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.GetHabilities(hability.ToString()))
        {
            isTaken = true;
        }
        if (!audioSource.isPlaying && isTaken)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        audioSource.clip = clip;
        audioSource.Play();
        CircleCollider2D circleCollider = GetComponent<CircleCollider2D>();
        circleCollider.enabled = false;
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;


        if (collision.gameObject.tag == "Player")
        {
            if (hability == habilities.basic)
            {
                GameManager.Instance.SetHabilities(hability.ToString());
            }
            if (hability == habilities.hook)
            {
                GameManager.Instance.SetHabilities(hability.ToString());
            }
            if (hability == habilities.fireball)
            {
                GameManager.Instance.SetHabilities(hability.ToString());
            }
            if (hability == habilities.doblejump)
            {
                GameManager.Instance.SetHabilities(hability.ToString());
            }
        }

        isTaken = true;
        
        //Destroy(gameObject);
    }



}

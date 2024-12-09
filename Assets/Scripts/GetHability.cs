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
        doblejump,
        walljump
    }

    public habilities hability;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clip;

    [SerializeField]private bool isTaken;
    // Start is called before the first frame update
    void Start()
    {
        //audioSource = GetComponent<AudioSource>();
        /*
        if (GameManager.Instance.GetHabilities(hability.ToString()))
        {
            isTaken = true;
        }
        */

        StartCoroutine(CheckHabilities());
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
            CircleCollider2D circleCollider = GetComponent<CircleCollider2D>();
            circleCollider.enabled = false;
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.enabled = false;

            PlayerMovementV2 player = collision.gameObject.GetComponent<PlayerMovementV2>();
            
            GameManager.Instance.SetHabilities(hability.ToString());
            player.ForceCheckHabilities();

            isTaken = true;

        }

        
        
        //Destroy(gameObject);
    }

    private IEnumerator CheckHabilities()
    {
        yield return new WaitForSeconds(0.1f);

        if (GameManager.Instance.GetHabilities(hability.ToString()))
        {
            isTaken = true;
        }
    }

}

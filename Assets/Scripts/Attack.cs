using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("Disapear");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            enemy.LoseLive(GameManager.Instance.GetPlayerDamage());
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    private IEnumerator Disapear()
    {
        yield return new WaitForSeconds(0.02f);
        Destroy(gameObject);
    }

    

}

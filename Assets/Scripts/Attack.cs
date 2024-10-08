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
        Rigidbody2D collisionRb = collision.GetComponent<Rigidbody2D>();
        
        if (collision.gameObject.tag == "Enemy")
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            enemy.LoseLive(GameManager.Instance.GetPlayerDamage());
            //collisionRb.AddForce(-(collision.transform.position - transform.position) * 3000);
        }

        if(gameObject.transform.parent.gameObject.tag == "Enemy" && collision.gameObject.tag == "Player")
        {
            //Debug.Log("hit");
            GameManager.Instance.LoseLive(1);
            
        }


    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    private IEnumerator Disapear()
    {
        yield return new WaitForSeconds(0.03f);
        //Destroy(gameObject.transform.parent.gameObject);
        Destroy(gameObject);
    }

    

}

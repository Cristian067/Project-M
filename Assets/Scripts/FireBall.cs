using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    [SerializeField] private int damage;

    [SerializeField] private int speed;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("Disapear");
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            enemy.LoseLive(damage);
        }
        if (collision.gameObject.tag == "Boss")
        {
            Stats stats = collision.gameObject.GetComponent<Stats>();
            stats.LoseLive(damage);
        }
    }
    private IEnumerator Disapear()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);

    }
}

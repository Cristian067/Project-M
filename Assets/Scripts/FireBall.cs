using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    [SerializeField] private int damage;

    [SerializeField] private int speed;
    [SerializeField] private bool enemy;

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
        if(collision.gameObject.tag == "Enemy" && !enemy)
        {
            EnemyBasics enemy = collision.gameObject.GetComponent<EnemyBasics>();
            enemy.Damaged(damage,(collision.transform.position - transform.position));
        }
        if (collision.gameObject.tag == "Boss" && !enemy)
        {
            Stats stats = collision.gameObject.GetComponent<Stats>();
            stats.LoseLive(damage);
        }
        if (enemy && collision.gameObject.tag == "Player")
        {
            PlayerMovement.Instance.Damaged(1, (collision.transform.position - transform.position));
        }
    }
    private IEnumerator Disapear()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);

    }
}

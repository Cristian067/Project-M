using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Detector : MonoBehaviour
{
    [SerializeField] private Vector2 _distance;
    private Rigidbody2D rb;

    private CircleCollider2D circleCollider;

    private Enemy enemy;


    // Start is called before the first frame update
    void Start()
    {
        rb = transform.parent.gameObject.GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();

        enemy = transform.parent.GetComponent<Enemy>();

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _distance = collision.gameObject.transform.position - transform.position;

           
            enemy.GetDistance(_distance);
            circleCollider.radius = 6;

            //Debug.Log(rb.velocity.x);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _distance = new Vector2(0, 0);
            enemy.GetDistance(_distance);

            circleCollider.radius = 4;

        }
    }
}

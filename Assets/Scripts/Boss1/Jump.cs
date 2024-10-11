using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Use(Rigidbody2D rb, Vector2 direction, float force)
    {
        rb.AddForce(direction * force);
        StartCoroutine(StayInAir(rb));

    }

    private IEnumerator StayInAir(Rigidbody2D rb)
    {
        yield return new WaitForSeconds(0.2f);
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        yield return new WaitForSeconds(1.5f);
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

    }

}

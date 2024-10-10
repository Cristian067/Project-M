using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{

    [SerializeField] private GameObject attack;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Use(Rigidbody2D rb, float dashForce)
    {
        GameObject attackInv = Instantiate(attack, transform.position, Quaternion.identity);
        //Debug.Log(transform.rotation.eulerAngles.y);
        if (transform.rotation.eulerAngles.y == 180)
        {
            
            rb.velocity = Vector3.zero;
            rb.AddForce(new Vector3(-1,0,0) * dashForce);

            attackInv.transform.localPosition = transform.position + new Vector3(-5, 0, 0); //= Instantiate(attack, transform.position - new Vector3(-5,0,0), Quaternion.identity);
            
        }
        else if (transform.rotation.y == 0)
        {
            rb.velocity = Vector3.zero;
            rb.AddForce((Vector3.right) * dashForce);
            attackInv.transform.localPosition = transform.position + new Vector3(5, 0, 0);
            //attackInv = Instantiate(attack, transform.position - new Vector3(5, 0, 0), Quaternion.identity);
        }
        attackInv.transform.localScale = new Vector3(10, 0.7f, 0);

        StartCoroutine(StayInAir(rb, dashForce));

    }

    private IEnumerator StayInAir(Rigidbody2D rb, float dashForce)
    {
        yield return new WaitForSeconds(0.2f);
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        yield return new WaitForSeconds(3);
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

    }


}

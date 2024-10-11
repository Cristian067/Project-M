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


    public void Use(Rigidbody2D rb, float dashForce, bool down)
    {
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        GameObject attackInv = Instantiate(attack, transform.position, Quaternion.identity);
        attackInv.transform.parent = transform;
        //Debug.Log(transform.rotation.eulerAngles.y);
        if (transform.rotation.eulerAngles.y == 180 && !down)
        {
            
            rb.velocity = Vector3.zero;
            rb.AddForce(new Vector3(-1,0,0) * dashForce);

            attackInv.transform.localPosition = new Vector3(2.5f, 0, 0); //= Instantiate(attack, transform.position - new Vector3(-5,0,0), Quaternion.identity);
            
        }
        else if (transform.rotation.y == 0 && !down)
        {
            rb.velocity = Vector3.zero;
            rb.AddForce((Vector3.right) * dashForce);
            attackInv.transform.localPosition = new Vector3(2.5f, 0, 0);
            //attackInv = Instantiate(attack, transform.position - new Vector3(5, 0, 0), Quaternion.identity);
        }
        else if (down)
        {
            rb.velocity = Vector3.zero;
            rb.AddForce((Vector3.down * 2) * dashForce);
            attackInv.transform.rotation = Quaternion.Euler(new Vector3(0,-90,0));
            attackInv.transform.localPosition = new Vector3(2.5f, 0, 0);

        }
        attackInv.transform.localScale = new Vector3(5, 0.7f, 0);

        StartCoroutine(StayInAir(rb, dashForce));

    }

    private IEnumerator StayInAir(Rigidbody2D rb, float dashForce)
    {
        yield return new WaitForSeconds(0.2f);
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        yield return new WaitForSeconds(1.2f);
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

    }


}

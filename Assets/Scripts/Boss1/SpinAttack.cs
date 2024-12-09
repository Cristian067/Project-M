using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAttack : MonoBehaviour
{

    [SerializeField] private GameObject attack;

    public void Use(Rigidbody2D rb)
    {
        StartCoroutine(StayInAir(rb));
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        GameObject spin = Instantiate(attack, transform.position, Quaternion.identity);
        spin.transform.parent = transform;
        //Debug.Log(transform.rotation.eulerAngles.y);
        if (transform.rotation.eulerAngles.y == 180)
        {
            rb.velocity = Vector3.zero;
            //spin.transform.localPosition = new Vector3(2.5f, 0, 0); //= Instantiate(attack, transform.position - new Vector3(-5,0,0), Quaternion.identity);
        }
        else if (transform.rotation.y == 0)
        {
            rb.velocity = Vector3.zero;
            //spin.transform.localPosition = new Vector3(2.5f, 0, 0);
            //attackInv = Instantiate(attack, transform.position - new Vector3(5, 0, 0), Quaternion.identity);
        }
        
    }
    private IEnumerator StayInAir(Rigidbody2D rb)
    {
        yield return new WaitForSeconds(0.3f);
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        yield return new WaitForSeconds(1.2f);
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
}

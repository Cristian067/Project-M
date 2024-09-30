using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField] private int lives;

    [SerializeField] private GameObject attack;

    [SerializeField] private string lookDirection;

    private bool attackInCooldown;
    [SerializeField] private float attackCooldown;

    // Start is called before the first frame update
    void Start()
    {
        attackInCooldown = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (lives <= 0)
        {
            Destroy(gameObject);
        }

        
        if (!attackInCooldown)
        {
            Attack();
        }
        
    }

    public void LoseLive(int damage)
    {
        lives -= damage;
    }

    public void Attack()
    {
        //yield return new WaitForSeconds(1);

        Vector3 attackDirection = new Vector3(0, 0, 0);
        Vector3 offset = new Vector3(0, 0, 0);
        Quaternion attackRotation = Quaternion.identity;

        if (lookDirection == "left")
        {
            //attackDirection = new Vector3(-1f,0,0);
            offset = new Vector3(-1f, 0, 0);
            attackRotation = transform.rotation;
        }
        else if (lookDirection == "right")
        {
            //attackDirection = new Vector3(1f,0,0);
            offset = new Vector3(1f, 0, 0);
            attackRotation = transform.rotation;
        }
        

        Instantiate(attack, transform.position + attackDirection + offset, attackRotation, this.transform);
        StartCoroutine("AttackCooldown");

    }


    private IEnumerator AttackCooldown()
    {
        attackInCooldown = true;
        yield return new WaitForSeconds(attackCooldown);
        attackInCooldown = false;
    }

}

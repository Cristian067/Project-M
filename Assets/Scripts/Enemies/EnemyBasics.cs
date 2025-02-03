using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBasics : MonoBehaviour
{
    [SerializeField] private int lives;

    private Rigidbody2D rb;
    private bool knockback;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (lives <= 0)
        {
            Destroy(gameObject);
        }

    }
    public void LoseLive(int damage)
    {
        lives -= damage;
    }

    public void Damaged(int damage, Vector3 knockbackDir)
    {

        LoseLive(damage);
        //animator.SetTrigger("isDamaged");

        Debug.Log(knockbackDir.normalized);
        rb.velocity = (new Vector3(0f, 0.5f, 0) + knockbackDir.normalized) * 10;

        StartCoroutine(KnockbackTime());
        //animator.ResetTrigger("isDamaged");

    }

    private IEnumerator KnockbackTime()
    {
        knockback = true;
        yield return new WaitForSeconds(0.5f);
        knockback = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlUnit : MonoBehaviour
{
    private Dash dash;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {

        dash = GetComponent<Dash>();
        rb = GetComponent<Rigidbody2D>();

        dash.Use(rb, 1200);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            dash.Use(rb, 1200);
        }
    }

    private IEnumerator SelfCooldown()
    {
        yield return new WaitForSeconds(3);
    }
}

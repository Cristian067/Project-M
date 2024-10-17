using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    [SerializeField] private int lives;

    [SerializeField] private int damage;

    private ControlUnit controlUnit;

    // Start is called before the first frame update
    void Start()
    {
        controlUnit = GetComponent<ControlUnit>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void LoseLive(int damaged)
    {
        lives -= damaged;
        if (lives == 5)
        {
            controlUnit.BeamAttack();
        }
        if (lives <= 0)
        {
            Destroy(gameObject);
        }
    }

    public int GetDamage()
    {
        return damage;
    }

}

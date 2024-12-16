using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Stats : MonoBehaviour
{
    [SerializeField]private bool isAlive;
    [SerializeField] private string livingName;
    [SerializeField] private int lives;
    [SerializeField] private int damage;

    private ControlUnit controlUnit;

    private void Awake()
    {
        controlUnit = GetComponent<ControlUnit>();
    }

    void Start()
    {
        if(PlayerPrefs.GetInt(livingName + "_isDead_" + GameManager.Instance.GetFileNum()) == 1)
        {
            isAlive = false;
        }
        
        if (!isAlive)
        {
            Destroy(gameObject);
        }
        
    }

    private void KillNpc()
    {
        PlayerPrefs.SetInt(livingName + "_isDead_" + GameManager.Instance.GetFileNum(), 1);
                
    }

    public void LoseLive(int damaged)
    {
        lives -= damaged;
        //TODO: hacer beamattack
        /*
        if (lives == 5)
        {
            controlUnit.BeamAttack();
        }
        */
        if (lives <= 0)
        {
            KillNpc();
            Destroy(gameObject);
        }
    }

    public int GetDamage()
    {
        return damage;
    }
    public bool IsAlive()
    {
        return isAlive;
    }

}

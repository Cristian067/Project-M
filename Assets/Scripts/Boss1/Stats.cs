using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Stats : MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] private bool isAlive;
    [SerializeField] private string livingName;
    [SerializeField] private int lives;
    [SerializeField] private int damage;

    

    private void Awake()
    {
        
    }

    void Start()
    {
        if(Save.Instance.LoadBossData(GameManager.Instance.GetFileNum()).Contains(id))
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
        Save.Instance.AddTempData("Boss",id);
                
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

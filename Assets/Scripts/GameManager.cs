using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }

    [SerializeField] private int lives;

    [SerializeField] private int damage;

    [SerializeField] private int fuel;

    [SerializeField] private GameObject player;


    [SerializeField] private bool haveMelee;
    [SerializeField] private bool haveHook;
    [SerializeField] private bool haveFireball;
    [SerializeField] private bool haveDobleJump;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Existe mas de un Game Manager");
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void LoseLive(int damage)
    {
        lives -= damage;

        if (lives <= 0)
        {
            Destroy(player);
        }
        UiManager.instance.RefreshLives(lives);

    }

    public void HealLive(int heal)
    {
        lives += heal;
        UiManager.instance.RefreshLives(lives);
    }


    public int GetPlayerDamage()
    {
        return damage;
    }

    public int GetOil()
    {
        return fuel;
    }
    public void UseFuel(int usedFuel)
    {
        fuel -= usedFuel;
    }
    public void RechargeFuel(int moreFuel)
    {
        fuel += moreFuel;
    }

    public bool GetHabilities(string hability)
    {
        if (hability == "basic")
        {
            return haveMelee;
        }
        if (hability == "hook")
        {
            return haveHook;
        }
        if (hability == "fireball")
        {
            return haveFireball;
        }
        if (hability == "doblejump")
        {
            return haveDobleJump;
        }

        return false;

    }
    public void SetHabilities(string hability)
    {
        if (hability == "basic")
        {
            haveMelee = true;
        }
        if (hability == "hook")
        {
            haveHook = true;
        }
        if (hability == "fireball")
        {
            haveFireball = true;
        }
        if (hability == "doblejump")
        {
            haveDobleJump = true;
        }

        

    }
}

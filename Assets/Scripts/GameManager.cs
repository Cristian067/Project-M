using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }

    [SerializeField] private int lives;

    [SerializeField] private int damage;

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

    }

    public void HealLive(int heal)
    {
        lives += heal;
    }


    public int GetPlayerDamage()
    {
        return damage;
    }

}

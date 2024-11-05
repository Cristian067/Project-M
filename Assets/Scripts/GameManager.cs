using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }

    private int fileNum;

    //private Save save;



    [SerializeField] private int lives;
    [SerializeField] private int maxLives;
    [SerializeField] private bool invencibility;

    [SerializeField] private int damage;

    [SerializeField] private int fuel;
    [SerializeField] private int maxFuel;

    [SerializeField] private GameObject player;


    [SerializeField] private bool haveMelee;
    [SerializeField] private bool haveHook;
    [SerializeField] private bool haveFireball;
    [SerializeField] private bool haveDobleJump;

    private bool paused;

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
        Time.timeScale = 1;
        fileNum = PlayerPrefs.GetInt("actualFile");
        
        Save.Instance.LoadData(fileNum);

        paused = false;

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            
            if(!paused)
            {
                
                Pause();
            }
            else if (paused)
            {
                
                UnPause();
            }
            
        }
    }

    private void Pause()
    {
        
        UiManager.Instance.DisplayEscMenu();
        Time.timeScale = 0f;
        paused = true;
    }

    private void UnPause()
    {
        UiManager.Instance.UndisplayEscMenu();
        Time.timeScale = 1f;
        paused = false;
    }


    public void LoseLive(int damage)
    {
        if (!invencibility)
        {
            lives -= damage;

            if (lives <= 0)
            {
                Destroy(player);
            }
            UiManager.Instance.RefreshLives(lives);
            StartCoroutine(HitCooldown());

        }
        

    }
    private IEnumerator HitCooldown()
    {
        if (!invencibility)
        {
            invencibility = true;
            yield return new WaitForSeconds(2);
            invencibility = false;
        }

        yield return null;
    }
    public void HealLive(int heal)
    {
        lives += heal;
        UiManager.Instance.RefreshLives(lives);
    }
    public int GetPlayerHealth()
    {
        return lives;
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

    public int GetFileNum()
    {
        return fileNum;
    }
    
    public void GetData(int _lives, int _fuel, int _damage,Vector3 pos, bool _haveMelee,bool _haveHook , bool _haveFireball,bool _haveDobleJump)
    {
        lives = _lives;
        fuel = _fuel;
        damage = _damage;

        haveMelee = _haveMelee;
        haveHook = _haveHook;
        haveFireball = _haveFireball;
        haveDobleJump = _haveDobleJump;

        UiManager.Instance.RefreshLives(lives);

        SetPlayerData(pos);
    }
    private void SetPlayerData(Vector3 pos)
    {

        player.transform.position = pos;

    }



    public void GoTo(int id)
    {
        SceneManager.LoadScene(id);
    }

    public void FullRestore()
    {
        lives = maxLives;
        fuel = maxFuel;
        UiManager.Instance.RefreshLives(lives);
    }

    public bool IsPaused()
    {
        return paused;
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }

    private int fileNum = 1;

    private float timePlayed;
    private float totalTimePlayed;

    //private Save save;


    // Vida
    [SerializeField] private int lives;
    [SerializeField] private int maxLives;
    [SerializeField] private bool invencibility;

    //Daño a hacer
    [SerializeField] private int damage;

    //El "mana" 
    [SerializeField] private int fuel;
    [SerializeField] private int maxFuel;


    //Referencia del jugador para colocarse en el punto donde se guardo
    [SerializeField] private GameObject player;

    private bool isMapChanged;
    private Vector2 posToChange;

    //Los trozos de almas que se consiguen
    [SerializeField] private int souls;

    //Mirar y guardar las habilidades
    [SerializeField] private bool haveMelee;
    [SerializeField] private bool haveHook;
    [SerializeField] private bool haveFireball;
    [SerializeField] private bool haveDobleJump;
    [SerializeField] private bool haveWallJump;

    //pausa?
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
        //Debug.Log(PlayerPrefs.GetFloat("newPosX") + PlayerPrefs.GetFloat("newPosY"));
        if (PlayerPrefs.GetInt("mapInChange") == 1)
        {
            Debug.Log($"{PlayerPrefs.GetFloat("newPosX")} + {PlayerPrefs.GetFloat("newPosY")}");
            player.transform.position = new Vector2(PlayerPrefs.GetFloat("newPosX"), PlayerPrefs.GetFloat("newPosY"));
            PlayerPrefs.SetInt("mapInChange",0);
        }

    }
    
    public void MapChanged(bool mapChange, float posToBeX, float posToBeY, int mapId)
    {
        if (mapChange)
        {
            PlayerPrefs.SetInt("mapInChange", 1);
            //Debug.Log(posToBeY);
            PlayerPrefs.SetFloat("newPosX", posToBeX);
            PlayerPrefs.SetFloat("newPosY", posToBeY);
            
            GoTo(mapId);
            //posToChange = posToBe;
            //isMapChanged = true;
        }

    }
    // Update is called once per frame
    void Update()
    {

        timePlayed = Time.time;
        if(InputControl.Esc())
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
        
        UiManager.Instance.ShowEscMenu();
        Time.timeScale = 0f;
        paused = true;
    }

    private void UnPause()
    {
        UiManager.Instance.HideEscMenu();
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
                PlayerMovement.Instance.ChangeInteracting(true);
                PlayerMovement.Instance.SetAnimationBool("isDead", true);
                PlayerMovement.Instance.KillPlayer();
                UiManager.Instance.ShowGameOverPanel();
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
    public bool GetInvencibility()
    {
        return invencibility;
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
        if (hability == "walljump")
        {
            return haveWallJump;
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
        if (hability == "walljump")
        {
            haveWallJump = true;
        }



    }

    public int GetFileNum()
    {
        return fileNum;
    }
    public float GetTimePlayed()
    {
        return timePlayed + totalTimePlayed;
    }
    
    /*
    public void SetTimePlayed(float time)
    {
        timePlayed = time;
    }*/
    public void GetData(int _souls, int _lives, int _fuel, int _damage,Vector3 pos, bool _haveMelee,bool _haveHook , bool _haveFireball,bool _haveDobleJump, bool _haveWallJump, float _timePlayed)
    {
        lives = _lives;
        souls = _souls;
        fuel = _fuel;
        damage = _damage;

        haveMelee = _haveMelee;
        haveHook = _haveHook;
        haveFireball = _haveFireball;
        haveDobleJump = _haveDobleJump;
        haveWallJump = _haveWallJump;
        totalTimePlayed = _timePlayed;
        

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
    public void Retry()
    {
        //Debug.Log(SceneManager.GetActiveScene().buildIndex);
        GoTo(SceneManager.GetActiveScene().buildIndex);
    }

    public void ChangeInteractingWithTime(float time, bool change)
    {

        StartCoroutine(ChangeInteractCoroutine(time, change));

    }

    private IEnumerator ChangeInteractCoroutine(float time, bool change)
    {
        yield return new WaitForSeconds(time);
        PlayerMovement.Instance.ChangeInteracting(change);
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

    public void GetASoul()
    {
        souls++;
    }
    public void ChangeSouls(int _souls)
    {
        souls = _souls;
    }
    public int GetSouls()
    {
        return souls;
    }

}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Save : MonoBehaviour
{
    static public Save Instance { get; private set; }


    private string path = Application.dataPath + "/../saves/";

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    //private bool inTitleScreen = true;
    void Start()
    {



    }

    // Update is called once per frame
    void Update()
    {

    }

    public void saveData(int fileNum,int souls,int lives, int fuel, int damage, string mapIn, Vector3 setPos, bool melee, bool hook, bool fireball, bool dobleJump, bool wallJump, float setTimePlayed)
    {



        Progresion save = new Progresion
        {
            //name = playerName,
            file = fileNum,
            souls = souls,
            lives = lives,
            fuel = fuel, 
            damage = damage,

            map = mapIn,
            mapPosition = setPos,

            haveMelee = melee,
            haveHook = hook,
            haveFireball = fireball,
            haveDobleJump = dobleJump,
            haveWallJump = wallJump,

            timePlayed = setTimePlayed,


            
        };
        

        string jsonContent = JsonUtility.ToJson(save);
        Debug.Log(jsonContent);
        File.WriteAllText(path + $"{fileNum}.json", jsonContent);

    }


    public void LoadData(int fileNum)
    {
        if (File.Exists(path + $"{fileNum}.json"))
        {
            string jsonContent = File.ReadAllText(path + $"{fileNum}.json");

            Progresion save = JsonUtility.FromJson<Progresion>(jsonContent);

            GameManager.Instance.GetData(save.souls,save.lives,save.fuel,save.damage, save.mapPosition,save.haveMelee,save.haveHook,save.haveFireball,save.haveDobleJump, save.haveWallJump, save.timePlayed);
            //if(!inTitleScreen){}

            //GameManager.Instance.Load(save.name, save.mapPosition, save.points, save.color, save.level, save.exp, save.initialPower, save.levelsCompleted, save.timePlayed);
            //GameManager.Instance.Load(save.name, save.mapPosition, save.points, save.color, save.level, save.exp, save.initialPower, save.levelsCompleted, save.timePlayed);
            //PlayerControlMap.Instance.SetPlayerPos(save.mapPosition);
        }
        else
        {
            Debug.LogError("¡¡¡ EL ARCHIVO DE GUARDADO NO EXISTE !!!");
        }
    }

    public void DeleteData(int fileNum)
    {
        if(File.Exists(path + $"{fileNum}.json"))
        {
            File.Delete(path + $"{fileNum}.json");
        }
    }

    public void LoadDataTittleScreen(int fileNum)
    {
        if (File.Exists(path + $"{fileNum}.json"))
        {
            string jsonContent = File.ReadAllText(path + $"{fileNum}.json");

            Stats save = JsonUtility.FromJson<Stats>(jsonContent);

            //if(!inTitleScreen){}

            //GameManager.Instance.Load(save.name, save.mapPosition, save.points, save.color, save.level, save.exp, save.initialPower, save.levelsCompleted, save.timePlayed);
            //GameManager.Instance.LoadTittleScreen(true, save.name, save.mapPosition, save.points, save.color, save.level, save.exp, save.initialPower, save.levelsCompleted, save.timePlayed);
            //PlayerControlMap.Instance.SetPlayerPos(save.mapPosition);
        }
        else if (!File.Exists(path))
        {
            //GameManager.Instance.LoadTittleScreen(false, "a", new Vector3(0, 0, 0), 1, new Color(0, 0, 0), 1, 0, 0, 0, 0);

        }
    }
    public bool ExistFileData(int fileNum)
    {
        if (File.Exists(path + $"{fileNum}.json"))
        {
            return true;
        }
        return false;
    }
    public int LoadDataTittleScreenLifes(int fileNum)
    {
        if (File.Exists(path + $"{fileNum}.json"))
        {
            string jsonContent = File.ReadAllText(path + $"{fileNum}.json");

            Progresion save = JsonUtility.FromJson<Progresion>(jsonContent);

            return save.lives;
        }
        else if (!File.Exists(path))
        {
            return 0;
        }
        return 0;
    }

    public int LoadDataTittleScreenFuel(int fileNum)
    {
        if (File.Exists(path + $"{fileNum}.json"))
        {
            string jsonContent = File.ReadAllText(path + $"{fileNum}.json");

            Progresion save = JsonUtility.FromJson<Progresion>(jsonContent);

            return save.fuel;
        }
        else if (!File.Exists(path))
        {
            return 0;
        }
        return 0;
    }

    public string LoadDataTittleScreenLocation(int fileNum)
    {
        if (File.Exists(path + $"{fileNum}.json"))
        {
            string jsonContent = File.ReadAllText(path + $"{fileNum}.json");

            Progresion save = JsonUtility.FromJson<Progresion>(jsonContent);

            return save.map;
        }
        else if (!File.Exists(path))
        {
            return "";
        }
        return "";
    }




}
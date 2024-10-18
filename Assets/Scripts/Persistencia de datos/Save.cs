using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Save : MonoBehaviour
{
    static public Save Instance { get; private set; }


    private string path = Application.dataPath + "/../saves/" + "save.json";

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    private bool inTitleScreen = true;
    void Start()
    {



    }

    // Update is called once per frame
    void Update()
    {

    }

    public void saveData(int lives, int fuel, int damage, string mapIn, Vector3 setPos, bool melee, bool hook, bool fireball, bool dobleJump)
    {

        Progresion save = new Progresion
        {
            //name = playerName,

            lives = lives,
            fuel = fuel, 
            damage = damage,

            map = mapIn,
            mapPosition = setPos,

            haveMelee = melee,
            haveHook = hook,
            haveFireball = fireball,
            haveDobleJump = dobleJump

    //timePlayed = setTimePlayed,
};


        string jsonContent = JsonUtility.ToJson(save);
        Debug.Log(jsonContent);
        File.WriteAllText(path, jsonContent);

    }


    public void LoadData()
    {
        if (File.Exists(path))
        {
            string jsonContent = File.ReadAllText(path);

            Progresion save = JsonUtility.FromJson<Progresion>(jsonContent);

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

    public void LoadDataTittleScreen()
    {
        if (File.Exists(path))
        {
            string jsonContent = File.ReadAllText(path);

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


}
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TriggerSave : MonoBehaviour
{

    private string path = Application.dataPath + "/../saves/";

    private Stats stats;


    private void Start()
    {
        stats = GetComponent<Stats>();
    }


    public void saveData(int fileNum, int lives, int fuel, int damage, string mapIn, Vector3 setPos, bool melee, bool hook, bool fireball, bool dobleJump, bool wallJump)
    {

        Progresion save = new Progresion
        {
            //name = playerName,
            file = fileNum,
            lives = lives,
            fuel = fuel,
            damage = damage,

            map = mapIn,
            mapPosition = setPos,

            haveMelee = melee,
            haveHook = hook,
            haveFireball = fireball,
            haveDobleJump = dobleJump,
            haveWallJump = wallJump


            //timePlayed = setTimePlayed,
        };


        string jsonContent = JsonUtility.ToJson(save);
        Debug.Log(jsonContent);
        File.WriteAllText(path + $"{fileNum}.json", jsonContent);

    }
    private void Save(int fileNum)
    {
        if (File.Exists(path + $"{fileNum}.json"))
        {
            string jsonContent = File.ReadAllText(path + $"{fileNum}.json");

            Progresion save = JsonUtility.FromJson<Progresion>(jsonContent);

            saveData(fileNum,save.lives, save.fuel, save.damage,save.map, PlayerMovementV2.Instance.GetPosition(), save.haveMelee, save.haveHook, save.haveFireball, save.haveDobleJump, save.haveWallJump);
            
        }
    }

    private void OnDestroy()
    {
        if (stats.IsAlive())
        {
            Save(GameManager.Instance.GetFileNum());
        }
        
    }

}

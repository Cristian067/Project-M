using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerSave : MonoBehaviour
{

    private string path = Application.dataPath + "/../saves/";

    private Stats stats;


    private void Start()
    {
        stats = GetComponent<Stats>();
    }

    /*
    public void saveData(int fileNum, int lives, int fuel, int damage, string mapIn, Vector3 setPos, bool melee, bool hook, bool fireball, bool dobleJump, bool wallJump, float setTimePlayed)
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
            haveWallJump = wallJump,

            

            timePlayed = setTimePlayed,
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

            saveData(fileNum,save.lives, save.fuel, save.damage,save.map, PlayerMovementV2.Instance.GetPosition(), save.haveMelee, save.haveHook, save.haveFireball, save.haveDobleJump, save.haveWallJump, save.timePlayed);
            
        }
    }
    */
    private void OnDestroy()
    {
        if (stats.IsAlive())
        {
            Save.Instance.saveData(GameManager.Instance.GetFileNum(), GameManager.Instance.GetSouls(), GameManager.Instance.GetPlayerHealth(), GameManager.Instance.GetOil(), GameManager.Instance.GetPlayerDamage(), SceneManager.GetActiveScene().name, transform.position, GameManager.Instance.GetHabilities("basic"), GameManager.Instance.GetHabilities("hook"), GameManager.Instance.GetHabilities("fireball"), GameManager.Instance.GetHabilities("doblejump"), GameManager.Instance.GetHabilities("walljump"), GameManager.Instance.GetTimePlayed());

            for (int i = 0; i < InfoObjects.objects.Count; i++)
            {
                PlayerPrefs.SetInt(InfoObjects.objects[i] + "_" + GameManager.Instance.GetFileNum(), InfoObjects.dead[i]);
            }
        }
        
    }

}

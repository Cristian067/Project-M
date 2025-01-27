using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using System.Linq;
using Unity.VisualScripting;

public class Save : MonoBehaviour
{
    static public Save Instance { get; private set; }

    private List<int> tempItemId = new List<int>();
    private List<int> tempBossId = new List<int>();

    private string path = Application.dataPath + "/../saves/";

    private void Awake()
    {
        Instance = this;
    }
    public void saveData(int fileNum,int souls,int lives, int fuel, int damage, string mapIn, Vector3 setPos, bool melee, bool hook, bool fireball, bool dobleJump, bool wallJump, float setTimePlayed, List<ItemSO> _items, List<ItemSO> _specialItems /*List<int> itemId*/)
    {
        string[] specialItemsStringArray;
        List<string> specialItemsStringList = new List<string>();

        if (!File.Exists(path))
        {
            AssetDatabase.CreateFolder("../", "saves");
        }
        if (!File.Exists(path +"bosses/"))
        {
            AssetDatabase.CreateFolder("../saves/", "bosses");
        }

        foreach (ItemSO i in _specialItems)
        {
            specialItemsStringList.Add(i.name);
        }
        specialItemsStringArray = specialItemsStringList.ToArray();
        

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

            items = _items,
            specialItems = specialItemsStringArray,
            
            itemsID = tempItemId,
            bossesKilledID = tempBossId,

            
        };
        

        string jsonContent = JsonUtility.ToJson(save);
        Debug.Log(jsonContent);
        File.WriteAllText(path + $"{fileNum}.json", jsonContent);



    }


    public void AddTempData(string dataType, int tempNumber)
    {
        if(dataType == "Item")
        {
            tempItemId.Add(tempNumber);
        }

        if(dataType == "Boss")
        {
            tempBossId.Add(tempNumber);
        }

    }
    public List<int> LoadItemData(int fileNum)
    {
        if (File.Exists(path + $"{fileNum}.json"))
        {
            string jsonContent = File.ReadAllText(path + $"{fileNum}.json");
            Progresion save = JsonUtility.FromJson<Progresion>(jsonContent);
            return save.itemsID;
        }
        return null;

    }
    public List<int> LoadBossData(int fileNum)
    {
        if (File.Exists(path + $"{fileNum}.json"))
        {
            string jsonContent = File.ReadAllText(path + $"{fileNum}.json");
            Progresion save = JsonUtility.FromJson<Progresion>(jsonContent);
            return save.bossesKilledID;
        }
        return null;
    }
    public void SaveItemData()
    {

    }
    public void LoadData(int fileNum)
    {
        if (File.Exists(path + $"{fileNum}.json"))
        {
            string jsonContent = File.ReadAllText(path + $"{fileNum}.json");

            Progresion save = JsonUtility.FromJson<Progresion>(jsonContent);

            GameManager.Instance.GetData(save.souls,save.lives,save.fuel,save.damage, save.mapPosition,save.haveMelee,save.haveHook,save.haveFireball,save.haveDobleJump, save.haveWallJump, save.timePlayed);
            /*
            for (int i = 0; i < save.specialItems.Length; i++)
            {
                //save.specialItems[i];
                Debug.Log(ObjectsList.items[i].name);
                ObjectsList.items.ToList().Contains(save.specialItems[i].ConvertTo<ItemSO>());
                //Debug.Log(save.specialItems[i].ConvertTo<ItemSO>());
                //Inventory.Instance.GetItem(save.specialItems[i].ConvertTo<ItemSO>(), save.specialItems[i].ConvertTo<ItemSO>().specialItem);
            }*/
            foreach(ItemSO item in ObjectsList.Instance.items)
            {
                for (int i = 0;i < save.specialItems.Length; i++)
                {
                    if (item.name == save.specialItems[i])
                    {
                        Inventory.Instance.GetItem(item, item.specialItem);
                    }
                }
            }

            foreach (int i in save.itemsID)
            {
                AddTempData("Item", i);
            }

            for (int i = 0; i < save.items.Count; i++)
            {
                Inventory.Instance.GetItem(save.items[i], save.items[i].specialItem);
            }
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
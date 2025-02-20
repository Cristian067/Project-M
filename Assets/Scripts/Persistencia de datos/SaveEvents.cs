using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using UnityEngine.UIElements;



public class SaveEvents : MonoBehaviour
{
    static public SaveEvents Instance { get; private set; }

    private List<string> events = new List<string>();
    private string path = Application.dataPath + "/../saves/events/";

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        LoadData(GameManager.Instance.GetFileNum());
    }

    public void StoreData(string eventName)
    {
        events.Add(eventName);
    }
    public void SaveData(int filenum)
    {
        Events save = new Events
        {
            eventName = events
        };
        
        string jsonContent = JsonUtility.ToJson(save);
        Debug.Log(jsonContent);
        File.WriteAllText(path + $"{filenum}.json", jsonContent);
    }
    public void AddTempData(string dataType, int tempNumber)
    {
        
    }
    public void LoadData(int fileNum)
    {
        if (File.Exists(path + $"{fileNum}.json"))
        {
            string jsonContent = File.ReadAllText(path + $"{fileNum}.json");
            Events save = JsonUtility.FromJson<Events>(jsonContent);
            events = save.eventName;
            //Debug.Log(events.Count);
        }
       
    }
    
    public bool CheckIfComplete(string name)
    {
        //Debug.Log(events.Count);
        if (events.Contains(name))
        {
            return true;
        }
        else
        {
            return false;
        }
        
    }
    


}
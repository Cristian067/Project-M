using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class BreakWall : MonoBehaviour
{

    [SerializeField] private int live;
    private string path = "../saves";
    private bool breaked;

    // Start is called before the first frame update
    void Start()
    {
        saveData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void saveData()
    {
        
        /*
        if (!File.Exists(path))
        {
            AssetDatabase.CreateFolder("../saves", $"{GameManager.Instance.GetFileNum()}");
        }
        if (!File.Exists(path + "bosses/"))
        {
            AssetDatabase.CreateFolder("../saves/", "bosses");
        }

        */

        string jsonContent = JsonUtility.ToJson(breaked);
        Debug.Log(jsonContent);
        File.WriteAllText(path + $"{GameManager.Instance.GetFileNum()}a.json", jsonContent);



    }

    public void Damage()
    {
        if (live == 0)
        {
            Destroy(gameObject);
        }
    }

}

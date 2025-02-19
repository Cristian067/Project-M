using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance { get; private set; }

    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject gameFilesPanel;
    [SerializeField] private GameObject optionsPanel;

    [SerializeField] private Files file1;
    [SerializeField] private Files file2;
    [SerializeField] private Files file3;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Hay mas de un MainManager");
        }
    }

    
    
    public void GoTo(GameObject destination)
    {
        mainPanel.SetActive(false);
        gameFilesPanel.SetActive(false);
        optionsPanel.SetActive(false);
        destination.SetActive(true);
        //destination.SetActive(true);

    }

    public void TestButton()
    {
        Debug.Log("boton");
    }

    public void NewGame(int fileNum)
    {
        SceneManager.LoadScene(1);
        PlayerPrefs.SetInt("actualFile",fileNum);
        PlayerPrefs.Save();
    }

    public void Continue(int fileNum)
    {
        SceneManager.LoadScene(Save.Instance.LoadDataTittleScreenLocation(fileNum));
        PlayerPrefs.SetInt("actualFile", fileNum);
        PlayerPrefs.Save();
    }

    public void DeleteFile(int fileNum)
    {
        Save.Instance.DeleteData(fileNum);
        file1.CheckFiles();
        file2.CheckFiles();
        file3.CheckFiles();
    }

    public void ToCredits()
    {
        SceneManager.LoadScene(3);
    }

    public void Exit()
    {
        Application.Quit();
    }


}

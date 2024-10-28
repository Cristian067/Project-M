using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{

    public static MainManager Instance { get; private set; }


    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject gameFilesPanel;
    [SerializeField] private GameObject optionsPanel;


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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoTo(GameObject destination)
    {
        mainPanel.SetActive(false);
        gameFilesPanel.active = false;
        optionsPanel.SetActive(false);

        destination.active = true;
        //destination.SetActive(true);

    }


}

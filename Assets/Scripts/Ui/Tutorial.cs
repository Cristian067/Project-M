using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{

    [SerializeField] private GameObject tutorialPanel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DeployTutorial()
    {
        PlayerMovement.Instance.ForceStop();
        PlayerMovement.Instance.ChangeInteracting(true);
        Time.timeScale = 0;
        tutorialPanel.SetActive(true);
    }
}

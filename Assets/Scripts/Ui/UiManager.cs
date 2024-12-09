using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance { get; private set; }

    [SerializeField] private Slider fuelMeter;
    [SerializeField] private Slider live;

    [SerializeField] private GameObject escMenu;
    [SerializeField] private GameObject[] escPanels;

    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;

    [SerializeField] private GameObject GOPanel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Hay mas de un UiManager");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        escMenu.SetActive(false);
        GOPanel.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {

        fuelMeter.value = GameManager.Instance.GetOil();
        
    }

    public void RefreshLives(int lives)
    {
        live.value = lives;
    }

    public void ShowEscMenu()
    {
        escMenu.SetActive(true);

    }
    public void HideEscMenu()
    {
        escMenu.SetActive(false);
    }

    public void ChangeManuPanel(int panel)
    {
        for(int i = 0; i < escPanels.Length; i++)
        {
            escPanels[i].SetActive(false);
        }
        escPanels[panel].SetActive(true);
    }

    public void ShowDialogue(string msg)
    {
        dialoguePanel.SetActive(true);
        dialogueText.text = msg;
    }
    public void HideDialogue()
    {
        dialoguePanel.SetActive(false);
    }

    public void ShowGameOverPanel()
    {
        GOPanel.SetActive(true);
    }
    public void HideGameOverPanel()
    {
        GOPanel.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance { get; private set; }

    [SerializeField] private Slider fuelMeter;
    [SerializeField] private GameObject fuelMeterGO;
    [SerializeField] private Slider live;

    [SerializeField] private GameObject escMenu;
    [SerializeField] private GameObject[] escPanels;

    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;

    [SerializeField] private GameObject GOPanel;

    [SerializeField] private GameObject InfoPanel;
    [SerializeField] private Image InfoImage;
    [SerializeField] private TextMeshProUGUI InfoNameText;
    [SerializeField] private TextMeshProUGUI InfoDescriptionText;



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
        fuelMeterGO.SetActive(false);

        
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            if (Inventory.Instance.GetItemsForSave(true).Contains(ObjectsList.Instance.GetItemByName("Pedernal")))
            {
                fuelMeterGO.SetActive(true);
                fuelMeter.value = GameManager.Instance.GetOil();
            }
        }
        catch
        {
            
        }
        
        
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
        InfoPanel.SetActive(false);
    }

    public void ChangeManuPanel(int panel)
    {
        for(int i = 0; i < escPanels.Length; i++)
        {
            escPanels[i].SetActive(false);
        }
        escPanels[panel].SetActive(true);
    }
    public void ShowItemInfo(string itemName, string description, Sprite image)
    {
        InfoPanel.SetActive(true);
        InfoImage.sprite = image;
        InfoNameText.text = itemName;

        InfoDescriptionText.text = description;
    }
    public void HideItemInfo()
    {
        InfoPanel.SetActive(false);
        InfoNameText.text = "";
        InfoDescriptionText.text = "";
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

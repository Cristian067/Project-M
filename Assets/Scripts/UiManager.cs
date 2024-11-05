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

    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;

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

    public void DisplayEscMenu()
    {
        escMenu.SetActive(true);

    }
    public void UndisplayEscMenu()
    {
        escMenu.SetActive(false);
    }

    public void DialogueDisplay(string msg)
    {
        dialoguePanel.SetActive(true);
        dialogueText.text = msg;
    }
    public void DialogueUndisplay()
    {
        dialoguePanel.SetActive(false);
    }
}

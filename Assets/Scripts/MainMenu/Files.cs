using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Files : MonoBehaviour
{

    [SerializeField] private int fileNum;

    [SerializeField] private GameObject noExistPanel;
    [SerializeField] private GameObject infoPanel;

    [SerializeField] private TextMeshProUGUI lifeText;
    [SerializeField] private TextMeshProUGUI fuelText;
    [SerializeField] private TextMeshProUGUI locationText;

    [SerializeField] private Save save;

   



    // Start is called before the first frame update
    void Start()
    {
        save = FindAnyObjectByType<Save>();
        CheckFiles();  
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckFiles()
    {
        if (save.ExistFileData(fileNum))
        {
            noExistPanel.SetActive(false);
            infoPanel.SetActive(true);

            lifeText.text = $"Lives: {save.LoadDataTittleScreenLifes(fileNum)}";
            fuelText.text = $"Fuel: {save.LoadDataTittleScreenFuel(fileNum)}";
            locationText.text = $"Location: {save.LoadDataTittleScreenLocation(fileNum)}";


        }
        else
        {
            noExistPanel.SetActive(true);
            infoPanel.SetActive(false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public static UiManager instance { get; private set; }

    [SerializeField] private Slider fuelMeter;
    [SerializeField] private Slider live;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("Hay mas de un UiManager");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
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

}

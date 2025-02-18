using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPanel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (InputControl.Interact())
        {
            Time.timeScale = 1;
            PlayerMovement.Instance.ChangeInteracting(false);
            Destroy(gameObject);
        }
        
    }
}

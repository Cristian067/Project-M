using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerMap : MonoBehaviour
{
    [SerializeField] private GameObject blockInMap;

    [SerializeField] private bool isActive;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            blockInMap.SetActive(isActive);
        }
        
        
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCam : MonoBehaviour
{
    [SerializeField] private bool isFollow;

    [SerializeField] private Vector3 newCamPosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            CamControl.Instance.FollowPlayer(isFollow);
        }
        if (!isFollow)
        {
            CamControl.Instance.SetCamPosition(newCamPosition);
        }
        
    }


}

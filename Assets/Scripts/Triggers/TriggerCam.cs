using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCam : MonoBehaviour
{
    [SerializeField] private bool isFollow;

    [SerializeField] private bool isOnTrigger;

    [SerializeField] private Vector3 newCamPosition;

    [SerializeField] private Transform playerTransform;
    [SerializeField] private CinemachineVirtualCamera cam;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetNewPosition()
    {
        cam.Follow = null;
        cam.transform.position = newCamPosition;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {

        
            if (isOnTrigger)
            {
            if (isFollow)
            {
                cam.Follow = playerTransform;
            }
            if (!isFollow)
            {
                SetNewPosition();
            }
            }
        }
    }

    private void OnDestroy()
    {
        if (!isOnTrigger)
        {
            if (isFollow)
            {
                cam.Follow = playerTransform;
            }
            if (!isFollow)
            {
                SetNewPosition();
            }
        }
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamControl : MonoBehaviour
{
    public static CamControl Instance { get; private set; }

    [SerializeField]private GameObject player;

    [SerializeField] private bool followPlayer = false;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("Hay mas de un Camera Control");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (followPlayer)
        {
            transform.position = new Vector3(player.transform.position.x,player.transform.position.y +2,-10);
            //cam.transform.position.y = player.transform.position.y;
        }

        if (transform.position.x < -1)
        {
            transform.position = new Vector3(-1,transform.position.y,transform.position.z);
        }

    }

    public void FollowPlayer(bool follow)
    {
        followPlayer = follow;
    }

    public void UnFollowPlayer()
    {
        followPlayer = false;
    }

    public void SetCamPosition(Vector3 newCamPosition)
    {
        transform.position = newCamPosition;
    }

}

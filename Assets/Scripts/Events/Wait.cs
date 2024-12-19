using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Wait : MonoBehaviour
{
    [SerializeField] private Event1 mainEvent;
    [SerializeField] private float timeToWait;
    private void Start()
    {
        //mainEvent = FindAnyObjectByType<Event1>();
        //targetRb = target.GetComponent<Rigidbody2D>();
    }

    public void Use()
    {
        //mainEvent.InProcces(true);
        //move = true;
        //PlayerMovementV2.Instance.ChangeInteracting(true);
        StartCoroutine(WaitInSeconds());
    }

    private IEnumerator WaitInSeconds()
    {
        yield return new WaitForSeconds(timeToWait);
        //PlayerMovementV2.Instance.ChangeInteracting(false);
        mainEvent.Next();
    }

}

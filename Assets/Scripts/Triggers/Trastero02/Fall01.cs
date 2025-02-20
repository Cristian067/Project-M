using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fall01 : MonoBehaviour
{
    private void Start()
    {
        if (SaveEvents.Instance.CheckIfComplete(name))
        {
            //Debug.Log("si");
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerMovement.Instance.ChangeInteracting(true);
            StartCoroutine(Cutscene());
        }
    }

    private IEnumerator Cutscene()
    {
        Animator anim = GetComponent<Animator>();
        anim.SetTrigger("trumble");
        yield return new WaitForSeconds(2f);
        EndCutscene();

        
    }
    public void EndCutscene()
    {
        GameManager.Instance.ChangeInteractingWithTime(1.5f, false);
        SaveEvents.Instance.StoreData(name);
        Destroy(gameObject);
        
        
        
    }
}

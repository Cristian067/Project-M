using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fall01 : MonoBehaviour
{
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
        Destroy(gameObject);
        
        
        
    }
}

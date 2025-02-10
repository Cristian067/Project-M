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
        yield return new WaitForSeconds(2.5f);
        EndCutscene();

        
    }
    public void EndCutscene()
    {
        PlayerMovement.Instance.ChangeInteracting(false);
        Destroy(gameObject);
    }
}

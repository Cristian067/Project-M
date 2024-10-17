using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamAttack : MonoBehaviour
{

    [SerializeField] private GameObject beam;
    [SerializeField] private AudioSource beamattack;
    // Start is called before the first frame update
    void Start()
    {
        beamattack = GetComponent<AudioSource>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Use(Rigidbody2D rb, Vector2 position)
    {

        ParticleSystem particleSystem = GetComponent<ParticleSystem>();


        transform.position = position;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        particleSystem.Play();
        StartCoroutine(Wait());

    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(7);
        Shoot();

        
    }

    private void Shoot()
    {
        beamattack.Play();
        GameObject beamAttack = Instantiate(beam, transform.position, Quaternion.identity);
        beamAttack.transform.position = new Vector3(70.5f, 19, 0);
    }

}

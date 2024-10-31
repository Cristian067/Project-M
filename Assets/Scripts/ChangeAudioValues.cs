using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeAudioValues : MonoBehaviour
{

    [SerializeField]private AudioSource audioSource;
    [SerializeField]private Slider slider;

    // Start is called before the first frame update
    void Start()
    {

        audioSource = GetComponent<AudioSource>();


        audioSource.volume = PlayerPrefs.GetFloat($"volume_{gameObject.name}");
        slider.value = audioSource.volume;

        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeVolumen(float volumen)
    {
        audioSource.volume = volumen;
        PlayerPrefs.SetFloat($"volume_{gameObject.name}",volumen);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    private AudioSource audioSource;
    static bool ifPlay = false;


    // Start is called before the first frame update
    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.I))
        //    audioSource.Play();
        if (BoatControlTrigger.ifEnterTrigger == false && ifPlay == true)
        {
            audioSource.Stop();
            ifPlay = false;
        }
        else if (BoatControlTrigger.ifEnterTrigger == true && ifPlay == false)
        {
            audioSource.Play();
            ifPlay = true;
        }
    }
}

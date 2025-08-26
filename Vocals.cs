using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vocals : MonoBehaviour
{
    public AudioSource source;
    public static Vocals instance;

    private void Awake(){
        instance = this;
    }

    private void Start(){
        source = gameObject.AddComponent<AudioSource>();
    }

    private void Say(AudioClip clip){

        if (source.isPlaying)
            source.Stop();
            
        source.PlayOneShot(clip);
    }
}

using UnityEngine;

public class MusicSheets : MonoBehaviour
{
    AudioSource voSource;

    bool clipPlayed = false;

    void Start(){
        voSource = GetComponent<AudioSource>();
    }

    public void TryPlayClip(){
        if(!clipPlayed){
            voSource.Play();
            clipPlayed = true;
        }
        else{
            Debug.Log("Cannot play music vo, already played");
        }
    }

    public void StopClip(){
        if(voSource.isPlaying){
            voSource.Stop();
        }
    }
}

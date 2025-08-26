using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class TVcontroller : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public VideoClip clip1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()

    {
        
    }

    public void PlayVideo(int videoIndex){
        videoPlayer.clip=clip1;
        videoPlayer.Play();
    }

}

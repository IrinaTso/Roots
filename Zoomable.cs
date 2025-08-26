using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zoomable : MonoBehaviour
{

    public float fov;

    public AudioClip toPlay;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void OnMouseUp(){
        // slerp to this

        PostcardManager.Instance.ZoomableClicked(this);
    }
}

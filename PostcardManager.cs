using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class PostcardManager : MonoBehaviour
{

    #region Singleton

    // Singleton instance /////

    private static PostcardManager _instance;

    public static PostcardManager Instance { get { return _instance; } }

    private void Awake(){
        if(_instance != null && _instance != this){
            Destroy(this.gameObject);
        }
        else{
            _instance = this;
        }
    }

    #endregion

    // Public variables

    public float flipSpeed;
    public float zoomSpeed;

    public SlerpCamera slerpCamera;

    public GameObject postcardParent;

    Vector3 startPos;
    AudioSource source;
    float startFOV;

    bool clipTriggered;
    bool isZoomed;
    bool isFlipping;

    void Start()
    {
        source = GetComponent<AudioSource>();

        startPos = slerpCamera.transform.position;
        startFOV = Camera.main.fieldOfView;
    }

    void Update()
    {
        if(clipTriggered && !source.isPlaying){
            // Clip finished
            ReturnToOriginal();
            clipTriggered = false;
        }
    }

    void ReturnToOriginal(){
        isZoomed = false;
        slerpCamera.MoveCam(startPos, startFOV, 3);
    }

    public void ZoomableClicked(Zoomable zoomable){

        if(!isZoomed && !isFlipping){
            Vector3 newPos = new Vector3(zoomable.transform.position.x, zoomable.transform.position.y, 0);
            slerpCamera.MoveCam(newPos, zoomable.fov, zoomSpeed);

            source.clip = zoomable.toPlay;
            source.Play();
            clipTriggered = true;

            isZoomed = true;
        }
    }

    public void Flip(){
        if(!isFlipping) StartCoroutine(FlipPostcard(flipSpeed));
    }

    IEnumerator FlipPostcard(float duration){
        float timer = 0;
        Vector3 startRot = postcardParent.transform.eulerAngles;
        Vector3 target = new Vector3(startRot.x, startRot.y + 180, startRot.z);

        isFlipping = true;

        while(timer < duration){
            postcardParent.transform.eulerAngles = Vector3.Slerp(startRot, target, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }
        postcardParent.transform.eulerAngles = target;

        isFlipping = false;
    }
}

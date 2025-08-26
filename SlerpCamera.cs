using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;

public class SlerpCamera : MonoBehaviour
{
    public bool triggerMove = false;
    public float transitionTime = 3;

    public Transform target;
    public float targetFOV = 60;

    void Start()
    {
        
    }

    void Update()
    {
        if(triggerMove){
            //ZoomIn();
            triggerMove = false;
        }
    }

    public void MoveCam(Vector3 pos, float fov, float time){
        StartCoroutine(SlerpCam(pos, fov, time));
    }

    IEnumerator SlerpCam(Vector3 targetPos, float targetFOV, float duration){
        float timer = 0;

        Vector3 startPos = transform.position;
        float startFOV = Camera.main.fieldOfView;

        while(timer < duration){
            transform.position = Vector3.Slerp(startPos, targetPos, timer / duration);
            Camera.main.fieldOfView = Mathf.SmoothStep(startFOV, targetFOV, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;
        Camera.main.fieldOfView = targetFOV;
    }
}

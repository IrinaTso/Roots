using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamPanner : MonoBehaviour
{
    public float mouseThreshold;

    public float panSpeed;
    public float minAngle;
    public float maxAngle;

    public bool camPanningActive;

    Vector2 mousePos;
    float currentRot;

    GameObject cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(camPanningActive){
            mousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);

            // Change to get distance from mouse.x to 1 or -1

            if(mousePos.x > 1 - mouseThreshold){
                // Pan right
                cam.transform.Rotate(Vector3.up, panSpeed * Time.deltaTime, Space.World);
                CheckAngle();
            }
            else if(mousePos.x  < mouseThreshold){
                // Pan left
                cam.transform.Rotate(Vector3.up, -panSpeed * Time.deltaTime, Space.World);
                CheckAngle();
            }
        }
    }

    void CheckAngle(){
        currentRot = cam.transform.localEulerAngles.y;

        if(currentRot >= maxAngle){
            // Max angle reached
        }
        else if(currentRot <= minAngle){
            // Min angle reached
        }
    }
}

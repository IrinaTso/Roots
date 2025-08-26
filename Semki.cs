using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Semki : MonoBehaviour
{
   public float TVdelay;
   public UnityEvent eventcunt;
   float timer;
   bool timerActive;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(timerActive)
        {
            timer+=Time.deltaTime;
            if(timer>=TVdelay){
                eventcunt.Invoke();
                timerActive=false;
            }
        }
    }
    public void StartTimer(){
        timer=0;
        timerActive=true;
        
    }
}

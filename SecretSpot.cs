using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SecretSpot : MonoBehaviour
{
    public UnityEvent onSpotFound;

    bool isFound;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpotFound(){
        if(!isFound){
            Debug.Log("Secret spot found!!!");
            isFound = true;

            onSpotFound.Invoke();
        }
    }
}

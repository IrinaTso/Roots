using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class TextScroller : MonoBehaviour
{
    // Public ////
    public float scrollSpeed;
    public bool scrollOnStart;

    // Private ////
    string originalString;
    char[] charList;
    int currentChar;

    bool scrollActive;

    float timer;

    TextMeshProUGUI text;
    
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();

        originalString = text.text;
        charList = originalString.ToCharArray();

        text.text = "";

        if(scrollOnStart) StartScrolling();
    }

    void Update()
    {
        if(scrollActive){
            if(timer > scrollSpeed){
                text.text += charList[currentChar];
                if(currentChar == charList.Length - 1){
                    Debug.Log("scroll finished");
                    scrollActive = false;
                    return;
                }
                else{
                    currentChar++;
                    timer = 0;
                }
                
            }
            timer += Time.deltaTime;
        }
    }

    public void StartScrolling(){
        scrollActive = true;
        timer = 0;

        currentChar = 0;
        
    }
}

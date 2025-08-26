using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Clickable : MonoBehaviour
{
    [Header("Settings")]
    public bool triggerOnce;

    [Header("Events")]
    public UnityEvent onClick;

    void OnMouseUp(){
        onClick.Invoke();
    }
}

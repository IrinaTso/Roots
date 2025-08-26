using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HoverOverText : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject hoverOverText;
    public void Start()
    {
        hoverOverText.SetActive(false);
    }
    public void OnMouseOver()
    {
        hoverOverText.SetActive(true);
    }

    public void OnMouseExit(){
        hoverOverText.SetActive(false);
    }

}

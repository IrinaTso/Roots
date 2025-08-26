using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using System;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [Serializable]
    public enum MenuStates { Main, Settings, Credits, Quit }

    [Serializable]
    public struct MenuParents{
        public GameObject mainParent;
        public GameObject settingsParent;
        public GameObject creditsParent;
        public GameObject quitParent;
    }

    [Serializable]
    public struct DollParents{
        public GameObject doll0;
        public GameObject doll1;
        public GameObject doll2;
    }

    public MenuStates currentState;
    public MenuParents menuParents;
    public DollParents dollParents;

    public string dollProgressKey;

    void Start()
    {        
        UpdateDollState();

        //SetMenuState(MenuStates.Main);
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void BeforeSceneLoaded(){
        Debug.Log("Before scene load, setting key to 0");
        PlayerPrefs.SetInt("DollProgress", 0);
    }

    public void SetMenuStateByIndex(int index){
        SetMenuState((MenuStates)index);
    }

    void SetMenuState(MenuStates newState){
        currentState = newState;

        switch(currentState){
            case MenuStates.Main:
                SetParentsActive(0);
                break;
            case MenuStates.Settings:
                SetParentsActive(1);
                break;
            case MenuStates.Credits:
                SetParentsActive(2);
                break;
            case MenuStates.Quit:
                SetParentsActive(3);
                break;
            default:
                break;
        }
    }

    void SetParentsActive(int index){
        menuParents.mainParent.SetActive(false);
        menuParents.settingsParent.SetActive(false);
        menuParents.creditsParent.SetActive(false);
        menuParents.quitParent.SetActive(false);

        if(index == 0) menuParents.mainParent.SetActive(true);
        else if(index == 1) menuParents.settingsParent.SetActive(true);
        else if(index == 2) menuParents.creditsParent.SetActive(true);
        else if(index == 3) menuParents.quitParent.SetActive(true);
    }

    void UpdateDollState(){

        if(!PlayerPrefs.HasKey(dollProgressKey)) PlayerPrefs.SetInt(dollProgressKey, 0);
        int currentState = PlayerPrefs.GetInt(dollProgressKey);

        dollParents.doll0.SetActive(false);
        dollParents.doll1.SetActive(false);
        dollParents.doll2.SetActive(false);

        switch(currentState){
            case 0:
                dollParents.doll0.SetActive(true);
                break;
            case 1:
                dollParents.doll1.SetActive(true);
                break;
            case 2:
                dollParents.doll2.SetActive(true);
                break;
            default:
                break;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public enum GameStates { None, Menu, Idle, Inspecting }

public class GameManager : MonoBehaviour
{

    #region Singleton

    // Singleton instance /////

    private static GameManager _instance;

    public static GameManager Instance { get { return _instance; } }

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

    public GameStates gameState;

    [Header("Settings")]
    public float inspectPosDistance;

    [Header("UI")]
    public RectTransform textPanel;
    public TextMeshProUGUI objectText;
    public float textHiddenXpos = -470;
    public float textVisibleXpos;

    [Header("References")]
    public Transform inspectPos;

    [Header("Debug")]
    public string debugPrefix = "GAMEMANAGER --- ";

    // Hidden
    [HideInInspector] public bool canPickupInspectable;

    // Private variables
    RaycastHit hitInfo;
    Inspectable currentInspectable;

    bool textPanelMoving;
    bool isTextPanelVisible;
    bool textPanelPlayerPreference;

    void Start()
    {
        //inspectPos.transform.localPosition = new Vector3(0, 0, inspectPosDistance);

        SetGameState(GameStates.Idle);
        textPanelPlayerPreference = true;
        objectText.text = "";
    }

    void Update()
    {
        if(gameState == GameStates.Inspecting) CheckForSecretSpot();
    }
    public void SetDollState(int index){
        PlayerPrefs.SetInt("DollProgress",index);
    }
    public void SetGameState(GameStates newState){
        if(gameState != newState){
            
            gameState = newState;

            switch(gameState){
                case GameStates.None:
                    canPickupInspectable = false;
                    break;
                case GameStates.Menu:
                    break;
                case GameStates.Idle:
                    canPickupInspectable = true;
                    break;
                case GameStates.Inspecting:
                    canPickupInspectable = false;
                    break;
            }

            Debug.Log(debugPrefix + "Game state changed to: " + gameState);
        }
    }

    public void CheckForSecretSpot(){
        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitInfo, 100f)){
            if(hitInfo.collider.tag == "SecretSpot"){
                hitInfo.collider.GetComponent<SecretSpot>().SpotFound();
            }
        }
    }

    public void InspectablePickedUp(Inspectable newInspectable){
        currentInspectable = newInspectable;
        
        if(currentInspectable.voText != ""){
            UpdateTextPanel(currentInspectable.voText);
        }
    }

    public void InspectableDropped(){
        HideTextPanel();
    }

    public void ToggleTextPanel(){
        if(isTextPanelVisible){
            HideTextPanel();
            textPanelPlayerPreference = false;
        }
        else{
            UpdateTextPanel(currentInspectable.voText);
            textPanelPlayerPreference = true;
        }
    }

    public void UpdateTextPanel(string newText){
        if(!textPanelMoving && !isTextPanelVisible){

            objectText.text = newText;

            //if(textPanelPlayerPreference){
                textPanel.anchoredPosition = new Vector2(textHiddenXpos, textPanel.anchoredPosition.y);

                StartCoroutine(LerpTextPanel(textVisibleXpos, 2));
                isTextPanelVisible = true;
            //}
        
        }
    }

    public void HideTextPanel(){
        if(!textPanelMoving && isTextPanelVisible){
            StartCoroutine(LerpTextPanel(textHiddenXpos, 2));
            isTextPanelVisible = false;
        }
    }

    IEnumerator LerpTextPanel(float xTarget, float duration){
        float time = 0;
        float startXpos = textPanel.anchoredPosition.x;

        textPanelMoving = true;

        while(time < duration){
            float x = Mathf.Lerp(startXpos, xTarget, time / duration);
            textPanel.anchoredPosition = new Vector2(x, textPanel.anchoredPosition.y);
            time += Time.deltaTime;

            yield return null;
        }

        textPanel.anchoredPosition = new Vector2(xTarget, textPanel.anchoredPosition.y);
        textPanelMoving = false;
    }
}

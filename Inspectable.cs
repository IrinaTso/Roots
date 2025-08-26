using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class Inspectable : MonoBehaviour
{
    public enum Position { Desk, Camera }
    
    // Public variables
    [Header("Settings")]
    public Position currentPosition;

    [Tooltip("The object's position will be offset by this much from the camera position.")]
    public Vector3 positionOffset;
    [Tooltip("The object's rotation will be offset by this many degrees from the default camera position.")]
    public Vector3 rotationOffset;

    [Tooltip("The amount of time it takes the object to move from desk to camera and vice versa.")]
    public float moveDuration;
    public float rotateSpeed;

    [Tooltip("If true, the connected audio source will stop playing when the object is put down on the desk.")]
    public bool stopAudioOnRelease;
    [Tooltip("Maximum amount of time to count a mouse input as a click and not a hold and drag.")]
    public float clickTime = 0.2f;

    [Header("Content")]
    [Tooltip("The audio clip to play when this object is picked up.")]
    public AudioClip clip;
    public GameObject enableOnMouseOver;
    public string voText;

    [Header("Events")]
    [Tooltip("Event triggers when the object is picked up.")]
    public UnityEvent onPickup;
    public UnityEvent onPutDown;

    [Header("Refs")]
    [Tooltip("The position in front of the camera that the object will move to when selected.")]
    Transform camTransform;

    [Header("Debug")]
    public bool updateOffset;

    // Private variables
    AudioSource source;

    Vector3 deskPosition;       // The original position and rotation of the object when on the desk
    Quaternion deskRotation;

    Vector3 camPosition;
    Quaternion camRotation;
    
    bool isMoving;              // Checks if the object is moving between positions. Disables controls when moving.
    bool rotateControlsEnabled;
    bool mouseDown;
    bool mouseOverEnabled;
    float clickTimer;

    string debugPrefix;

    Vector3 prevMousePos;       // Stores the mouse position of the previous frame. Used for rotation controls.

    void Start()
    {
        deskPosition = transform.parent.position;
        deskRotation = transform.parent.rotation;
        camPosition = GameManager.Instance.inspectPos.position + positionOffset;
        camRotation = Quaternion.Euler(GameManager.Instance.inspectPos.rotation.eulerAngles + rotationOffset);

        currentPosition = Position.Desk;
        camTransform=GameManager.Instance.inspectPos;

        source = GetComponent<AudioSource>();
        if(clip) source.clip = clip;

        if(enableOnMouseOver) enableOnMouseOver.SetActive(false);
        mouseOverEnabled = true;

        debugPrefix = "INSPECTABLE("+name+") --- ";
    }

    void Update()
    {
        if(updateOffset){
            camPosition = GameManager.Instance.inspectPos.position + positionOffset;
            camRotation = Quaternion.Euler(GameManager.Instance.inspectPos.rotation.eulerAngles + rotationOffset);
            if(currentPosition == Position.Camera){
                transform.parent.position = camPosition;
                transform.parent.rotation = camRotation;
            }
        }

        if(mouseDown){
            clickTimer += Time.deltaTime;
            if(clickTimer > clickTime && GameManager.Instance.canPickupInspectable){
                rotateControlsEnabled = true;
            }
        }

        if(rotateControlsEnabled){
            Vector3 diff = Input.mousePosition - prevMousePos;      // Get the difference in mouse position on this frame vs the last frame

            if(Input.GetMouseButton(0)){        // If left mouse is held down (adapt this in the future to work with touchscreens)

                float xRot = -diff.y * rotateSpeed * Time.deltaTime;        // Calculate the rotation needed on each axis
                float yRot = diff.x * rotateSpeed * Time.deltaTime;

                transform.parent.Rotate(Camera.main.transform.right, -xRot, Space.World);   // Rotate the parent around the axis of the camera
                transform.parent.Rotate(Camera.main.transform.up, -yRot, Space.World);
            }

            prevMousePos = Input.mousePosition;         // Save the mouse position for use in the next frame
        }
    }

    private void OnMouseEnter(){
        if(enableOnMouseOver && mouseOverEnabled) enableOnMouseOver.SetActive(true);
    }

    private void OnMouseExit(){
        if(enableOnMouseOver && mouseOverEnabled) enableOnMouseOver.SetActive(false);
    }

    private void OnMouseDown() {
        mouseDown = true;
        clickTimer = 0;
    }

    private void OnMouseUp() {
        mouseDown = false;

        Debug.Log(debugPrefix + "Object clicked");

        if(clickTimer < clickTime){
            rotateControlsEnabled = false;
            if(!isMoving){             // Check that we are currently able to pick up objects and that this object isn't moving
                if(GameManager.Instance.canPickupInspectable && currentPosition == Position.Desk) MoveToCamPosition();              // If the object is on the desk, move it to the camera position
                else if(!GameManager.Instance.canPickupInspectable && currentPosition == Position.Camera) MoveToDeskPosition();
            }
        }
    }

    void MoveToCamPosition(){
        Debug.Log(debugPrefix + "Moving to cam pos");

        StartCoroutine(SlerpToPosition(camPosition, camRotation, moveDuration, true));
        currentPosition = Position.Camera; 

        if(source.clip) source.Play();
        onPickup.Invoke();      // Invoke event

        GameManager.Instance.InspectablePickedUp(this);
        GameManager.Instance.SetGameState(GameStates.Inspecting);

        if(enableOnMouseOver) enableOnMouseOver.SetActive(false);
        mouseOverEnabled = false;

    }

    void MoveToDeskPosition(){
        Debug.Log(debugPrefix + "Moving to desk pos");

        rotateControlsEnabled = false;      // Disable rotation controls immediately

        StartCoroutine(SlerpToPosition(deskPosition, deskRotation, moveDuration));  // Animate object to original desk position and rotation
        currentPosition = Position.Desk;

        onPutDown.Invoke();

        Debug.Log("source playing: " + source.isPlaying);
        if(stopAudioOnRelease && source.isPlaying){
            source.Stop();   // Stops the audio if setting enabled and audio is currently playing
            Debug.Log("Stopping");
        } 

        GameManager.Instance.SetGameState(GameStates.Idle);
        GameManager.Instance.InspectableDropped();
        mouseOverEnabled = true;
    }

    // Function to smoothly move the object to a target position and rotation with a duration
    IEnumerator SlerpToPosition(Vector3 targetPos, Quaternion targetRot, float duration, bool enableRotationOnMoveComplete = false){
        float time = 0;
        Vector3 startPos = transform.parent.position;
        Quaternion startRot = transform.parent.rotation;

        isMoving = true;

        while (time < duration)
        {
            transform.parent.position = Vector3.Slerp(startPos, targetPos, time / duration);
            transform.parent.rotation = Quaternion.Slerp(startRot, targetRot, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.parent.position = targetPos;
        transform.parent.rotation = targetRot;

        isMoving = false;
        rotateControlsEnabled = enableRotationOnMoveComplete;       // Enable or disable rotation after move
    }
}

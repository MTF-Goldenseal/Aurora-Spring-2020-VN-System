using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;
using Paratoxic.DialogueManager;

public class GameManager : MonoBehaviour //Manages general game logic, communication between lower level scripts/systems
{
    //Scripts
    public static GameManager gameManager;
	public static EventManager eventManager;
    //public static DialogueManager dialogueManager;
	public static GeneralDialogueManager generalDialogueManager;
	public static PhoneDialogueManager phoneDialogueManager;
	public static TextCommands textCommands;
	public static Data data;

	//Values
	public int controlMode;
	public int previousControlMode;
	public int initialControlMode;
	public bool PlayerInControl { get; set; } = true;
    [HideInInspector]
	public bool playerInChoice = false;
	[HideInInspector]
	public bool playingDialogue = false;
	public string speaker;

    public Queue eventQueue = new Queue();
	public List<System.Object[]> eventQueueParamList = new List<System.Object[]>();

    void Awake(){ //instantiate dialoguemanager script and maintain between scenes
		if (!gameManager){
			gameManager = this;
		}
		else
		{
			Destroy(gameObject);
			return;
		}
		DontDestroyOnLoad(gameObject);
	}
    
    void Start()
    {
		eventManager = GetComponent<EventManager>();
        //dialogueManager = GetComponent<DialogueManager>();
		generalDialogueManager = GetComponent<GeneralDialogueManager>();
		phoneDialogueManager = GetComponent<PhoneDialogueManager>();
		textCommands = GetComponent<TextCommands>();
		data = GetComponent<Data>();
        
        speaker = data.speakerList[0];

		controlMode = initialControlMode; //Scene based configuration

		Resources.LoadAsync(""); //Literally just loads every asset on boot. Probably not the best way to go about it, but sucks for now
    }

    // Update is called once per frame
    void Update()
    {
		if (PlayerInControl == true){
			if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0)){
				InputSelect(controlMode);
			}
			if (/*Input.GetKeyDown(KeyCode.LeftControl) || */Input.GetKeyDown(KeyCode.Mouse1)){ //Skips text playback.
				InputBack(controlMode);
			}
			/* Commented out. History box incomplete atm.
			if (Input.GetKeyDown(KeyCode.C)){ // Open History
				dialogueManager.HistoryBox();
			}
			*/
		}
    }

	#region Input

	// TODO: QTE - Once VFX is done for animation (after QTE), changes back to phone 
	// TODO: QTE - Command in Script - Tells Dialoge Manager/Game Manager to prep extended messages (etiher to a point in the script or a predetermined number set by command

	public void InputSelect(int mode) {
		switch (mode) {
			case 0: //Normal Dialogue
				if (playingDialogue == false) {
					generalDialogueManager.LoadNextLine();
				} else { //Same functionality as left control/right click while dialogue is playing.
					generalDialogueManager.LoadNextLine(displayQuickly: true);
				}
				break;
			case 1: //Phone System
					// TODO: Call Next message from Dialogue Manager for phone system
					// Currently following existing structure to load and prep text through game manager
				phoneDialogueManager.LoadNextLine();
				break;
			case 2:
				//Block input. Do nothing. 
			default:
				Debug.Log("InputSelect failed, controlMode int in GameManager is set to an invalid value: "+mode);
				break;
		}
	}

	public void InputBack(int mode) {
		switch (mode) {
			case 0: //Normal Dialogue
				generalDialogueManager.LoadNextLine(displayQuickly: true);
				break;
			case 1: //Phone System
				// Process input anyways.
				InputSelect(controlMode);
				break;
			default:
				Debug.Log("InputSelect failed, controlMode int in GameManager is set to an invalid value: " + mode);
				break;
		}
	}
	#endregion
}

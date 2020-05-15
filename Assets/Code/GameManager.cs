using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour //Manages general game logic, communication between lower level scripts/systems
{
    //Scripts
    public static GameManager gameManager;
	public static EventManager eventManager;
    public static DialogueManager dialogueManager;
	public static TextCommands textCommands;
	public static Data data;

	//Values
	public int controlMode;
	public int initialControlMode;
	[HideInInspector]
	public bool playerInControl = true;
	[HideInInspector]
	public bool blockInputWhilePrinting = false;
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
        dialogueManager = GetComponent<DialogueManager>();
		textCommands = GetComponent<TextCommands>();
		data = GetComponent<Data>();
        
        speaker = data.speakerList[0];

		controlMode = initialControlMode;

		Resources.LoadAsync(""); //Literally just loads every asset on boot. Probably not the best way to go about it, but sucks for now
    }

    // Update is called once per frame
    void Update()
    {
		if (playerInControl == true){
			if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0)){
				InputSelect(controlMode);
			} else if (/*Input.GetKeyDown(KeyCode.LeftControl) || */Input.GetKeyDown(KeyCode.Mouse1)){ //Skips text playback.
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
				if (playingDialogue == false)
				{
					AdvanceText();
				}
				else
				{ //Same functionality as left control/right click while dialogue is playing.
					if (!blockInputWhilePrinting)
					{
						dialogueManager.DisplayAllText();
					}
				}
				break;
			case 1: //Phone System
				AdvancePhoneText();
				break;
			default:
				Debug.Log("InputSelect failed, controlMode int in GameManager is set to an invalid value: "+mode);
				break;
		}
	}

	public void InputBack(int mode) {
		switch (mode) {
			case 0: //Normal Dialogue
				if (!blockInputWhilePrinting)
				{
					if (dialogueManager.finalText == "")
					{
						PrepText();
					}
					dialogueManager.DisplayAllText();
				}
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

	private string ProcessInitialBrackets(string lineToProcess)
	{
		string initialBracketsPattern = @"((\[.*?\])+\[.*?\])|(^\[.*?\])";
		string bracketSeparationPattern = @"\[.*?\]";

		string capturedBrackets = Regex.Match(lineToProcess, initialBracketsPattern).Groups[0].Value;

		foreach (Match match in Regex.Matches(capturedBrackets, bracketSeparationPattern))
		{
			textCommands.ProcessEvent(match.Value.Substring(1, match.Value.Length - 2), isStartOfLine: true);
		}

		dialogueManager.SetTextIndex(dialogueManager.GetTextIndex() + capturedBrackets.Length);
		return lineToProcess.Substring(capturedBrackets.Length);
	}

	public void PrepText() {
		dialogueManager.LoadNextLine();

		if (controlMode == 1)
		{
			dialogueManager.finalText = ProcessInitialBrackets(dialogueManager.finalText);
		}
		dialogueManager.AddToHistory(dialogueManager.finalText);

		//textCommands.CheckBracket(dialogueManager.GetTextIndex(), true);
		StartCoroutine("ParseQueue");
	}
	public void AdvanceText() {
		PrepText();
		dialogueManager.StartCoroutine("LetterByLetter");
	}

	// Following existing paradigm from AdvanceText
	/* NOTE: consider shifting responsibility of preparing text and such from GameManager to DialogueManager?
		I notice several different links to methods across the dialogueManager and textCommands to 
		handle command inputs and such as well as GameManager
	*/
	public void AdvancePhoneText()
	{
		PrepText();
		dialogueManager.NextPhoneMessage();
	}

    IEnumerator ParseQueue(){ //Runs through start-of-line events
		Debug.Log("Parsing queue...");
		bool shouldReturnControl = false;
		if (playerInControl)
		{
			shouldReturnControl = true;
			playerInControl = false;
		}
        
		int count = 0;
		Type type = typeof(EventManager);
		MethodInfo mi;
        while (eventQueue.Count != 0){
			Debug.Log("Running event: "+eventQueue.Peek());
			if ((string)eventQueue.Peek() == "Delay") {
				IEnumerator delay = eventManager.Delay((float)eventQueueParamList[count][0]);
				yield return StartCoroutine(delay);
			} else {
				mi = type.GetMethod(eventQueue.Dequeue().ToString());
				mi.Invoke(eventManager, eventQueueParamList[count]);
			}
			//eventQueue.Dequeue();
			count++;
        }
		eventQueueParamList.Clear();
		if (shouldReturnControl)
		{
			playerInControl = true;
		}
		yield return null;
    }

	public void SetPlayerControl (bool control) {
		playerInControl = control;
	}
	public bool getPlayerControl () {
		return playerInControl;
	}

	void SetBlockInputWhilePrinting ()
	{

	}

	void SetControlMode (int mode) {

		controlMode = mode;
	}
	int getControlMode () {
		return controlMode;
	}
}

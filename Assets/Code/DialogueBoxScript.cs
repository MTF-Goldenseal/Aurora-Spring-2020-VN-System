using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueBoxScript : MonoBehaviour {

	//Scripts
	public static GameManager gameManager;
	public static DialogueManager dialogueManager;
	public static Data data;

	//GameObjects
	public GameObject dialogueBox;
	public GameObject pointerActive;
	public GameObject pointerLeft;
	public GameObject pointerRight;
	public GameObject pointerBackActive;
	public GameObject pointerBackLeft;
	public GameObject pointerBackRight;
	public GameObject pointerGroup;

	public GameObject boxStatus;
	public GameObject nameplate;

	//Animators
	public Animator boxAnim;
	public Animator pointerAnim;
	public Animator boxBackAnim;
	public Animator pointerBackAnim;
	public Animator pointerBackLeftAnim;
	public Animator pointerBackRightAnim;

	public Animator boxFillAnim;

	public Animator backFillAnim;
	public Animator nameplateAnim;
	public Animator boxStatusAnim;

	private AudioSource boxAudio;

	void Start() {
		gameManager = GameObject.Find("Overlord").GetComponent<GameManager>();
		dialogueManager = GameObject.Find("Overlord").GetComponent<DialogueManager>();
		data = GameObject.Find("Overlord").GetComponent<Data>();

		dialogueBox = this.gameObject;
		pointerLeft = GameObject.Find("BoxPointerLeft");
		pointerRight = GameObject.Find("BoxPointerRight");
		pointerRight.SetActive(false);
		pointerActive = pointerLeft;
		pointerBackLeft = GameObject.Find("PointerBackLeft");
		pointerBackRight = GameObject.Find("PointerBackRight");
		pointerBackLeftAnim = GameObject.Find("PointerBackLeft").GetComponent<Animator>();
		pointerBackRightAnim = GameObject.Find("PointerBackRight").GetComponent<Animator>();
		pointerBackAnim = pointerBackLeftAnim;
		pointerBackRight.SetActive(false);
		pointerBackActive = pointerBackLeft;
		pointerGroup = GameObject.Find("BoxPointerGroup");

		boxStatus = GameObject.Find("BoxStatus");
		nameplate = GameObject.Find("Nameplate");

		boxAnim = GetComponent<Animator>();
		pointerAnim = pointerActive.GetComponent<Animator>();
		boxBackAnim = GameObject.Find("BoxBack").GetComponent<Animator>();
		boxFillAnim = GameObject.Find("BoxFill").GetComponent<Animator>();
		nameplateAnim = nameplate.GetComponent<Animator>();
		boxStatusAnim = boxStatus.GetComponent<Animator>(); 
}
	
	void Update() {
		
	}

    #region Pointer Control
    public void PointerMove(Vector3 location) { //note: this seems to overshoot the final location by a little bit, try converting to Lerping?
        Vector3 dialogueBoxPosition = dialogueBox.GetComponent<Transform>().position;
        //location += dialogueBoxPosition;
		IEnumerator movePointerCoroutine = MovePointerCoroutine(location);
		StartCoroutine(movePointerCoroutine);
		IEnumerator MovePointerCoroutine(Vector3 l) {
			Transform pointerTransform = pointerGroup.transform;
            Debug.Log("Moving from Location: [" + pointerTransform.position + "] to Destination: [" + location + "]");
            //float step;
			while (Vector3.Distance(pointerTransform.localPosition, l) > 0.01f) {
				/*step = Vector3.Distance(pointerTransform.position, l)*0.35f;
				Debug.Log("Stepping a distance of ["+step+"]");
				pointerTransform.position = Vector3.MoveTowards(pointerTransform.position, l, step);
				Debug.Log("Current position: [" + pointerTransform.position + "]");*/
				pointerTransform.localPosition = Vector3.Lerp(pointerTransform.localPosition, l, 0.3f);
				yield return new WaitForSeconds(0.025f);
			}
			pointerTransform.localPosition = l;
			Debug.Log("<color=#ff0000>"+pointerTransform.position+"</color>");
			yield return null;
		}
	}
	public void PointerSet(Vector3 location) {
		pointerGroup.transform.position = location;
	}
	public void PointerFlip() {
		GameObject pFlipBefore;
		GameObject pFlipBeforeBack;
		GameObject pFlipAfter;
		GameObject pFlipAfterBack;
		if (pointerActive == pointerLeft) {
			pFlipBefore = pointerLeft;
			pFlipBeforeBack = pointerBackLeft;
			pFlipAfter = pointerRight;
			pFlipAfterBack = pointerBackRight;
		} else {
			pFlipBefore = pointerRight;
			pFlipBeforeBack = pointerBackRight;
			pFlipAfter = pointerLeft;
			pFlipAfterBack = pointerBackLeft;
		}
		pFlipAfter.SetActive(true);
		pFlipAfterBack.SetActive(true);
		int savedAnimState = pointerAnim.GetInteger("animState");
		bool savedIsTalkingState = pointerAnim.GetBool("isTalking");
		pointerActive = pFlipAfter;
		pointerBackActive = pFlipAfterBack;
		pointerAnim = pFlipAfter.GetComponent<Animator>();
		pointerBackAnim = pFlipAfterBack.GetComponent<Animator>();

		pointerAnim.SetInteger("animState", savedAnimState);
		pointerBackAnim.SetInteger("animState", savedAnimState);
		pointerAnim.SetBool("isTalking", savedIsTalkingState);
		pointerBackAnim.SetBool("isTalking", savedIsTalkingState);

		pFlipBefore.SetActive(false);
		pFlipBeforeBack.SetActive(false);
	}
    #endregion

    #region Animation State Control
    public bool GetDialogueAnim() {
		return this.GetComponent<Animator>().enabled;
	}
	public void EnableDialogueAnim() { //no bool parameter to support animator events in Unity's Animator Window
		Debug.Log("Called EnableDialogueAnim()");
		boxAnim.enabled = true;
		pointerAnim.enabled = true;
		boxBackAnim.enabled = true;
		pointerBackAnim.enabled = true;
		nameplateAnim.enabled = true;
		boxFillAnim.enabled = true;
	}
	public void DisableDialogueAnim() {
		Debug.Log("Called DisableDialogueAnim()");
		boxAnim.enabled = false;
		pointerAnim.enabled = false;
		boxBackAnim.enabled = false;
		pointerBackAnim.enabled = false;
		nameplateAnim.enabled = false;
		boxFillAnim.enabled = false;
	}

	public void SetBoxState(int state) {
		//Debug.Log("Called SetBoxState(" + state + ")");
		boxAnim.SetInteger("animState", state);
		pointerAnim.SetInteger("animState", state);
		boxBackAnim.SetInteger("animState", state);
		pointerBackAnim.SetInteger("animState", state);
		boxFillAnim.SetInteger("animState", state);

		nameplateAnim.SetInteger("animState", state);
	}
	public void SetNameState(int state) {
		nameplateAnim.SetInteger("charID", state);
	}
	public void SetBoxStatusState(int state) {
		boxStatusAnim.SetInteger("animState", state);
	}
	public void SetBoxFill(int state) {
		boxFillAnim.SetInteger("boxFill", state);
	}

	public void SetTalking(bool boolean) {
		//Debug.Log("Called SetTalking(" + boolean + ")");
		boxAnim.SetBool("isTalking", boolean);
		pointerAnim.SetBool("isTalking", boolean);
		boxBackAnim.SetBool("isTalking", boolean);
		pointerBackAnim.SetBool("isTalking", boolean);
		boxFillAnim.SetBool("isTalking", boolean);

		nameplateAnim.SetBool("isTalking", boolean);
	}
	#endregion

	public void NameDistortMag (float strength) {
		nameplate.GetComponent<SpriteRenderer>().material.SetFloat("Magnitude", strength);
	}
	public void NameDistortRate (float rate) {
		nameplate.GetComponent<DistortHelper>().DistortionRate = rate;
	}
}

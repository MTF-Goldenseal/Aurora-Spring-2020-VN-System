using TMPro;
using UnityEngine;

public class PhoneMessage : MonoBehaviour
{
    public float scrollSpeed;
    
    int sender;
    Vector3 offset = new Vector3(0, 0, 0);
    Vector3 startPosition;
    Vector3 destinationPosition;
    float transitionFraction = 0.0f;
    bool transitioning = false;

    public GameObject dialogueTextObject;
    public TextMeshPro dialogueText;
    private GameObject spawn;

    private void Start()
    {
        dialogueTextObject = gameObject.transform.GetChild(0).gameObject;
        dialogueText = dialogueTextObject.GetComponent<TextMeshPro>();
        dialogueText.text = "";
    }

    private void Update()
    {
        if (transitioning)
        {
            if (transform.position != destinationPosition)
            {
                transitionFraction += (1 / scrollSpeed) * Time.deltaTime;
                transform.position = Vector3.Lerp(startPosition, destinationPosition, transitionFraction);
            } 
            else 
            {
                transitioning = false;
            }
        }
    }

    public void Initialize(DialoguePhoneScript phone, int sender, string text)
    {
        this.sender = sender;
        dialogueText.text = text;
        // this.messageUnderlayColor = c;

        // Set Underlay Color 
        //dialogueText.fontSharedMaterial.SetColor("_UnderlayColor", this.messageUnderlayColor);
        // Also differentiate user message Spawn vs Other message spawn so we can have custom prefabs for user messages that have custom widths
        if (this.sender == 0) { // Main Actor 
            dialogueText.fontSharedMaterial = Resources.Load<Material>("Fonts & Materials/Carlito-Regular SDF User");
            this.spawn = phone.messageSpawn; 
        } else {
            dialogueText.fontSharedMaterial = Resources.Load<Material>("Fonts & Materials/Carlito-Regular SDF Other");
        }

        transform.position = spawn.transform.position;
    }

    public void ShiftMessageOffset(Vector3 v)
    {
        this.transitioning = true;
        this.transitionFraction = 0.0f;
        this.startPosition = transform.position;
        this.offset += v;
        this.destinationPosition = spawn.transform.position + offset;
    }
}

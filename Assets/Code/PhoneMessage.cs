using TMPro;
using UnityEngine;
using static Paratoxic.DialogueManager.PhoneDialogueManager;

public class PhoneMessage : MonoBehaviour
{
    public float scrollSpeed;
    
    int sender;
    Vector3 offset = new Vector3(0, 0, 0);
    Vector3 startPosition;
    Vector3 destinationPosition;
    float transitionFraction = 0.0f;
    bool transitioning = false;

    public SpriteRenderer bubbleSprite;
    public GameObject dialogueTextObject;
    public TextMeshPro dialogueText;
    private GameObject spawn;

    private void Start()
    {
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

    public void Initialize(Paratoxic.DialogueManager.PhoneDialogueManager phone, SenderTypes sender, string text)
    {
        dialogueTextObject = gameObject.transform.GetChild(0).gameObject;
        dialogueText = dialogueTextObject.GetComponent<TextMeshPro>();
        dialogueText.text = "";

        Debug.Log(text);
        dialogueText.text = text;
        // this.messageUnderlayColor = c;

        // Set Underlay Color 

        // Also differentiate user message Spawn vs Other message spawn so we can have custom prefabs for user messages that have custom widths
        if (sender == SenderTypes.MAIN) {
            // Some hack shit
            bubbleSprite = transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>();
            transform.GetChild(2).gameObject.SetActive(false);
            dialogueText.alignment = TextAlignmentOptions.TopRight;
            dialogueText.fontSharedMaterial = Resources.Load<Material>("Fonts & Materials/Carlito-Regular SDF User");
            spawn = phone.UserMessageSpawnPoint.gameObject; 
        } else if(sender == SenderTypes.OTHER){
            // Some hack shit
            bubbleSprite = transform.GetChild(2).gameObject.GetComponent<SpriteRenderer>();
            transform.GetChild(1).gameObject.SetActive(false);
            dialogueText.fontSharedMaterial = Resources.Load<Material>("Fonts & Materials/Carlito-Regular SDF Other");
            spawn = phone.OtherMessageSpawnPoint.gameObject;
        }
        else
        {
            Debug.LogError($"I couldn't understand what type of sender {sender} is");
        }

        transform.position = spawn.transform.position;
    }

    public void ShiftMessageOffset(Vector3 v)
    {
        transitioning = true;
        transitionFraction = 0.0f;
        startPosition = transform.position;
        offset += v;
        destinationPosition = spawn.transform.position + offset;
    }
}

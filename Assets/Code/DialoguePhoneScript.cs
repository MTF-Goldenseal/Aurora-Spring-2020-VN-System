using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialoguePhoneScript : MonoBehaviour
{
    //Scripts
    public static GameManager gameManager;
    public static DialogueManager dialogueManager;
    public static Data data;

    //Prefabs
    //public GameObject phoneMessagePrefab;

    //Resources
    public Sprite smallBox;
    public Sprite mediumBox;
    public Sprite largeBox;

    //GameObjects
    public GameObject otherSpawn;
    public GameObject userSpawn;
    public GameObject messageSpawn;
    public List<PhoneMessage> sentMessages = new List<PhoneMessage>();

    //Values
    public float defaultMessageDistance;

    // Start is called before the first frame update
    void Start()
    {
        smallBox = Resources.Load<Sprite>("Art/Phone/TextHalfBox1");
        mediumBox = Resources.Load<Sprite>("Art/Phone/TextHalfBox2");
        largeBox = Resources.Load<Sprite>("Art/Phone/TextHalfBox3");

        userSpawn = gameObject.transform.GetChild(0).gameObject;
        otherSpawn = gameObject.transform.GetChild(1).gameObject;
        messageSpawn = otherSpawn;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateMessage(int sender, string text)
    { 
        GameObject messageObject = Instantiate(Resources.Load<GameObject>("Art/Phone/PhoneTextBox"), gameObject.transform);
        PhoneMessage phoneMessage = messageObject.GetComponent<PhoneMessage>();

        phoneMessage.Initialize(this, sender, text);
        sentMessages.Add(phoneMessage);

        SpriteRenderer messageSprite = phoneMessage.bubbleSprite;

        if (phoneMessage.dialogueText.GetPreferredValues().y > 1.0) // Jank to see if big box needed
        {
            messageSprite.sprite = largeBox;
        } 
        else if (phoneMessage.dialogueText.GetPreferredValues().y > .5) // Jank to see if medium box
        {
            messageSprite.sprite = mediumBox;
        } 
        else // smol box
        {
            messageSprite.sprite = smallBox;
        }

        // add message length and default message distance to offset
        Vector3 offset = new Vector3(0, defaultMessageDistance, 0) + new Vector3(0, phoneMessage.dialogueText.GetPreferredValues().y, 0);

        ShiftMessages(offset);
    }

    public void ShiftMessages(Vector3 offset)
    {
        foreach (PhoneMessage phoneMessage in sentMessages)
        {
            phoneMessage.ShiftMessageOffset(offset);
        }
        // Currently adjust offset for all messages
    }
}

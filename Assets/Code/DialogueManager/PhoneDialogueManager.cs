using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Paratoxic.DialogueManager
{
    public class PhoneDialogueManager : DialogueManager
    {
        public enum SenderTypes
        {
            MAIN,
            OTHER
        }
        public SenderTypes CurrentSender { get; set; }

        [SerializeField]
        private float defaultMessageDistance;
        [SerializeField]
        private GameObject phoneTextBoxPrefab;
        [SerializeField]
        private Transform messagesContainer;
        [SerializeField]
        private bool playSounds;
        [SerializeField]
        private Transform userMessageSpawnPoint;
        public Transform UserMessageSpawnPoint { get => userMessageSpawnPoint; private set => userMessageSpawnPoint = value; }
        [SerializeField]
        private Transform otherMessageSpawnPoint;
        public Transform OtherMessageSpawnPoint { get => otherMessageSpawnPoint; private set => otherMessageSpawnPoint = value; }

        private bool PlaySounds { get; set; }

        private List<PhoneMessage> messageHistory;

        //SEB: These should later be replaced by other sprites if my suggestion is accepted
        [SerializeField]
        private Sprite largeBox;
        [SerializeField]
        private Sprite mediumBox;
        [SerializeField]
        private Sprite smallBox;
        //SEB: These should later be replaced by other sprites if my suggestion is accepted

        // Start is called before the first frame update
        new void Start()
        {
            messageHistory = new List<PhoneMessage>();
            base.Start();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public override void DisplayEntireLineAtOnce(string line)
        {
            GameObject messageObject = Instantiate(phoneTextBoxPrefab, messagesContainer);
            PhoneMessage phoneMessage = messageObject.GetComponent<PhoneMessage>();

            line = ProcessInitialBrackets(line);

            phoneMessage.Initialize(this, CurrentSender, line);
            messageHistory.Add(phoneMessage);

            /*SEB:
             * This should get replaced by a different system. 
             * Here's a suggestion:
             * Have 3 sprites. A top, which contains the arrow part of the bubble (https://i.imgur.com/ndmUZIv.png), a middle, which is just an extension without a defined end on top of bottom (https://i.imgur.com/BhQx2Q9.png), and a bottom, which closes out the box (https://i.imgur.com/3IlPRyv.png). 
             * If it's small enough, sure, use the already existing sprite for the small box, but if it's medium, use the top and bottom, and if it's anything bigger than that, calculate the necessary height and divide it by the height of the medium extension, making it so you can add as many "middle parts" as necessary to accomodate the entire message.
             * Send me a message if it's unclear and I can provide a better example.
             */
            if (phoneMessage.dialogueText.preferredHeight > 1)
            {
                phoneMessage.bubbleSprite.sprite = largeBox;
            }
            else if (phoneMessage.dialogueText.preferredHeight > 0.5f)
            {
                phoneMessage.bubbleSprite.sprite = mediumBox;
            }
            else
            {
                phoneMessage.bubbleSprite.sprite = smallBox;
            }

            // add message length and default message distance to offset
            Vector3 offset = new Vector3(0, defaultMessageDistance, 0) + new Vector3(0, phoneMessage.dialogueText.GetPreferredValues().y, 0);

            ShiftMessages(offset);
            if (playSounds)
            {
                PlayMessageSound(CurrentSender);
            }
        }

        public void ShiftMessages(Vector3 offset)
        {
            foreach (PhoneMessage phoneMessage in messageHistory)
            {
                phoneMessage.ShiftMessageOffset(offset);
            }
            // Currently adjust offset for all messages
        }

        public void PlayMessageSound(SenderTypes sender)
        {
            if (sender == SenderTypes.MAIN)
            {
                GameManager.eventManager.SFX("MessagePing1");
            }
            else
            {
                GameManager.eventManager.SFX("MessagePing2");
            }
        }

        protected override void DisplayLine(string line)
        {
            DisplayEntireLineAtOnce(line);
        }

        protected override void ResetVariables()
        {
            throw new System.NotImplementedException();
        }
    }
}

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
        private GameObject phoneTextBoxPrefab;
        [SerializeField]
        private Transform messagesContainer;
        [SerializeField]
        private bool playSounds;
        private bool PlaySounds { get; set; }

        protected override void DisplayEntireLineAtOnce(string line)
        {
            GameObject messageObject = Instantiate(phoneTextBoxPrefab, messagesContainer);
            PhoneMessage phoneMessage = messageObject.GetComponent<PhoneMessage>();

            phoneMessage.Initialize(this, CurrentSender, line);
            sentMessages.Add(phoneMessage);

            SpriteRenderer messageSprite = phoneMessage.bubbleSprite;

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
            foreach (PhoneMessage phoneMessage in sentMessages)
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

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}

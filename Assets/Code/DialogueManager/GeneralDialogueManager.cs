using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Paratoxic.DialogueManager
{
    public class GeneralDialogueManager : DialogueManager
    {
        [SerializeField]
        private AudioSource charSoundBite;
        [SerializeField]
        private DialogueBoxScript dialogueBox;
        [SerializeField]
        private GameObject dialogueTextObject;
        [SerializeField]
        private bool isPlayingDialogue;
        public bool IsPlayingDialogue { get { return isPlayingDialogue; } private set { isPlayingDialogue = value; } }

        private List<char> specialChars = new List<char> { ' ', '[', ']', '<', '>', '=' };
        private TextMeshPro dialogueText;

        // Start is called before the first frame update
        void Start()
        {
            dialogueText = dialogueTextObject.GetComponent<TextMeshPro>();
            dialogueText.text = "";
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        protected override void DisplayLine(string line)
        {        
            StartCoroutine(WriteTextOut(line));
        }

        private IEnumerator WriteTextOut(string line)
        {
            isPlayingDialogue = true;
            dialogueBox.SetTalking(true);

            string parsedLine = ProcessInitialBrackets(line);

            for (int i = 0; i < parsedLine.Length; i++)
            {
                if (IsItABracket(parsedLine[i]))
                {
                    //Warning: It's possible that altering the string like this mid-loop will cause "Out of bounds" funkiness because the length is variable. Keep an eye out for that.
                    parsedLine = ProcessSingleBracket(parsedLine);
                }
                if(IsDelaying)
                {
                    yield return new WaitForSeconds(secondsOfDelayLeft);
                    IsDelaying = false;
                    secondsOfDelayLeft = 0f;
                }
                //Warning: It's possible that writing <tags> character by character won't trigger the intended effects. Keep an eye out for that.
                PlayCharSoundBite(parsedLine[i]);
                dialogueText.text += parsedLine[i];
                yield return null;
            }

            ResetVariables();
        }

        private bool IsItABracket(char character)
        {
            return character == '[';
        }

        void PlayCharSoundBite(char character)
        {
            if (specialChars.Contains(character))
            {
                return;
            }
            if (charSoundBite.isPlaying == true)
            {
                charSoundBite.Stop();
            }
            charSoundBite.Play();
        }

        protected override void ResetVariables()
        {
            dialogueBox.SetTalking(false);
            IsPlayingDialogue = false;

            if (autoNext == true && lineIndex < sceneScript.Count)
            {
                AutoAdvance();
            }
        }
    }
}

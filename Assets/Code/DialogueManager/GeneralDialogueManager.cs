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
        public bool IsPlayingDialogue { get => isPlayingDialogue; private set => isPlayingDialogue = value; }
        [SerializeField]
        private float delayBetweenEachLetter = 0.04f;
        public float DelayBetweenEachLetter { get => delayBetweenEachLetter; set => delayBetweenEachLetter = value; }

        private List<char> specialChars = new List<char> { ' ', '[', ']', '<', '>', '=' };
        private TextMeshPro dialogueText;
        private Coroutine WrittingTextOut;

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

        public override void DisplayEntireLineAtOnce(string line)
        {
            IsPlayingDialogue = false;
            if (WrittingTextOut != null)
            {
                StopCoroutine(WrittingTextOut);
            }
            PlayCharSoundBite();
            for (int i = 0; i < line.Length; i++)
            {
                if (IsItABracket(line[i]))
                {
                    //Warning: It's possible that altering the string like this mid-loop will cause "Out of bounds" funkiness because the length is variable. Keep an eye out for that.
                    line = ProcessSingleBracket(line);
                }
                if (IsDelaying)
                {
                    IsDelaying = false;
                    secondsOfDelayLeft = 0f;
                }
                //Warning: It's possible that writing <tags> character by character won't trigger the intended effects. Keep an eye out for that.
                dialogueText.text += line[i];
            }
        }

        protected override void DisplayLine(string line)
        {        
            WrittingTextOut = StartCoroutine(WriteTextOut(line));
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
                yield return new WaitForSeconds(delayBetweenEachLetter);
            }

            ResetVariables();
            WrittingTextOut = null;
        }

        private bool IsItABracket(char character)
        {
            return character == '[';
        }

        void PlayCharSoundBite(char character = 'a')
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

            if (IsAudoAdvancing && CurrentLineNumber < numOfBytesToOffsetToSpecificLine.Count)
            {
                AutoAdvance();
            }
        }

        private void AutoAdvance()
        {
            Timer.Register(SecondsBetweenAutoAdvancedMessages, () => LoadNextLine());
        }

        
    }
}

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
        private float delayBetweenEachLetter = 0.04f;
        public float DelayBetweenEachLetter { get => delayBetweenEachLetter; set => delayBetweenEachLetter = value; }

        private List<char> specialChars = new List<char> { ' ', '[', ']', '<', '>', '=' };
        private TextMeshPro dialogueText;
        private Coroutine WrittingTextOut;

        // Start is called before the first frame update
        new void Start()
        {
            dialogueText = dialogueTextObject.GetComponent<TextMeshPro>();
            dialogueText.text = "";
            //Since this is a class that inherits from another one, it's best that you always call the start function on the base one, as you may want to initialise things that should only be referenced in the base class. Were this something other than Unity, you'd just call the constructor instead.
            base.Start();
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
                    ProcessSingleBracket(line, ref i);
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
            IsPlayingDialogue = true;
            dialogueBox.SetTalking(true);
            dialogueText.text = "";

            string lineWithInitialParsing = ProcessInitialBrackets(line);

            for (int i = 0; i < lineWithInitialParsing.Length; i++)
            {
                if (IsItABracket(lineWithInitialParsing[i]))
                {
                    ProcessSingleBracket(lineWithInitialParsing, ref i);
                }
                if(IsDelaying)
                {
                    yield return new WaitForSeconds(secondsOfDelayLeft);
                    IsDelaying = false;
                    secondsOfDelayLeft = 0f;
                }
                //Warning: It's possible that writing <tags> character by character won't trigger the intended effects. Keep an eye out for that.
                PlayCharSoundBite(lineWithInitialParsing[i]);
                dialogueText.text += lineWithInitialParsing[i];
                yield return new WaitForSeconds(delayBetweenEachLetter);
            }

            ResetVariables();
            WrittingTextOut = null;
        }

        private bool IsItABracket(char character)
        {
            return character == '[';
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character">This assumes the character 'a' in the event that it only needs to be played once, outside of a loop where you'd have access to the characters</param>
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

            if (IsAudoAdvancing && CurrentLineNumber < loadedScript.Count)
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

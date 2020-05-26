using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Paratoxic.DialogueManager
{
    public abstract class DialogueManager : MonoBehaviour
    {
        public static StreamReader scriptReader;
        private TextCommands textCommands;

        protected static List<string> loadedScript = new List<string> ();
        protected static int CurrentLineNumber { get; set; } = 0;

        public bool IsDelaying { get; protected set; }
        protected float secondsOfDelayLeft = 0f;
        protected bool IsAudoAdvancing { get; set; } = false;

        [SerializeField]
        private float secondsBetweenAutoAdvancedMessages;
        protected float SecondsBetweenAutoAdvancedMessages { get { return secondsBetweenAutoAdvancedMessages; } set { secondsBetweenAutoAdvancedMessages = value; } }

        [SerializeField]
        private bool isPlayingDialogue;
        public bool IsPlayingDialogue { get => isPlayingDialogue; protected set => isPlayingDialogue = value; }

        [SerializeField]
        private bool isBlockingInput;
        public bool IsBlockingInput { get => isBlockingInput; set => isBlockingInput = value; }

        // Start is called before the first frame update
        protected void Start()
        {
            textCommands = GameObject.Find("Overlord").GetComponent<TextCommands>();
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public static void LoadScript(string fileName)
        {
            scriptReader = new StreamReader($"StoryScripts\\{fileName}.txt");
            string line;
            do
            {
                line = scriptReader.ReadLine().Replace("\\n", "\n");
                loadedScript.Add(line);
                
            } while (scriptReader.Peek() >= 0);
            scriptReader.Close();

        }

        public void LoadNextLine(bool displayQuickly = false)
        {
            if(!isBlockingInput && CurrentLineNumber < loadedScript.Count)
            {
                LoadLine(displayQuickly);
                CurrentLineNumber++;
            }
        }

        public void JumpToLine(int lineNumber, bool displayQuickly = false)
        {
            if(lineNumber < loadedScript.Count)
            {
                CurrentLineNumber = lineNumber;
                LoadLine(displayQuickly);
            }
        }

        private void LoadLine(bool displayQuickly = false)
        {
            string line = loadedScript[CurrentLineNumber];
            Debug.Log($"{line}");
            if(displayQuickly)
            {
                DisplayEntireLineAtOnce(line);
            }
            else
            {
                DisplayLine(line);
            }
        }

        protected abstract void DisplayLine(string line);

        protected string ProcessInitialBrackets(string line)
        {
            string initialBracketsPattern = @"((\[.*?\])+\[.*?\])|((?!\<.*\>)\[.*?\])";
            string bracketSeparationPattern = @"\[.*?\]";

            string capturedBrackets = Regex.Match(line, initialBracketsPattern).Groups[0].Value;

            foreach (Match match in Regex.Matches(capturedBrackets, bracketSeparationPattern))
            {
                textCommands.ProcessEvent(match.Value.Substring(1, match.Value.Length - 2), isStartOfLine: true);
            }

            StartCoroutine(textCommands.ParseEventQueue());

            return line.Substring(capturedBrackets.Length);
        }

        protected void ProcessSingleBracket(string lineToProcess, ref int currentIndexInLine)
        {
            string bracketSeparationPattern = @"\[.*?\]";
            Match match = Regex.Match(lineToProcess, bracketSeparationPattern);
            string bracket = match.Value;
            currentIndexInLine += bracket.Length;
            textCommands.ProcessEvent(bracket.Substring(1, bracket.Length - 2), isStartOfLine: false);
        }

        public void DelayTextForSeconds(float secondsToDelay)
        {
            IsDelaying = true;
            secondsOfDelayLeft = secondsToDelay;
        }

        protected abstract void ResetVariables();

        public abstract void DisplayEntireLineAtOnce(string line);
    }
}

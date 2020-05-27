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

        //These two are left static so that you can flip between dialogue modes without having to have multiple script loads and are able to keep track of where you're at in it
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

        /// <summary>
        /// Loads a script assuming the directory of a folder named "StoryScripts" at the very base of the project
        /// </summary>
        /// <param name="fileName">The name of the file without the extension</param>
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

        /// <summary>
        /// Load the next line of the script unless you're blocking input
        /// </summary>
        /// <param name="displayQuickly">Boolean to determine whether you load the entire line immediately or let it play out letter by letter</param>
        public void LoadNextLine(bool displayQuickly = false)
        {
            if(!isBlockingInput && CurrentLineNumber < loadedScript.Count)
            {
                LoadLine(displayQuickly);
                CurrentLineNumber++;
            }
        }

        /// <summary>
        /// Jumps to a specific line in the script. This does not care about whether the input is being blocked
        /// </summary>
        /// <param name="lineNumber"></param>
        /// <param name="displayQuickly">Boolean to determine whether you load the entire line immediately or let it play out letter by letter</param>
        public void JumpToLine(int lineNumber, bool displayQuickly = false)
        {
            if(lineNumber < loadedScript.Count)
            {
                CurrentLineNumber = lineNumber;
                LoadLine(displayQuickly);
            }
        }

        /// <summary>
        /// Prepares the line for display
        /// </summary>
        /// <param name="displayQuickly">Boolean to determine whether you load the entire line immediately or let it play out letter by letter</param>
        private void LoadLine(bool displayQuickly = false)
        {
            string line = loadedScript[CurrentLineNumber];
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

        /// <summary>
        /// Taking a line, it will find and process all the commands encased in square brackets, cycle through them and run the events
        /// </summary>
        /// <param name="line">The line that needs to be processed</param>
        /// <returns>The same line, but without all the brackets at the start</returns>
        protected string ProcessInitialBrackets(string line)
        {
            //SEB: https://regexr.com
            //Regex is magic of the old gods. Use that website whenever you want to figure it out. The references and information on the side are immensely helpful. If you want an exaplanation on what these unreadable strings do, paste them onto the website and it'll break it down for you below
            
            //This one gets *all* the brackets into one block.
            string initialBracketsPattern = @"((\[.*?\])+\[.*?\])|((?!\<.*\>)\[.*?\])";
            //This separates those brackets one by one
            string bracketSeparationPattern = @"\[.*?\]";

            string capturedBrackets = Regex.Match(line, initialBracketsPattern).Groups[0].Value;

            foreach (Match match in Regex.Matches(capturedBrackets, bracketSeparationPattern))
            {
                textCommands.ProcessEvent(match.Value.Substring(1, match.Value.Length - 2), isStartOfLine: true);
            }

            StartCoroutine(textCommands.ParseEventQueue());

            return line.Substring(capturedBrackets.Length);
        }

        /// <summary>
        /// Process a single, loose bracket-enclosed event. This function assumes that they're in the middle of the line, as all the ones at the start were already processed. After that, it will skip
        /// </summary>
        /// <param name="lineToProcess"></param>
        /// <param name="currentIndexInLine">The current index of where the reader is at on the line. It is passed by reference so it can be incremented by the number of characters the bracket found has</param>
        protected void ProcessSingleBracket(string lineToProcess, ref int currentIndexInLine)
        {
            string bracketSeparationPattern = @"\[.*?\]";
            //Funny thing about this. If the line has two sets of brackets, because I'm using "Match" and not "Matches", like above, it will only return the first match it finds.
            //Also, the "- 1" is purely for safety because I never know whether the index they ask for includes itself, or if it starts one ahead of it.
            Match match = Regex.Match(lineToProcess.Substring(currentIndexInLine - 1), bracketSeparationPattern);
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

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
        public static StreamReader script;
        private TextCommands textCommands;

        protected static List<long> numOfBytesToOffsetToSpecificLine = new List<long>();
        protected int CurrentLineNumber { get; set; } = 0;
        public bool IsDelaying { get; protected set; }
        protected float secondsOfDelayLeft = 0f;
        protected bool IsAudoAdvancing { get; set; } = false;
        [SerializeField]
        private float secondsBetweenAutoAdvancedMessages;
        protected float SecondsBetweenAutoAdvancedMessages { get { return secondsBetweenAutoAdvancedMessages; } set { secondsBetweenAutoAdvancedMessages = value; } }

        // Start is called before the first frame update
        void Start()
        {
            textCommands = GameObject.Find("Overlord").GetComponent<TextCommands>();
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public static void LoadScript(string fileName)
        {
            script = new StreamReader($"StoryScripts\\{fileName}.txt");
            string line;
            int lineNumber = 0;
            do
            {
                line = script.ReadLine().Replace("\\n", "\n");
                AddByteCountToList(line, lineNumber);
                lineNumber++;
                
            } while (script.Peek() >= 0);
            script.BaseStream.Position = 0;
        }

        private static void AddByteCountToList(string line, int lineCount)
        {
            if (lineCount == 0)
            {
                numOfBytesToOffsetToSpecificLine.Add(System.Text.Encoding.Unicode.GetByteCount(line));
            }
            else
            {
                numOfBytesToOffsetToSpecificLine.Add(numOfBytesToOffsetToSpecificLine[lineCount - 1] + System.Text.Encoding.Unicode.GetByteCount(line));
            }
        }

        public void LoadNextLine(bool displayQuickly = false)
        {
            CurrentLineNumber++;
            LoadLine(displayQuickly);
        }

        public void JumpToLine(int lineNumber, bool displayQuickly = false)
        {
            CurrentLineNumber = lineNumber;
            LoadLine(displayQuickly);
        }

        private void LoadLine(bool displayQuickly = false)
        {
            script.BaseStream.Position = numOfBytesToOffsetToSpecificLine[CurrentLineNumber];
            Debug.Log($"Reading line number {CurrentLineNumber}");
            string line = script.ReadLine();
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

        protected string ProcessSingleBracket(string lineToProcess)
        {
            string bracketSeparationPattern = @"\[.*?\]";
            Match match = Regex.Match(lineToProcess, bracketSeparationPattern);
            string bracket = match.Value;
            textCommands.ProcessEvent(bracket.Substring(1, bracket.Length - 2), isStartOfLine: false);

            return lineToProcess.Remove(match.Index, bracket.Length);
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

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Paratoxic.DialogueManager
{
    public class DialogueManager : MonoBehaviour
    {
        private StreamReader script;
        private TextCommands textCommands;

        private List<long> numOfBytesToOffsetToSpecificLine;
        private int currentLineNumber = 0;

        // Start is called before the first frame update
        void Start()
        {
            numOfBytesToOffsetToSpecificLine = new List<long>();
            textCommands = GameObject.Find("Overlord").GetComponent<TextCommands>();
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void LoadScript(string fileName)
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
        }

        private void AddByteCountToList(string line, int lineCount)
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

        public void LoadNextLine()
        {
            currentLineNumber++;
            LoadLine();
        }

        public void JumpToLine(int lineNumber)
        {
            currentLineNumber = lineNumber;
            LoadLine();
        }

        private void LoadLine()
        {
            script.BaseStream.Position = numOfBytesToOffsetToSpecificLine[currentLineNumber];
            Debug.Log($"Reading line number {currentLineNumber}");
            string line = script.ReadLine();
            DisplayLine($"<color=\"black\">{line}");
        }

        private void DisplayLine(string line)
        {
            PlayVoiceClip();
            StartCoroutine(WriteTextOut(line));
        }

        private IEnumerator WriteTextOut(string line)
        {
            string parsedLine = ProcessInitialBrackets(line);

            for(int i = 0; i < parsedLine.Length; i++)
            {

            }
        }

        private string ProcessInitialBrackets(string line)
        {
            string initialBracketsPattern = @"((\[.*?\])+\[.*?\])|((?!\<.*\>)\[.*?\])";
            string bracketSeparationPattern = @"\[.*?\]";

            string capturedBrackets = Regex.Match(line, initialBracketsPattern).Groups[0].Value;

            foreach (Match match in Regex.Matches(capturedBrackets, bracketSeparationPattern))
            {
                textCommands.ProcessEvent(match.Value.Substring(1, match.Value.Length - 2), isStartOfLine: true);
            }

            string initialTags =  line.LastIndexOf('>');

            return line.Substring() + line.Substring()
        }
    }
}

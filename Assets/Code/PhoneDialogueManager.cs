using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class PhoneDialogueManager : MonoBehaviour
{
    private TextCommands textCommands;
    private GameManager gameManager;

    //Text Elements
    public GameObject nameTextObject;
    public GameObject dialogueTextObject;
    public GameObject historyTextObject;
    public TextMeshPro dialogueText;
    public TextMeshProUGUI historyText;
    public RectTransform historyTextContainer;
    private List<string> sceneScript = new List<string>();

    private AudioSource voiceSource;
    private AudioSource musicSource;
    private int currentLineIndex;
    private int currentCharInLineIndex;
    private string lineToDisplay;
    private List<char> specialChars = new List<char> { ' ', '[', ']', '<', '>', '=' };

    // Start is called before the first frame update
    void Start()
    {
        textCommands = GetComponent<TextCommands>();
        gameManager = GetComponent<GameManager>();

        nameTextObject = GameObject.FindWithTag("NameText");
        dialogueTextObject = GameObject.FindWithTag("DialogueText");
        historyTextObject = GameObject.FindWithTag("HistoryText");
        dialogueText = dialogueTextObject.GetComponent<TextMeshPro>();
        historyText = historyTextObject.GetComponent<TextMeshProUGUI>();
        historyTextContainer = historyTextObject.GetComponent<RectTransform>();

        historyTextObject.SetActive(false);

        dialogueText.text = "";
        historyText.text = "";

        LoadScript("testScript");
    }

    public void LoadScript(string fileName)
    {
        StreamReader fileReader = new StreamReader($"StoryScripts\\{fileName}.txt");
        string line = "";
        do
        {
            line = fileReader.ReadLine().Replace("\\n", "\n");

            sceneScript.Add(line);
            
        } while (fileReader.Peek() >= 0);
    }

    public void LoadNextLine()
    {
        Debug.Log($"Reading next line. #{currentLineIndex}");
        lineToDisplay = "<color=\"black\">" + sceneScript[currentLineIndex];
        currentLineIndex++;
    }

    public void JumpToLine(int lineNumber)
    {
        Debug.Log($"Jumping to line #{lineNumber}");
        lineToDisplay = "<color=\"black\">" + sceneScript[lineNumber - 1];
        currentLineIndex = lineNumber - 1;
    }

    public void DisplayAllText()
    {
        gameManager.playingDialogue = false;

        PlayVoiceClip();
        StartCoroutine(WriteTextOut());
        //ResetTextVariables();
    }

    private void PlayVoiceClip()
    {
        if(specialChars.Contains(lineToDisplay[currentCharInLineIndex]))
        {
            return;
        }

        if(voiceSource.isPlaying)
        {
            voiceSource.Stop();
        }
        voiceSource.Play();
    }

    private IEnumerator WriteTextOut()
    {
        string parsedLine = ProcessInitialBrackets(lineToDisplay);

        for(int i = 0; i < parsedLine.Length; i++)
        {
            if(IsItABracket(parsedLine[i]))
            {
                parsedLine = ProcessSingleBracket(parsedLine);
            }
        }
        foreach (char letter in parsedLine)
        {

            yield return null;
        }
    }

    private string ProcessInitialBrackets(string lineToProcess)
    {
        string initialBracketsPattern = @"((\[.*?\])+\[.*?\])|(^\[.*?\])";
        string bracketSeparationPattern = @"\[.*?\]";

        string capturedBrackets = Regex.Match(lineToProcess, initialBracketsPattern).Groups[0].Value;

        foreach (Match match in Regex.Matches(capturedBrackets, bracketSeparationPattern))
        {
            textCommands.ProcessEvent(match.Value.Substring(1, match.Value.Length - 2), isStartOfLine: true);
        }

        return lineToProcess.Substring(capturedBrackets.Length);
    }

    private bool IsItABracket(char letter)
    {
        return letter == '[';
    }

    private string ProcessSingleBracket(string lineToProcess)
    {
        string bracketSeparationPattern = @"[.*?]";
        Match match = Regex.Match(lineToProcess, bracketSeparationPattern);
        string bracket = match.Value;
        textCommands.ProcessEvent(bracket, isStartOfLine: false);

        return lineToProcess.Remove(match.Index, bracket.Length);
    }

    private void CheckNextChar()
    {
        if(lineToDisplay[currentCharInLineIndex] == '[')
        {
            textCommands.CheckBracket(currentCharInLineIndex, isStartOfLine: false);
        }
        else
        {

        }
    }

    private void CheckBracket()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

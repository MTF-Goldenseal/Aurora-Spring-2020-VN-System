using Paratoxic.DialogueManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockInputUntilTextIsWrittenOut : MonoBehaviour
{
    GeneralDialogueManager generalDialogueManager;

    // Start is called before the first frame update
    void Start()
    {
        generalDialogueManager = GameObject.Find("Overlord").GetComponent<GeneralDialogueManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(generalDialogueManager.IsPlayingDialogue != generalDialogueManager.IsBlockingInput)
        {
            generalDialogueManager.IsBlockingInput = generalDialogueManager.IsPlayingDialogue;
        }
    }
}

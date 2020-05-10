using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockInputUntilTextIsWrittenOut : MonoBehaviour
{
    private GameManager gameManager;
    private EventManager eventManager;
    private Transform transformToObserve;
    private int previousChildCount;
    private int previousControlModeId;
    private bool jankFlag;

    // Start is called before the first frame update
    void Start()
    {
        eventManager = GameObject.Find("Overlord").GetComponent<EventManager>();
        gameManager = GameObject.Find("Overlord").GetComponent<GameManager>();
        previousControlModeId = gameManager.controlMode;
        //transformToObserve = GameObject.Find("Phone").transform;
        //previousChildCount = transformToObserve.childCount;
    }

    // Update is called once per frame
    void Update()
    {
        if(gameManager.playingDialogue)
        {
            gameManager.controlMode = 2;
            jankFlag = true;
        }
        else
        {
            if(jankFlag)
            {
                eventManager.ControlMode(gameManager.previousControlMode);
                jankFlag = false;
            }
            
        }
        //if(transformToObserve.childCount > previousChildCount)
        //{
        //    eventManager.ControlMode(2);
        //    if(!gameManager.playingDialogue)
        //    {
        //        eventManager.ControlMode(previousControlModeId);
        //        previousChildCount = transformToObserve.childCount;
        //    }
        //}
    }
}

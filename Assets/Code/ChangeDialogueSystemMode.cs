using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeDialogueSystemMode : MonoBehaviour
{
    [SerializeField]
    private int controlModeId;
    private int previousControlModeId;
    private EventManager eventManager;
    private GameManager gameManager;

    private void OnDestroy()
    {
        eventManager.ControlMode(previousControlModeId);
    }

    // Start is called before the first frame update
    void Start()
    {
        eventManager = GameObject.Find("Overlord").GetComponent<EventManager>();
        gameManager = GameObject.Find("Overlord").GetComponent<GameManager>();
        previousControlModeId = gameManager.controlMode;
        eventManager.ControlMode(controlModeId);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXScriptContentWarning : VFXScript
{
    private GameManager gameManager;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        gameManager = GameObject.Find("Overlord").GetComponent<GameManager>();
        gameManager.SetPlayerControl(false);
    }

    // Animation event called at end of Content Warning timer
    public override void DestroySelf()
    {
        gameManager.SetPlayerControl(true);
        base.DestroySelf();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXScriptDream : VFXScript {
    // Start is called before the first frame update

    public void DispProb(float level) {
        this.GetComponent<SpriteRenderer>().material.SetFloat("_DispProbability", level);
    }

    public void ColorProb(float level) {
        this.GetComponent<SpriteRenderer>().material.SetFloat("_ColorProbability", level);
    }

    public void GlitchProb(float level) {
        this.GetComponent<SpriteRenderer>().material.SetFloat("_DispProbability", level);
        this.GetComponent<SpriteRenderer>().material.SetFloat("_ColorProbability", level);
    }

    // animation event called at the end of the Dream Sequence animation during fade
    public void ControlModeToPhone() {
        eventManager.ControlMode(1);
    }
}

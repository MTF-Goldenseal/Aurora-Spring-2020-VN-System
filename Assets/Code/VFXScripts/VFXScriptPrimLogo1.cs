using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXScriptPrimLogo1 : VFXScript
{
    public void DispProb (float level) {
        this.GetComponent<SpriteRenderer>().material.SetFloat("_DispProbability", level);
    }

    public void ColorProb (float level) {
        this.GetComponent<SpriteRenderer>().material.SetFloat("_ColorProbability", level);
    }

    public void GlitchProb (float level) {
        this.GetComponent<SpriteRenderer>().material.SetFloat("_DispProbability", level);
        this.GetComponent<SpriteRenderer>().material.SetFloat("_ColorProbability", level);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXScriptDream : VFXScript {
    // Start is called before the first frame update
    private AudioSource audioPlayer;

    void Start()
    {
        audioPlayer = this.GetComponent<AudioSource>();
    }
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

    public void ControlModeToPhone() {
        eventManager.ControlMode(1);
    }

    public void PlaySound() {
        if (audioPlayer.isPlaying == false)
            audioPlayer.Play();
    }
    public void StopSound() {
        audioPlayer.Stop();
    }
}

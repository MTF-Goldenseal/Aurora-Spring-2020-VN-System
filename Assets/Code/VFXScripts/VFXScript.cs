using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXScript : MonoBehaviour
{
    private Animator animController;
    public static EventManager eventManager;
    private int VFXInstanceIndex;
    private AudioSource audioPlayer;

    protected virtual void Start() {
        animController = this.GetComponent<Animator>();
        animController.SetBool("animExit", false);
        eventManager = GameObject.Find("Overlord").GetComponent<EventManager>();
        audioPlayer = this.GetComponent<AudioSource>();
    }

    public void SetIndex(int i) {
        VFXInstanceIndex = i;
    }

    public virtual void DestroySelf() {
        eventManager.RemoveVFX(VFXInstanceIndex);
    }
    public void PlaySound() {
        if (audioPlayer.isPlaying == false)
            audioPlayer.Play();
    }
    public void PlaySoundOnce(string name) {
        if (audioPlayer.isPlaying == false)
            audioPlayer.PlayOneShot(Resources.Load<AudioClip>("Sound/SFX/" + name));
    }
    public void StopSound() {
        audioPlayer.Stop();
    }
}

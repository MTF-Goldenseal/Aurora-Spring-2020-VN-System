using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharScript : MonoBehaviour
{
    private Animator animController;
    public static EventManager eventManager;
    public static DistortHelper distortHelper;
    private int CharInstanceIndex;

    void Start() {
        animController = this.GetComponent<Animator>();
        eventManager = GameObject.Find("Overlord").GetComponent<EventManager>();
        distortHelper = this.GetComponent<DistortHelper>();
    }

    public void SetIndex(int index) {
        CharInstanceIndex = index;
    }
    public void SetAnimInt(int index) {
        animController.SetInteger("animState", index);
    }
    public void DisableAnimator() {
        animController.enabled = false;
    }
    public void DestroySelf() {
        eventManager.RemoveChar(CharInstanceIndex);
    }

    #region Distortion Shader
    public void CharDistortMag(float strength) {
        this.GetComponent<SpriteRenderer>().material.SetFloat("Magnitude", strength);
    }
    public void CharDistortRate(float time) {
        distortHelper.DistortionRate = time;
    }
    #endregion
}

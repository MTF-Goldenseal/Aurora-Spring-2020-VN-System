using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXScriptMorgan : VFXScript 
{
    private int index = 1;
    private GameObject phone;
    private Animator phoneAnim;

    protected override void Start() {
        base.Start();
        phone = GameObject.Find("Phone");
        phoneAnim = phone.GetComponent<Animator>();
    }

    public void SetBG() {
        eventManager.BG("SceneEndBackground");
    }

    public void SpawnVFX(string name) {
        eventManager.SpawnVFX(name);
    }

    public void SetVFXIndex(int i) {
        index = i;
    }
    public void SetVFXState(int state){
        eventManager.SetVFX(index, state);
    }
    public void PhoneDown() {
        phoneAnim.SetInteger("animState", 1);
    }
    public void DisablePhone() {
        phone.SetActive(false);
    }

    public void SetSortingOrder() {
        this.GetComponent<SpriteRenderer>().sortingLayerName = "DialogueVFX";
    }

    public void EndGame() {
        eventManager.ForceQuit();
    }
}

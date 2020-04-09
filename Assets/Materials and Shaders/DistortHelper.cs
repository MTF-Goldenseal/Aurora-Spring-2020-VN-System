using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistortHelper : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public float DistortionRate;
    private float timer;
    private float distortTimesTime;
    private float originalDistortionSpeed;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        timer = DistortionRate;
        originalDistortionSpeed = spriteRenderer.material.GetFloat("_DistortionSpeed");
    }

    // Update is called once per frame
    void Update()
    {
        if (timer <= 0) {
            timer = DistortionRate;
            distortTimesTime = originalDistortionSpeed * (Time.time / 20f) * Random.Range(1, 69);
            spriteRenderer.material.SetFloat("_DistortionSpeed", 0f);
        } else {
            spriteRenderer.material.SetFloat("distortTimesTime", distortTimesTime);
            timer -= Time.deltaTime;
        }
    }

    public void SetDistortionMagnitude(float magnitude) {
        spriteRenderer.material.SetFloat("_Magnitude", magnitude);
    }

    //So if you want it to update every 4/24ths of a second, put in 4/24 into this function, make sure
    //that the 4 and the 24 are floats!!!! so literally write 4f/24f
    public void SetUpdateTime(float timeInSeconds) {
        DistortionRate = timeInSeconds;
    }
}

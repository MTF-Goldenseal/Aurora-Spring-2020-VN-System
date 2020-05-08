using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class DreamStabTimer : MonoBehaviour
{
    [SerializeField]
    private float timeToPressInput = 10f;
    private Timer timerToPressInput;
    private EventManager eventManager;

    private void StabThroat()
    {
        eventManager.SetVFX(1, 8);
    }

    private void OnEnable()
    {
        Debug.Log("Timer Started");
        eventManager = GameObject.Find("Overlord").GetComponent<EventManager>();
        timerToPressInput = Timer.Register(timeToPressInput, () => StabThroat());
    }

    // Update is called once per frame
    void Update()
    {
        if (enabled)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown((int)MouseButton.LeftMouse))
            {
                Debug.Log("Cancelled the timer");
                timerToPressInput.Cancel();
                StabThroat();
            }
        }
    }
}

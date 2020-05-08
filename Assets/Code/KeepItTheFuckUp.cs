using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepItTheFuckUp : MonoBehaviour
{
    [SerializeField]
    private Transform transform;
    [SerializeField]
    private int ZLevel = 100;
    private GameObject phone;


    private void OnDestroy() {
        phone.SetActive(true);
    }
    // Start is called before the first frame update
    void Start()
    {
        transform = GameObject.Find("DialogueBox").transform;
        GameObject.Find("BoxPointerGroup").SetActive(false);
        phone = GameObject.Find("Phone");
        phone.SetActive(false);
    }

    private void LateUpdate()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, ZLevel);
    }
}

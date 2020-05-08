using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QTE : MonoBehaviour
{
    public Component qteScript;

    public void StartQTE()
    {
        (qteScript as Behaviour).enabled = true;
    }
}

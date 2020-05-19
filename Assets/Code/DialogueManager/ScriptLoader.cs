using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Paratoxic.DialogueManager
{
    public class ScriptLoader : MonoBehaviour
    {
        [SerializeField]
        private string scriptName;

        // Start is called before the first frame update
        void Start()
        {
            DialogueManager.LoadScript(scriptName);
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}

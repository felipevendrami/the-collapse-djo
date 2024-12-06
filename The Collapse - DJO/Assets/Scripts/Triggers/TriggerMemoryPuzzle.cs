using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NavKeypad
{
    public class TriggerMemoryPuzzle : MonoBehaviour
    {
        private Keypad keypad;

        void Start()
        {
            keypad = GameObject.Find("KeypadStandard").GetComponent<Keypad>();
        }

        void OnTriggerEnter()
        {
            keypad.StartPuzzle();
        }
    }
}
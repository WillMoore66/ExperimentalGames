using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IgnoreNonVoice : MonoBehaviour
{
    // hi
    public static bool disableKeyboard = false;
    Toggle toggle;

    void Start()
    {
        toggle = GetComponent<Toggle>();
    }

    void Update()
    {
        disableKeyboard = toggle.isOn;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenu : Menu
{
    // Must be assigned in editor, just "hey where main menu go"
    [SerializeField] private Menu mainMenu;

    public void OnMainButton() {
        // I love putting parameters into coroutines, it's not scuffed at all!!!
        var openMain = MoveCameraTo(mainMenu);
        StartCoroutine(openMain);
    }
}

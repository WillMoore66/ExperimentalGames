using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void OnButton() {
        SceneManager.LoadScene("EvelynScene");
        Debug.Log("wha");  
    }

    // Ok so here's how the menu's going to work
    // Could have a list of abstract structs which could represent each page with a singleton class to transmit
    // data between scenes which is modified as you navigate the menus.

    // Menus will need a transition method and some method to animate.
    // Menu buttons will have an OnClick() and OnHover() function which gets inherited
    // Superlative parent method will contain/reference a subroutine which causes the button to blink and make a sound when pressed by default.

    // Main menu-
    // Background image is static while text buttons and whatnot float around?

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : Menu
{
    // Must be assigned in editor, just "hey where options menu go"
    [SerializeField] private Menu optionsMenu;

    // Will work once project settings are edited to add scenes to build settings
    public void OnPlayButton() {
        SceneManager.LoadScene("CallumScene");
        Debug.Log("hi");
    }

    public void OnOptionsButton() {
        // I love putting parameters into coroutines, it's not scuffed at all!!!
        var openOptions = MoveCameraTo(optionsMenu);
        StartCoroutine(openOptions);
    }

    public void OnExitButton() {
        Application.Quit();
    }

    // Start function, just puts the player set up in front of this menu when the scene is loaded
    void Start() {
        Camera.main.transform.position = cameraPosition;
        Camera.main.transform.eulerAngles = cameraRotation;
    }
}

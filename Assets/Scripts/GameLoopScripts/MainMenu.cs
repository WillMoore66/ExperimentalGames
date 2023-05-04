using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Windows.Speech;

public class MainMenu : Menu
{
    // Must be assigned in editor, just "hey where options/credits menus go"
    [SerializeField] private Menu optionsMenu;
    [SerializeField] private Menu creditsMenu;
    private Dictionary<string, Action> keywords = new Dictionary<string, Action>();
    private KeywordRecognizer keywordRecognizer;

    private void Awake() {
        keywords.Add("bork", OnPlayButton);
        keywords.Add("walkies", OnExitButton);
        keywords.Add("exit", OnExitButton);
        keywords.Add("how to bork", OnOptionsButton);
        keywords.Add("options", OnOptionsButton);
        keywords.Add("settings", OnOptionsButton);
        keywords.Add("owners", OnCreditsButton);
        keywords.Add("credits", OnCreditsButton);

        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += OnKeywordsRecognised;
        keywordRecognizer.Start();
    }

    private void OnKeywordsRecognised(PhraseRecognizedEventArgs args) {
        Debug.Log("Keyword: " + args.text);
        keywords[args.text].Invoke();
    }

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

    public void OnCreditsButton() {
        // I love putting parameters into coroutines, it's not scuffed at all!!!
        var openCredits = MoveCameraTo(creditsMenu);
        StartCoroutine(openCredits);
    }

    public void OnExitButton() {
        Application.Quit();
        Debug.Log("The game is closed, application.quit just doesn't do anything in playtest mode");
    }

    // Start function, just puts the player set up in front of this menu when the scene is loaded
    void Start() {
        Camera.main.transform.position = cameraPosition;
        Camera.main.transform.eulerAngles = cameraRotation;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Windows.Speech;

// is this code...... COPIED FROM OPTIONSMENU???!!!??!?!??!!?
public class CreditsMenu : Menu
{

    private Dictionary<string, Action> keywords = new Dictionary<string, Action>();
    private KeywordRecognizer keywordRecognizer;

    // Must be assigned in editor, just "hey where main menu go"
    [SerializeField] private Menu mainMenu;

    private void Awake() {
        //keywords.Add("bad dog", OnMainButton);
        keywords.Add("nico nico nii", OnMainButton);

        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += OnKeywordsRecognised;
        keywordRecognizer.Start();
    }

    private void OnKeywordsRecognised(PhraseRecognizedEventArgs args) {
        Debug.Log("Keyword: " + args.text);
        keywords[args.text].Invoke();
    }

    

    public void OnMainButton() {
        // Audio
        SoundManager.current.PlaySound("Button Click", true);

        // I love putting parameters into coroutines, it's not scuffed at all!!!
        var openMain = MoveCameraTo(mainMenu);
        StartCoroutine(openMain);
    }
}
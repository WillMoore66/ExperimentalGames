using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Windows.Speech;

public class HatSelector : MonoBehaviour
{
    // Evelyn is the one who figured out how the voice recognition worked and showed me how to do it
    private Dictionary<string, Action> keywords = new Dictionary<string, Action>();
    private KeywordRecognizer keywordRecognizer;

    // When a hat is selected, this is written to.
    public static GameObject selectedHat;

    [SerializeField] private string sceneIDToLoad;
    [SerializeField] private string voiceKeywordForLevel;
    [SerializeField] private GameObject buttonHatPrefab;

    public void OnHatButtonPress()
    {
        selectedHat = buttonHatPrefab;
        Debug.Log("Hat selected: " + buttonHatPrefab.name);
    }

    public void OnRequestSceneChange()
    {
        // We want to load the next scene now
        SceneManager.LoadScene(sceneIDToLoad);
        // Shouldn't 
        Debug.LogError("hi something has gone catastrophically wrong in typical unity fashion");
    }

    public static void ApplyHatToDog(GameObject dog)
    {
        try
        {
            // Make a new hat and sellotape it to the dog
            GameObject newHat = Instantiate(selectedHat);
            newHat.transform.position = dog.transform.position;
            //newHat.transform.localScale = new Vector3(dog.transform.localScale.x / newHat.transform.localScale.x, dog.transform.localScale.y / newHat.transform.localScale.y, dog.transform.localScale.z / newHat.transform.localScale.z);
            newHat.transform.eulerAngles = dog.transform.eulerAngles;
            newHat.transform.parent = dog.transform;
        }
        catch (ArgumentException ex)
        {
            // No hat is currently selected through one means or another
            Debug.Log(ex.ToString());
            // We essentially just want to skip this because the dog is fine as is
        }
    }

    private void Start()
    {
        keywords.Add(voiceKeywordForLevel, OnRequestSceneChange);
    }
}

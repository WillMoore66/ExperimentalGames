using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Script intended to be slapped onto elements which need to modify sound settings such as volume */

public class SoundController : MonoBehaviour
{
    SoundManager soundManager;

    [SerializeField]
    private string managedCategory;

    private void Start() {
        UpdateSoundManager();
    }

    private void UpdateSoundManager() {
        // get instance of soundmanager
        try {
            SoundManager soundManager = SoundManager.current;
        }
        catch (NullReferenceException) {
            Debug.LogError("NullReferenceException: The sound manager is not initialized!!");
        }
    }

    public void SetMasterVolume(System.Single newVolume) {
        UpdateSoundManager();
        Debug.Log("what " + newVolume);
        SoundManager.masterVolume = newVolume;
    }

    public void SetCategoryVolume(System.Single newVolume) {
        UpdateSoundManager();
        soundManager.soundCategories[soundManager.GetCategoryIndexFromID(managedCategory)].volume = newVolume;
        foreach (AudioSource currentSource in soundManager.soundCategories[soundManager.GetCategoryIndexFromID(managedCategory)].audioSources) {
            currentSource.volume = newVolume * SoundManager.masterVolume;
        }
    }

    public void SetCategoryVolume(string categoryID, float newVolume) {
        UpdateSoundManager();
        soundManager.soundCategories[soundManager.GetCategoryIndexFromID(categoryID)].volume = newVolume;
        foreach(AudioSource currentSource in soundManager.soundCategories[soundManager.GetCategoryIndexFromID(categoryID)].audioSources) {
            currentSource.volume = newVolume * SoundManager.masterVolume;
        }
    }
}

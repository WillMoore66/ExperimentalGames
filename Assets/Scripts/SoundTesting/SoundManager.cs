using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    /* 
    -----------------------------------------------------------

    These notes are due an update and probably aren't accurate anymore if I'm honest.
    
    This is the sound manager class which manages sounds (crazy I know)
    There is intended to be one single instance in the whole game, loaded upon game start and persisting between scenes (singleton jumpscare)
    (The above is managed by another class that just checks if we've made one yet, and if not then we do that)

    Managed by this class are the MUSIC and SOUND systems (they are kinda separate)
    To use it, open the prefab SoundManager in the Unity Editor, and find the instance of this class.

    From there you can add music tracks and sound effects to the game by appending them to the appropriate list. 
    These are played by giving each sound and track a unique string, and then passing an identical one when calling the appropriate "Play" function

    The sound manager will handle instances of sound objects so you don't have to worry about them.

    --------------------------------------------------------------
    */

    // -- Variables and stuff --
    // One instance of soundManager per scene (kind of a singleton class but also not really)
    public static SoundManager current;
    public static bool ready = false;

    // Definition of sound - stringID pair class
    [System.Serializable]
    private class AudioClipPair {
        public string ID;
        public AudioClip sound;
        public float volume = 1f;
    }

    // List of registered sounds.. Editable in inspector
    //[Header("Registered sounds")]
    [SerializeField] private List<AudioClipPair> registeredSounds;
    [SerializeField] private List<List<AudioSource>> soundCategories;

    // List of currently playing sound effects
    private List<AudioSource> currentSounds;
    // List of currently playing music - kept seperate so stopAllSounds won't affect it
    private List<AudioSource> currentMusic;


    // ==== Functions ====
    // Start function - singleton management
    void Start() {
        // Prevent multiple instances of the sound system from existing at once
        if (ready) {
            Destroy(this);
        }
        else {
            current = this;
            currentSounds = new List<AudioSource>();
            currentMusic = new List<AudioSource>();
        }
    }

    // === Private functions used below to spawn an audio source ===
    // Base function used as more of a failsafe - always should add component to list
    private AudioSource CreateAudioSource(string soundID) {
        GameObject audioSourceObj = new GameObject(soundID);
        AudioSource audioSourceComp = audioSourceObj.AddComponent<AudioSource>();
        audioSourceComp.clip = GetClipFromID(soundID);

        currentSounds.Add(audioSourceComp);
        return audioSourceComp;
    }

    // Overload which allows a list to be specified. These are used for stuff like muting all sounds/music or whatever.
    private AudioSource CreateAudioSource(string soundID, List<AudioSource> listToAddTo) {
        GameObject audioSourceObj = new GameObject(soundID);
        AudioSource audioSourceComp = audioSourceObj.AddComponent<AudioSource>();

        audioSourceComp.clip = GetClipFromID(soundID);
        audioSourceComp.Play();

        listToAddTo.Add(audioSourceComp);
        currentSounds.Add(audioSourceComp);
        return audioSourceComp;
    }

    // Gets an audio clip from the library using the audio ID
    private AudioClip GetClipFromID(string soundID) {
        foreach( AudioClipPair pair in registeredSounds) {
            if (pair.ID.Equals(soundID)) {
                Debug.Log("got it");
                return pair.sound;
            }
        }

        // Catch for if none is found
        Debug.LogError("No sound for given ID " + soundID);
        return currentSounds[0].clip;
    }

    // === Public functions used to play sounds ====
    // These first few will probably be used the most
    public AudioSource PlaySound(string soundID) {
        AudioSource audioSourceComp = CreateAudioSource(soundID);
        GameObject audioSourceObj = audioSourceComp.gameObject;

        // Default to play at camera position
        audioSourceObj.transform.position = Camera.main.transform.position;
        audioSourceObj.transform.parent = Camera.main.transform;
        audioSourceComp.Play();

        return audioSourceComp;
    }

   public AudioSource PlaySound(string soundID, GameObject emitter) {
        AudioSource audioSourceComp = CreateAudioSource(soundID);
        GameObject audioSourceObj = audioSourceComp.gameObject;

        // Default to play at camera position
        audioSourceObj.transform.position = emitter.transform.position;
        audioSourceObj.transform.parent = emitter.transform;

        return audioSourceComp;
    }

    public AudioSource PlaySound(string soundID, Vector3 position) {
        AudioSource audioSourceComp = CreateAudioSource(soundID);
        GameObject audioSourceObj = audioSourceComp.gameObject;

        // Play wherever
        audioSourceObj.transform.position = position;

        return audioSourceComp;
    }


    // == Play all sounds in list ==
    public void PlayAllSounds(List<AudioSource> currentSourceList) {
        foreach (AudioSource currentSource in currentSourceList) {
            currentSource.Play();
        }
    }

    public void PlayAllSounds() {
        PlayAllSounds(currentSounds);
    }

    // == Pause all sounds in list ==
    public void PauseAllSounds(List<AudioSource> currentSourceList) {
        foreach (AudioSource currentSource in currentSourceList) {
            currentSource.Pause();
        }
    }

    public void PauseAllSounds() {
        PauseAllSounds(currentSounds);
    }

    // == Destroy all sounds in list ==
    public void RemoveAllSounds(List<AudioSource> currentSourceList) {
        foreach (AudioSource currentSource in currentSourceList) {
            Destroy(currentSource.gameObject);
        }
    }

    public void RemoveAllSounds() {
        RemoveAllSounds(currentSounds);
    }

    // == Play and pause music == //
    public AudioSource PlayMusic(string MusicID) {
        GameObject audioSourceObj = new GameObject(MusicID);
        AudioSource audioSourceComp = audioSourceObj.AddComponent<AudioSource>();

        audioSourceComp.clip = GetClipFromID(MusicID);
        audioSourceComp.Play();

        // Default to play at camera position
        audioSourceObj.transform.position = Camera.main.transform.position;
        audioSourceObj.transform.parent = Camera.main.transform;

        // Add to current
        currentMusic.Add(audioSourceComp);

        return audioSourceComp;
    }

    public void PlayMusic() {
        foreach (AudioSource currentSource in currentMusic) {
            currentSource.Play();
        }
    }

    public void PauseMusic() {
        foreach (AudioSource currentSource in currentMusic) {
            currentSource.Pause();
        }
    }
    public void RemoveMusic() {
        foreach (AudioSource currentSource in currentMusic) {
            Destroy(currentSource.gameObject);
        }
    }
}

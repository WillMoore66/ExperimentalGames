using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    /* 
    -----------------------------------------------------------
   
    "What is any of this" well I am GLAD you asked-
    
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

    // One instance of soundManager per scene (kind of a singleton class but also not really)
    public static SoundManager current;

    // Definition of sound - stringID pair class
    private class AudioClipPair {
        public string ID;
        public AudioClip sound;
    }

    // List of registered sounds.. Editable in inspector
    //[Header("Registered sounds")]
    [SerializeField] private List<AudioClip> registeredSounds;

    // Start function heeoo
   


}
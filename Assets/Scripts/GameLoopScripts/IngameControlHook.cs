using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IngameControlHook : MonoBehaviour
{
    // Can be called from anywhere. Just call this after the win condition is hit I guess

    // Will also put like some win screen stuff here as well probably

    public static void returnToMenu() {
        SceneManager.LoadScene("DeclanScene"); // Scene switch needs to specify the menu rather than just this when it's ready
    }
}

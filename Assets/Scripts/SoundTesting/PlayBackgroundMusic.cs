using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBackgroundMusic : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AudioSource the = SoundManager.current.PlaySound("HowToPlayTheme",SoundManager.current.soundCategories[SoundManager.current.GetCategoryIndexFromID("music")]);
        the.loop = true;
    }
}

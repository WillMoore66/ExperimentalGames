using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatHook : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Applying hat");
        // Applies the selected hat to whatever the script is parented to
        HatSelector.ApplyHatToDog(this.gameObject);
    }
}

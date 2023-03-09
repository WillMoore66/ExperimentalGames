using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hats : MonoBehaviour
{
    // Data stuff
    public struct hatPrefabPair
    {
        public GameObject model;
        public int hatID;
    };
    public static List<hatPrefabPair> allHats;
    [SerializeField] private List<hatPrefabPair> hats;

    // Start is called before the first frame update
    void Start()
    {
        allHats = hats;
    }

    public static GameObject addHat(int hatID, GameObject dog)
    {
        GameObject hat = Instantiate(allHats[hatID].model);
        hat.transform.position = dog.transform.position;
        hat.transform.parent = dog.transform;
        return hat;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotlightScript : MonoBehaviour
{
    [SerializeField] GameObject player;

    void Update()
    {
        this.transform.LookAt(player.transform.position);
    }
}

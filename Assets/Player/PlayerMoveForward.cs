using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveForward : MonoBehaviour
{
    void Update()
    {
        this.transform.position += this.transform.forward*0.01f;
    }
}

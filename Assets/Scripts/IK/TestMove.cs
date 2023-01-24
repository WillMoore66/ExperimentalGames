using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMove : MonoBehaviour
{
    void FixedUpdate()
    {
        this.transform.position += new Vector3(0, 0, -0.1f);
    }
}

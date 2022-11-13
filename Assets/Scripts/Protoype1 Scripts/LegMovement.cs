using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegMovement : MonoBehaviour
{
    [SerializeField] private GameObject joint;

    void Update()
    {
        if (this.transform.parent)
        {
            this.transform.RotateAround(joint.transform.position, this.transform.right, 2);
        }
    }
}

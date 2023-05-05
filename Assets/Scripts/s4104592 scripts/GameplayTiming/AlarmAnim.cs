using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmAnim : MonoBehaviour
{
    float rotSpeed = 60f;
    float rotTime = 1f;
    float timeElapsed = 0f;
    bool rotRight = true;

    void Update()
    {
        if (rotRight)
        {
            transform.Rotate(Vector3.forward * (rotSpeed * Time.deltaTime));
        }
        else 
        {
            transform.Rotate(Vector3.back * (rotSpeed * Time.deltaTime));
        }
        timeElapsed += Time.deltaTime;
        if (timeElapsed >= rotTime)
        {
            rotRight = !rotRight;
            timeElapsed = 0f;
        }
    }
}

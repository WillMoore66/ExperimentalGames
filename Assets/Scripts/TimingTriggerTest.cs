using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingTriggerTest : MonoBehaviour
{
    [SerializeField] int preStumbleTicks;
    [SerializeField] int successTicks;
    [SerializeField] int postStumbleTicks;

    uint counter;
    bool dogInTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "dog")
        {
            counter = 0;
            dogInTrigger = true;
        }
    }

    void FixedUpdate()
    {
        if (dogInTrigger)
        {
            counter++;

            //if (player says the right word)
            //{

            if (counter <= 30 /*&& player says the right word*/)
            {
                //dog stumbles before reaching obstacle
                Debug.Log("stumble before");
            }
            else if (counter <= 90 /*&& player says the right word*/)
            {
                //dog successfully gets through the obstacle
                Debug.Log("successful dog!");
            }
            else if (counter <= 120)
            {
                //dog stumbles after reaching obstacle
                Debug.Log("stumble after");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "dog")
        {
            dogInTrigger = false;
        }
    }
}

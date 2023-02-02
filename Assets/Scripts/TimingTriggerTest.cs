using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingTriggerTest : MonoBehaviour
{
    [SerializeField] int preStumbleTicks;
    [SerializeField] int postStumbleTicks;

    uint counter;
    bool dogInTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
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

            if (counter <= preStumbleTicks /*&& player says the right word*/)
            {
                //dog stumbles before reaching obstacle
                Debug.Log("stumble before");
            }
            else if (counter >= postStumbleTicks)
            {
                //dog stumbles after reaching obstacle
                Debug.Log("stumble after");
            }
            else
            {
                Debug.Log("Successful Dog!");
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

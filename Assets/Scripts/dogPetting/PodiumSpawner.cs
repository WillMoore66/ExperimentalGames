using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PodiumSpawner : MonoBehaviour
{
    [SerializeField] List<GameObject> dogs;
    [SerializeField] List<GameObject> podiumPositions;
    TimerScript timerScript;

    [SerializeField] float silverTime;
    [SerializeField] float goldTime;

    void Start()
    {
        //findobjectoftype is not very safe
        timerScript = FindObjectOfType<TimerScript>();

        Debug.Log(timerScript.timer);

        //lots of duplication, should instead be done cleverly by manipulating list indexes
        if (timerScript.timer > silverTime)     //player gets bronze
        {
            dogs[0].gameObject.transform.position = podiumPositions[2].transform.position;
            dogs[1].gameObject.transform.position = podiumPositions[0].transform.position;
            dogs[2].gameObject.transform.position = podiumPositions[1].transform.position;
        }
        else if (timerScript.timer > goldTime) //player gets silver
        {
            dogs[0].gameObject.transform.position = podiumPositions[1].transform.position;
            dogs[1].gameObject.transform.position = podiumPositions[0].transform.position;
            dogs[2].gameObject.transform.position = podiumPositions[2].transform.position;
        }
        else                          //player gets gold
        {
            dogs[0].gameObject.transform.position = podiumPositions[0].transform.position;
            dogs[1].gameObject.transform.position = podiumPositions[1].transform.position;
            dogs[2].gameObject.transform.position = podiumPositions[2].transform.position;
        }
    }
}

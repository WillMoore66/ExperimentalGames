using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log(Time.timeSinceLevelLoad);
            //reset game
            SceneManager.LoadScene("DogPettingTestScene");
        }
    }
}

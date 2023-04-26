using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimation : MonoBehaviour
{

    [SerializeField] private Animator myAnimationController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PhysicsBasedPlayer"))
        {
            myAnimationController.SetBool("DogInTunnel", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PhysicsBasedPlayer"))
        {
            myAnimationController.SetBool("DogInTunnel", false);
        }
    }

}




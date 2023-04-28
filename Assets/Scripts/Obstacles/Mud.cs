using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mud : MonoBehaviour
{
    [SerializeField] Material mudMat;
    [SerializeField] int mudTime;
    [SerializeField] int mudSlowAmount;

    GameObject player;
    [SerializeField] Material defaultMat;
    SkinnedMeshRenderer playerRenderer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            player = other.gameObject;
            //this is slow as it checks in all children and there are a lot of them
            playerRenderer = player.GetComponentInChildren<SkinnedMeshRenderer>();
            StartCoroutine("slowDown");
        }
    }

    IEnumerator slowDown()
    {
        playerRenderer.material = mudMat;
        player.transform.GetComponent<NewCharacterController>().maxDogSpeed -= mudSlowAmount;
        yield return new WaitForSeconds(mudTime);
        playerRenderer.material = defaultMat;
        Debug.Log("Switching material to " + defaultMat.name);
        player.transform.GetComponent<NewCharacterController>().maxDogSpeed += mudSlowAmount;
    }
}

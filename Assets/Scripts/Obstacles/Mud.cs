using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mud : MonoBehaviour
{
    [SerializeField] Material mudMat;
    [SerializeField] int mudTime;

    GameObject player;
    Material defaultMat;
    Renderer playerRenderer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            player = other.gameObject;
            playerRenderer = player.GetComponent<Renderer>();
            defaultMat = playerRenderer.material;
            StartCoroutine("slowDown");
        }
    }

    IEnumerator slowDown()
    {
        playerRenderer.material = mudMat;
        player.transform.GetComponent<CharacterContoller>().theSpEEdOftheDog -= 12f;
        yield return new WaitForSeconds(mudTime);
        playerRenderer.material = defaultMat;
        player.transform.GetComponent<CharacterContoller>().theSpEEdOftheDog += 12f;

    }
}

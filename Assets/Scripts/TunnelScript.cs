using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelScript : MonoBehaviour
{
    [SerializeField] GameObject tunnelEntrance, tunnelEnd;
    [SerializeField] GameObject dog, sphere;
    [SerializeField] Camera mainCam, tunnelCam;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine(CameraMove());
        }
    }

    IEnumerator CameraMove() {
        mainCam.enabled = false;
        tunnelCam.enabled = true;
        dog.SetActive(false);

        sphere.transform.position = (sphere.transform.position = Vector3.Lerp(tunnelEntrance.transform.position, tunnelEnd.transform.position, 0.5f));
        for (int i = 0; i < 180; i++) {
            yield return new WaitForFixedUpdate();
            tunnelCam.transform.RotateAround(sphere.transform.position, sphere.transform.up, 1); 
        }
        tunnelCam.enabled = false;
        mainCam.enabled = true;
        dog.transform.position = tunnelEnd.transform.position;
        dog.transform.position = new Vector3(dog.transform.position.x, -0.5200009f, dog.transform.position.z);
        dog.transform.eulerAngles = new Vector3(dog.transform.eulerAngles.x, dog.transform.eulerAngles.y + 180, dog.transform.eulerAngles.z);
        dog.SetActive(true);
    }
}

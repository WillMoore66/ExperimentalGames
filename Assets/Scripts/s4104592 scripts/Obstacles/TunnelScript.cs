using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelScript : MonoBehaviour
{
    [SerializeField] GameObject tunnelEntrance, tunnelEnd;
    [SerializeField] GameObject dog, sphere;
    [SerializeField] Camera mainCam, tunnelCam;
    public float speed;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine(CameraMove());
        }
    }

    IEnumerator CameraMove() {
        //set tunnel camera to viewpoint above tunnel entrance, swap from main camera to tunnel camera, disable dog during tunnel
        tunnelCam.transform.position = new Vector3(333.37f, 10.2f, -49.56f);
        tunnelCam.gameObject.SetActive(true);
        mainCam.enabled = false;
        dog.SetActive(false);
        
        //spawn sphere between tunnel entrance and exit, then rotate tunnel camera around the midpoint
        sphere.transform.position = (sphere.transform.position = Vector3.Lerp(tunnelEntrance.transform.position, tunnelEnd.transform.position, 0.5f));
        
        for (float i = 0; i < 180; i+=speed) {
            yield return new WaitForFixedUpdate();
            tunnelCam.transform.RotateAround(sphere.transform.position, sphere.transform.up, speed); 
        }

        //swap back to main camera, set dog's new position to tunnel exit, reenable dog
        tunnelCam.gameObject.SetActive(false);
        mainCam.enabled = true;
        dog.transform.position = tunnelEnd.transform.position;
        dog.transform.position = new Vector3(dog.transform.position.x, -0.5200009f, dog.transform.position.z);
        dog.transform.eulerAngles = new Vector3(dog.transform.eulerAngles.x, dog.transform.eulerAngles.y + 180, dog.transform.eulerAngles.z);
        dog.SetActive(true);
        dog.GetComponent<NewCharacterController>().dogDirection = 0;
        dog.GetComponent<NewCharacterController>().currentAngle = dog.GetComponent<NewCharacterController>().dogDirection * 90;
    }
}

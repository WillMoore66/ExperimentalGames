using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampolineScript : MonoBehaviour
{
    GameObject dog;
    [SerializeField] int strength;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")   
        {
            dog = collision.gameObject;
            Rigidbody rb = dog.GetComponent<Rigidbody>();
            rb.velocity = rb.gameObject.transform.forward* strength + new Vector3(0,strength,0);
            StartCoroutine("KeepRotation");
        }
    }

    IEnumerator KeepRotation()
    {
        int frames = 0;
        while (frames < 180)
        {
            dog.transform.rotation = Quaternion.Euler(dog.transform.eulerAngles.x, 180, dog.transform.eulerAngles.z);
            frames++;
            yield return new WaitForFixedUpdate();
        }
    }
}

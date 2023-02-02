using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelScript : MonoBehaviour
{
    [SerializeField] GameObject tunnelEnd;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.transform.position = tunnelEnd.transform.position;
            other.transform.eulerAngles = new Vector3 (other.transform.eulerAngles.x, other.transform.eulerAngles.y + 180, other.transform.eulerAngles.z);
        }
    }
}

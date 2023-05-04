using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingTriggerTest : MonoBehaviour
{
    [SerializeField] int preStumbleTicks;
    [SerializeField] int prePerfectTicks;
    [SerializeField] int postPerfectTicks;
    [SerializeField] int postStumbleTicks;

    GameObject perfect;
    GameObject notBad;
    GameObject tooEarly;
    GameObject tooLate;

    uint counter;
    bool dogInTrigger;
    int dogLateness;

    GameObject dog;

    [SerializeField] Vector3 startScale = Vector3.zero;
    [SerializeField] Vector3 endScale = new Vector3(5, 4, 5);
    [SerializeField] float duration = 1f;
    [SerializeField] GameObject tunnel;

    NewCharacterController playerController;
    GameObject crowdParent;
    Rigidbody rb;

    //Gameobject.Find is unsafe and slow and shouldnt be used
    private void Awake()
    {
        perfect = GameObject.Find("Perfect");
        notBad = GameObject.Find("NotBad");
        tooEarly = GameObject.Find("TooEarly");
        tooLate = GameObject.Find("TooLate");
        crowdParent = GameObject.Find("AudienceParent");

        if (!tunnel)
        {
            tunnel = this.gameObject;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            counter = 0;
            dogInTrigger = true;
            dogLateness = 0;
            playerController = other.GetComponent<NewCharacterController>();
        }
        dog = other.gameObject;
        rb = dog.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (dogInTrigger)
        {
            counter++;
            ResolveLateness();
            Debug.Log(dogLateness);
        }
    }

    void ResolveLateness()
    {
        if (playerController.jumping || playerController.tunnelling)
        {
            if (counter <= preStumbleTicks)
            {
                //dog is early
                dogLateness = 1;
                tunnel.GetComponent<TunnelScript>().speed = 0.5f;
            }
            else if (counter <= prePerfectTicks)
            {
                //dog is okay but a little early
                dogLateness = 2;
                tunnel.GetComponent<TunnelScript>().speed = 1f;
            }
            else if (counter > prePerfectTicks)
            {
                //dog is perfect
                dogLateness = 3;
                tunnel.GetComponent<TunnelScript>().speed = 1.5f;
            }
            else if (counter <= postPerfectTicks)
            {
                //dog is okay but a little late
                dogLateness = 2;
                tunnel.GetComponent<TunnelScript>().speed = 1f;
            }
            else if (counter > postPerfectTicks)
            {
                //dog is late
                dogLateness = 4;
                tunnel.GetComponent<TunnelScript>().speed = 0.5f;
            }
            else
            {
                dogLateness = 4;
                tunnel.GetComponent<TunnelScript>().speed = 0.5f;
            }
        }
        PlayFeedback();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            dogInTrigger = false;
            //PlayFeedback();
        }
    }

    void PlayFeedback()
    {
        switch (dogLateness)
        {
            case 1:
                Debug.Log("stumble before");
                StartCoroutine("LerpScale", tooEarly);
                //rb.velocity -= dog.transform.forward * 3;
                break;
            case 2:
                Debug.Log("Okay!");
                StartCoroutine("LerpScale", notBad);
                crowdParent.GetComponent<CrowdGenerator>().React();
                break;
            case 3:
                Debug.Log("perfect");
                StartCoroutine("LerpScale", perfect);
                crowdParent.GetComponent<CrowdGenerator>().React();
                //rb.velocity += dog.transform.forward * 3;
                break;
            case 4:
                Debug.Log("stumble after");
                StartCoroutine("LerpScale", tooLate);
                //rb.velocity -= dog.transform.forward * 3;
                break;
            default:
                //Debug.Log("way too late");
                break;
        }
    }

    IEnumerator LerpScale(GameObject p)
    {
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float fraction = timer / duration;
            fraction = Mathf.Clamp01(fraction);

            // Use Mathf.SmoothStep to interpolate each component of the scale vector
            float x = Mathf.SmoothStep(startScale.x, endScale.x, fraction);
            float y = Mathf.SmoothStep(startScale.y, endScale.y, fraction);
            float z = Mathf.SmoothStep(startScale.z, endScale.z, fraction);

            // Set the scale of the gameobject to the interpolated vector
            p.transform.localScale = new Vector3(x, y, z);
            yield return null;
        }
        p.transform.localScale = endScale;
        StartCoroutine("AntiLerpScale", p);
    }

    //this should just be done with LerpScale instead of doing all this duplication
    IEnumerator AntiLerpScale(GameObject p)
    {
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime*2;
            float fraction = timer / duration;
            fraction = Mathf.Clamp01(fraction);

            // Use Mathf.SmoothStep to interpolate each component of the scale vector
            float x = Mathf.SmoothStep(p.transform.localScale.x, 0, fraction);
            float y = Mathf.SmoothStep(p.transform.localScale.y, 0, fraction);
            float z = Mathf.SmoothStep(p.transform.localScale.z, 0, fraction);

            // Set the scale of the gameobject to the interpolated vector
            p.transform.localScale = new Vector3(x, y, z);
            yield return null;
        }
        p.transform.localScale = Vector3.zero;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class KeywordInterpretation : MonoBehaviour
{
    private Dictionary<string, Action> keywords = new Dictionary<string, Action>();
    private KeywordRecognizer keywordRecognizer;
    [SerializeField] private int dogSpeed;
    [SerializeField] private int dogTurnSpeed;
    [SerializeField] private int jumpPower;
    private Rigidbody rb;
    private bool midair = false;

    void Start()
    {
        keywords.Add("right", TurnRight);
        keywords.Add("left", TurnLeft);
        keywords.Add("slow", SlowDown);
        keywords.Add("play dead", KillDog);
        keywords.Add("jump", Jump);

        keywordRecognizer = new KeywordRecognizer (keywords.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += OnKeywordsRecognised;
        keywordRecognizer.Start();

        rb = this.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            TurnLeft();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            TurnRight();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            SlowDown();
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            KillDog();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Jump();
        }
    }
    private void FixedUpdate()
    {
        //constantly move forward
        rb.AddForce(this.transform.forward * dogSpeed);
    }

    private void OnKeywordsRecognised(PhraseRecognizedEventArgs args)
    {
        Debug.Log("Keyword: " + args.text);
        keywords[args.text].Invoke();
    }

    private void TurnRight()
    {
        //halve the forward momentum
        rb.velocity /= 2;
        rb.AddRelativeTorque(this.transform.up * dogTurnSpeed, ForceMode.Impulse);
    }

    private void TurnLeft()
    {
        //halve the forward momentum
        //this.GetComponent<Rigidbody>().AddForce(-(this.GetComponent<Rigidbody>().velocity/1.5f));
        rb.velocity /= 2;
        //dog turns to face the left
        rb.AddRelativeTorque(-(this.transform.up * dogTurnSpeed),ForceMode.Impulse );
        //this.transform.Rotate(0, -dogTurnSpeed, 0);
        //rb.angularVelocity += new Vector3(0, -dogTurnSpeed, 0);
        //dog moves left
        //this.GetComponent<Rigidbody>().AddForce(-this.transform.right * dogSpeed*3);
    }

    private void SlowDown()
    {
        rb.velocity /= 4;
    }

    private void KillDog()
    {
        foreach (Transform child in this.transform)
        {
            if (!child.GetComponent<Rigidbody>())
            {
                child.gameObject.AddComponent<BoxCollider>();
                child.gameObject.AddComponent<Rigidbody>();
            }
        }
        this.transform.DetachChildren();
    }

    private void Jump()
    {
        rb.AddForce(0, jumpPower, 0);
        rb.useGravity = true;
        midair = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (midair == true && collision.gameObject.tag == "Floor")
        {
            rb.useGravity = false;
            midair = false;
            rb.velocity -= new Vector3(0, rb.velocity.y, 0);
        }
    }
}

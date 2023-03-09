using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;
using UnityEngine.Windows.Speech;

public class CharacterContoller : MonoBehaviour {
    [SerializeField] GameObject sphere;
    private Dictionary<string, Action> keywords = new Dictionary<string, Action>();
    private KeywordRecognizer keywordRecognizer;

    PlayerInput playerInput;
    [SerializeField][Range(1.0f, 100.0f)] public float theSpEEdOftheDog = 30;
    [SerializeField][Range(1.0f, 360.0f)] float turningDegrees = 45;
    //[SerializeField][Range(1.0f, 360.0f)] float jumpingDegrees = 90;
    [SerializeField][Range(0.0f, 10.0f)] float turningSpeed = 2;
    //[SerializeField][Range(0.0f, 10.0f)] float jumpingSpeed = 2;
    [SerializeField][Range(0.0f, 3.0f)] float jumpTime = 1;
    [SerializeField][Range(0.0f, 3.0f)] float jumpDistance = 1;
    [SerializeField][Range(0.0f, 10.0f)] float jumpHeight = 5;

    [SerializeField] GameObject camera;

    InputAction forwardAction;
    InputAction jumpAction;
    InputAction turnAction;
    InputAction playDeadAction;

    [SerializeField] bool constantForward;
    bool busy;

    bool turningLeft;
    //Vector3 oldPos;

    private void Awake() {
        playerInput = GetComponent<PlayerInput>();
        forwardAction = playerInput.actions["WS"];
        forwardAction.Enable();
        jumpAction = playerInput.actions["Jump"];
        jumpAction.Enable();
        turnAction = playerInput.actions["Turn"];
        turnAction.Enable();
        playDeadAction = playerInput.actions["PlayDead"];
        playDeadAction.Enable();

        keywords.Add("forward", GoForward);
        keywords.Add("backwards", GoForward);
        keywords.Add("jump", Jump);
        keywords.Add("left", TurnLeft);
        keywords.Add("right", TurnRight);
        keywords.Add("play dead", PlayDead);

        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += OnKeywordsRecognised;
        keywordRecognizer.Start();
    }

    void FixedUpdate() {
        //oldPos = this.transform.position;

        if (!busy) {

            if (constantForward)
            {
                GoForward();
            }

            if (forwardAction.ReadValue<float>() != 0 && !constantForward) {
                GoForward();
            } else if (jumpAction.ReadValue<float>() != 0) {
                Jump();
            } else if (turnAction.ReadValue<float>() != 0) {
                Turn();
            }

            if (playDeadAction.WasPerformedThisFrame())
            {
            PlayDead();
            }
        }
    }

    private void OnKeywordsRecognised(PhraseRecognizedEventArgs args) {
        Debug.Log("Keyword: " + args.text);
        keywords[args.text].Invoke();
    }

    void GoForward() {
        this.transform.position += this.transform.forward * theSpEEdOftheDog * Time.deltaTime;
    }

    void TurnLeft()
    {
        StartCoroutine("TurnLeftRoutine");
    }

    void TurnRight()
    {
        StartCoroutine("TurnRightRoutine");
    }

    void Turn() {
        StartCoroutine("TurnRoutine");
    }

    void Jump() {
        StartCoroutine("JumpUp");
    }

    public void PlayDead()
    {
        StartCoroutine("PlayDeadRoutine");
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collisionTest");
        //this.transform.position = oldPos;
    }

    public IEnumerator PlayDeadRoutine()
    {
        busy = true;
        Quaternion currentRot = this.transform.rotation;

        Rigidbody rb = this.GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.None;

        rb.AddTorque(this.transform.right * 1000f);
        rb.AddTorque(this.transform.forward * UnityEngine.Random.Range(-500f, 500f));
        rb.velocity += this.transform.forward * 10f;

        //camera = this.transform.GetChild(0).gameObject;

        this.transform.DetachChildren();

        yield return new WaitForSeconds(5f);

        camera.transform.parent = this.transform;
        camera.transform.localPosition = new Vector3(1.5f, 3.5f, -6f);
        camera.transform.localEulerAngles = new Vector3(15, 0, 0);
        rb.useGravity= false;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        this.transform.rotation = currentRot;
        busy = false;
    }

    IEnumerator TurnRoutine() {
        busy = true;
        if (turnAction.ReadValue<float>() == 1) {
            //turn left
            sphere.transform.position = (this.transform.position + (this.transform.right * (theSpEEdOftheDog / turningSpeed)));
            for (int i = 0; i < turningDegrees; i++) {
                yield return new WaitForFixedUpdate();
                this.transform.RotateAround(sphere.transform.position, sphere.transform.up, turningSpeed);
            }
        } else if (turnAction.ReadValue<float>() == -1) {
            //turn right
            sphere.transform.position = (this.transform.position + (-this.transform.right * (theSpEEdOftheDog / turningSpeed)));
            for (int i = 0; i < turningDegrees; i++) {
                yield return new WaitForFixedUpdate();
                this.transform.RotateAround(sphere.transform.position, sphere.transform.up, -turningSpeed);
            }
        }
        busy = false;
    }

    IEnumerator TurnRightRoutine()
    {
        busy = true;
            //turn right
            sphere.transform.position = (this.transform.position + (this.transform.right * (theSpEEdOftheDog / turningSpeed)));
            for (int i = 0; i < turningDegrees; i++)
            {
                yield return new WaitForFixedUpdate();
                this.transform.RotateAround(sphere.transform.position, sphere.transform.up, turningSpeed);
            }
        busy = false;
    }

IEnumerator TurnLeftRoutine()
{
    busy = true;
        //turn left
        sphere.transform.position = (this.transform.position + (-this.transform.right * (theSpEEdOftheDog / turningSpeed)));
        for (int i = 0; i < turningDegrees; i++)
        {
            yield return new WaitForFixedUpdate();
            this.transform.RotateAround(sphere.transform.position, sphere.transform.up, -turningSpeed);
        }
        busy = false;
}

private IEnumerator JumpUp() 
    {
        busy = true;
        float groundPos = this.transform.position.y;

        while (this.transform.position.y < (groundPos + jumpHeight)) {
            yield return new WaitForFixedUpdate();

            this.transform.position += new Vector3(0, jumpTime, 0);
            this.transform.position += transform.forward * jumpDistance;
        }
        while (this.transform.position.y > (groundPos)) {
            yield return new WaitForFixedUpdate();

            this.transform.position -= new Vector3(0, jumpTime, 0);
            this.transform.position += transform.forward * jumpDistance;
        }
        busy = false;
    }
}

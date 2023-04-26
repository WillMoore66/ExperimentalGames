using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;
using UnityEngine.Windows.Speech;

public class NewCharacterController : MonoBehaviour
{
    private Dictionary<string, Action> keywords = new Dictionary<string, Action>();
    private KeywordRecognizer keywordRecognizer;

    public float speed = 10f;
    bool isGrounded = true;

    private float targetAngle;
    [SerializeField] float turnCooldown;
    [SerializeField] float turnSpeed;
    [SerializeField] float numTurnSteps;

    int dogDirection;

    PlayerInput playerInput;
    [SerializeField][Range(1.0f, 100.0f)] public float maxDogSpeed = 30;
    [SerializeField][Range(0.0f, 1000.0f)] float jumpHeight = 5;

    [SerializeField] GameObject cam;
    [SerializeField] Animator animator;

    InputAction forwardAction;
    InputAction jumpAction;
    InputAction turnAction;
    InputAction playDeadAction;

    [SerializeField] bool constantForward;
    bool busy;
    Rigidbody rb;


    private void Awake()
    {
        //setup actions
        playerInput = GetComponent<PlayerInput>();
        forwardAction = playerInput.actions["WS"];
        forwardAction.Enable();
        jumpAction = playerInput.actions["Jump"];
        jumpAction.Enable();
        turnAction = playerInput.actions["Turn"];
        turnAction.Enable();
        playDeadAction = playerInput.actions["PlayDead"];
        playDeadAction.Enable();

        //setup keywords
        keywords.Add("forward", GoForward);
        keywords.Add("backwards", GoForward);
        keywords.Add("jump", Jump);
        //keywords.Add("left", TurnLeft);
        //keywords.Add("right", TurnRight);
        keywords.Add("play dead", PlayDead);

        //setup keywordRecognizer
        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += OnKeywordsRecognised;
        keywordRecognizer.Start();

        rb = this.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (!busy)
        {
            if (constantForward)
            {
                GoForward();
            }

            if (forwardAction.ReadValue<float>() != 0 && !constantForward)
            {
                GoForward();
            }
            else if (jumpAction.ReadValue<float>() != 0)
            {
                Jump();
            }
            else if (turnAction.ReadValue<float>() != 0)
            {
                if (turnAction.ReadValue<float>() == 1) {
                    //targetAngle = rb.transform.rotation.eulerAngles.y + 90;
                    Turn(false);
                }
                else if (turnAction.ReadValue<float>() == -1) {
                    //targetAngle = rb.transform.rotation.eulerAngles.y - 90;
                    Turn(true);
                }
            }

            if (playDeadAction.WasPerformedThisFrame())
            {
                PlayDead();
            }
        }
    }

    void GoForward()
    {
        if (rb.velocity.magnitude < maxDogSpeed)
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, rb.velocity.z) + this.transform.forward;
        }
    }

    void Turn(bool rightwards)
    {
        if (!busy)
        {
            StartCoroutine("CeaseBusiness");
            busy = true;
            //wrap the dogDirection around if it reaches bounds
            if (!rightwards)
            {
                if (dogDirection == 3)
                {
                    dogDirection = 0;
                }
                else
                {
                    dogDirection++;
                }
            }
            else
            {
                if (dogDirection == 0)
                {
                    dogDirection = 3;
                }
                else
                {
                    dogDirection--;
                }
            }
            targetAngle = dogDirection * 90;
            Debug.Log("targetAngle: " + targetAngle);

            rb.MoveRotation(Quaternion.Euler(0, targetAngle, 0));
        }
    }

    IEnumerator CeaseBusiness()
    {
        yield return new WaitForSeconds(turnCooldown);
        busy = false;
    }

    void Jump()
    {
        if (isGrounded && !busy)
        {
            Vector3 force = new Vector3(0, Mathf.SmoothStep(0, jumpHeight, 1.5f), 0);
            rb.AddForce(force);

            isGrounded = false;
            //needs to make the dog busy as well
            animator.SetTrigger("TriggerJump");
        }
    }

    public void PlayDead()
    {
        //enable ragdoll
    }

    private void OnKeywordsRecognised(PhraseRecognizedEventArgs args)
    {
        Debug.Log("Keyword: " + args.text);
        keywords[args.text].Invoke();
    }

    //this is pretty unsafe as it will allow the player to jump on anything even if they are not below the player
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "floor")
        {
            isGrounded = true;
        }
    }
}


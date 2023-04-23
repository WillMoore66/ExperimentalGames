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
    public float angle = 90f;
    bool isGrounded = true;

    // The amount of torque to apply per frame
    public float torque = 10f;

    // The direction of torque to apply per frame
    public Vector3 torqueDirection = Vector3.up;

    [SerializeField] public float targetAngle = 90f;
    [SerializeField] float currentAngle = 0f;
    Quaternion startingRotation;
    bool turning;
    bool startedTurning;

    PlayerInput playerInput;
    [SerializeField][Range(1.0f, 100.0f)] public float maxDogSpeed = 30;
    [SerializeField][Range(0.0f, 1000.0f)] float turningDegrees = 45;
    [SerializeField][Range(0.0f, 1000.0f)] float rotatingDegrees = 90;
    //[SerializeField][Range(1.0f, 360.0f)] float jumpingDegrees = 90;
    [SerializeField][Range(0.0f, 10.0f)] float turningTime = 2;
    //[SerializeField][Range(0.0f, 10.0f)] float jumpingSpeed = 2;
    [SerializeField][Range(0.0f, 3.0f)] float jumpTime = 1;
    [SerializeField][Range(0.0f, 3.0f)] float jumpDistance = 1;
    [SerializeField][Range(0.0f, 1000.0f)] float jumpHeight = 5;

    [SerializeField] GameObject cam;
    [SerializeField] Animator animator;

    InputAction forwardAction;
    InputAction jumpAction;
    InputAction turnAction;
    InputAction playDeadAction;

    [SerializeField] bool constantForward;
    bool busy;
    float turnDirection;

    bool turningLeft;
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
                turnDirection = turnAction.ReadValue<float>();
                currentAngle = rb.transform.eulerAngles.y;
                if (turnAction.ReadValue<float>() == 1) {
                    targetAngle = rb.transform.rotation.eulerAngles.y + 90;
                }
                else if (turnAction.ReadValue<float>() == -1) {
                    targetAngle = rb.transform.rotation.eulerAngles.y - 90;
                }
                SetTurn();
            }

            if (playDeadAction.WasPerformedThisFrame())
            {
                PlayDead();
            }
        }
        Debug.Log("is turning"+ turning);


        Turn();
    }

    private void OnKeywordsRecognised(PhraseRecognizedEventArgs args)
    {
        Debug.Log("Keyword: " + args.text);
        keywords[args.text].Invoke();
    }

    void GoForward()
    {
        if (rb.velocity.magnitude < maxDogSpeed)
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, rb.velocity.z) + this.transform.forward;
        }
        //animator.Play("AnimationName", 1, (1f / total_frames_in_animation) * desired_frame);
    }

    //void TurnLeft()
    //{
    //    Debug.Log("left");
    //    //StartCoroutine("TurnLeftRoutine");
    //}

    //void TurnRight()
    //{
    //    Debug.Log("right");
    //    //StartCoroutine("TurnRightRoutine");
    //}

    void SetTurn()
    {
        Debug.Log(turning);
        if (turning == true)
        {
            turning = false;
        }
        else 
        {
            turning = true;

        }
    }

    //probably better not to make new vectors every frame in these functions
    void Turn()
    {
        if (turning)
        {
            //Debug.Log(currentAngle);
            //turn
            currentAngle = rb.transform.eulerAngles.y;
            Debug.Log("curret angle = " + currentAngle + " target angle = " + targetAngle);
            if (turnDirection == 1f)
            {
                // Check if the current angle is less than the target angle
                if (currentAngle < targetAngle) {
                    // Apply torque to the rigidbody
                    rb.AddTorque(torqueDirection * -torque);

                    // Update the current angle by calculating the delta angle between the previous and current rotation
                    //currentAngle = Quaternion.Angle(startingRotation, rb.rotation);
                    turning = true; //this line might not be needed
                } else {
                    // Stop applying torque and set it to 0
                    rb.angularVelocity = Vector3.zero;
                    turning = false;
                    turnDirection = 0f;
                }
                busy = true;
                StartCoroutine(CeaseBusiness());
            }
            //if turning right
            else if (turnDirection == -1f)
            {
                // Check if the current angle is less than the target angle
                if (currentAngle > targetAngle)
                {
                    // Apply torque to the rigidbody
                    rb.AddTorque(torqueDirection * torque);

                    // Update the current angle by calculating the delta angle between the previous and current rotation
                    turning = true; //this line might not be needed
                }
                else
                {
                    rb.angularVelocity = Vector3.zero;
                    turning = false;
                    turnDirection = 0f;
                }
                busy = true;
                StartCoroutine(CeaseBusiness());
            }
        }
    }

    IEnumerator CeaseBusiness()
    {
        yield return new WaitForSeconds(1);
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

    //this is pretty unsafe as it will allow the player to jump on anything even if they are not below the player
    void OnCollisionEnter(Collision collision)
    {
        isGrounded = true;
    }
}


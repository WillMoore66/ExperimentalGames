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

    public float currentAngle = 0f;
    float rotationDuration = 1f;
    private float targetAngle = 180;

    [SerializeField] float speedWhileTurning;
    [SerializeField] float turnCooldown;
    [SerializeField] float turnSpeed;
    [SerializeField] float numTurnSteps;

    public int dogDirection;

    PlayerInput playerInput;
    [SerializeField][Range(1.0f, 100.0f)] public float maxDogSpeed = 30;
    [SerializeField][Range(0.0f, 10000.0f)] float jumpHeight = 5;

    [SerializeField] GameObject cam;
    [SerializeField] Animator animator;

    InputAction forwardAction;
    InputAction jumpAction;
    InputAction turnAction;
    InputAction playDeadAction;

    [SerializeField] bool constantForward;
    bool busy;
    Rigidbody rb;
    [SerializeField] float reverseTime;
    [SerializeField] float reverseVelocity;

    public bool jumping = false;
    public bool tunnelling = false;

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
        keywords.Add("left", TurnLeft);
        keywords.Add("right", TurnRight);
        keywords.Add("reverse", Reverse);
        keywords.Add("dig", Tunnel);
        keywords.Add("play dead", PlayDead);

        //setup keywordRecognizer
        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray(), ConfidenceLevel.Low);
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
                    Turn(false);
                }
                else if (turnAction.ReadValue<float>() == -1) {
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

        float time = animator.GetCurrentAnimatorStateInfo(0).normalizedTime * 450;

		Debug.Log(time);

        if (isGrounded) {
            if (rb.velocity.magnitude < 10 && time > 110 || rb.velocity.magnitude < 10 && time < 70) {
                animator.Play("Dog.DogAnimations", 0, 70f / 450f);
            } else if (rb.velocity.magnitude > 10 && time > 55 || rb.velocity.magnitude > 10 && time < 5) {
                animator.Play("Dog.DogAnimations", 0, 5f / 450f);
            }
        }
    }

    void Reverse()
    {
        if (rb.velocity.magnitude < maxDogSpeed)
        {
            rb.velocity = this.transform.forward*-reverseVelocity;
        }
        if (rb.velocity.magnitude < 0) {
			animator.Play("Dog.DogAnimations", 0, 110f / 450f);
            animator.speed = -1;
		}
        StartCoroutine("ReverseRoutine");
    }

    IEnumerator ReverseRoutine()
    {
        constantForward = false;
        yield return new WaitForSeconds(reverseTime);
        constantForward = true;
        animator.speed = 1;
    }

    //tiny functions should be unnecessary but are required for voice commands
    void TurnLeft()
    {
        Turn(true);
    }

    void TurnRight()
    {
        Turn(false);
    }

    void Turn(bool rightwards)
    {
        if (!busy)
        {
            StartCoroutine("CeaseBusiness");
            busy = true;
            //wrap the dogDirection around if it reaches bounds
            //0 = +Z    1= -X   2= -Z   3= +X
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
            currentAngle = targetAngle;
            targetAngle = dogDirection * 90;

            StartCoroutine("Turning");
        }
    }

    IEnumerator Turning()
    {
        float tempTargetAngle = 0;
        if (currentAngle == 0 && targetAngle == 270)
        {
            tempTargetAngle = -90;
        }
        else if (currentAngle == 270 && targetAngle == 0)
        {
            tempTargetAngle = 360;
        }
        else
        {
            tempTargetAngle = targetAngle;
        }

        float timer = 0f;                                                                           // Loop until the timer reaches the rotation duration
        while (timer < rotationDuration)
        {
            timer += Time.deltaTime;                                                                //increment the timer by the time inbetween frames
            float t = Mathf.SmoothStep(0f, rotationDuration, timer / rotationDuration);             //calculate a smooth interpolation factor between 0 and 1
            rb.MoveRotation(Quaternion.Euler(0, Mathf.Lerp(currentAngle, tempTargetAngle, t), 0));      //rotate the rigidbody from the current angle to the target angle by the interpolation factor
            yield return null;                                                                      //wait for the next frame
            rb.velocity += this.transform.forward/3;
        }
    }

    //stops the dog from being busy after the turnCooldown expires
    IEnumerator CeaseBusiness()
    {
        yield return new WaitForSeconds(turnCooldown);
        busy = false;
    }

    void Jump()
    {
        if (this.gameObject)
        {
            if (isGrounded && !busy)
            {
                Vector3 force = new Vector3(0, Mathf.SmoothStep(0, jumpHeight, 1.5f), 0);
                rb.AddForce(force);

                isGrounded = false;

				animator.Play("Dog.DogAnimations", 0, 282f / 450f); //282 is the first jump frame

				StartCoroutine("RegisterJump");
            }
        }
    }

    IEnumerator RegisterJump()
    {
        //Audio
        //SoundManager.current.PlaySound("Jump",this.gameObject,true);

        jumping = true;
        ShowFeedback();
        yield return new WaitForEndOfFrame();
        jumping = false;
    }

    void ShowFeedback() {
		//when the player jumps
		//get the hurdles that are within 15 either side of the dogs x position
		//of those hurdles, get the ones that closer to a point in front of the dog than they are to the dog
		//of those hurdles, get the closest one in the z direction
		//congrats! you now have the hurdle that you will need to jump over next!
		//record the absolute z distance from this hurdle
		//based on this distance, show a specific feedback sprite and move the audience up and down a bit
	}

	void Tunnel()
    {
        try
        {
            StartCoroutine("RegisterTunnel");
        }
        catch (NullReferenceException ex)
        {
            Debug.LogError(ex.ToString());
        }
    }

    IEnumerator RegisterTunnel()
    {
        tunnelling = true;
        yield return new WaitForEndOfFrame();
        tunnelling = false;
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
        //Debug.Log(collision.gameObject.name);

        if (collision.gameObject.tag == "floor")
        {
            isGrounded = true;
        }
    }
}


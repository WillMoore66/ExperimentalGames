using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;
using UnityEngine.Windows.Speech;



public class CharacterContoller : MonoBehaviour
{
    [SerializeField] GameObject sphere;
    private Dictionary<string, Action> keywords = new Dictionary<string, Action>();
    private KeywordRecognizer keywordRecognizer;

    PlayerInput playerInput;
    [SerializeField] [Range(1.0f, 100.0f)] float theSpEEdOftheDog = 30;
	[SerializeField] [Range(1.0f, 360.0f)] float turningDegrees = 45;
	[SerializeField] [Range(1.0f, 360.0f)] float jumpingDegrees = 90;
	[SerializeField] [Range(0.0f, 10.0f)] float turningSpeed = 2;
	[SerializeField] [Range(0.0f, 10.0f)] float jumpingSpeed = 2;
    InputAction forwardAction;
    InputAction jumpAction;
    InputAction turnAction;

    bool busy;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        forwardAction = playerInput.actions["WS"];
        forwardAction.Enable();
        jumpAction = playerInput.actions["Jump"];
        jumpAction.Enable();
        turnAction = playerInput.actions["Turn"];
        turnAction.Enable();

        keywords.Add("forward", GoForward);
        keywords.Add("backwards", GoForward);
        keywords.Add("jump", Jump);
        keywords.Add("left", Turn);
        keywords.Add("right", Turn);

        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += OnKeywordsRecognised;
        keywordRecognizer.Start();
    }

    void Update()
    {
        if (!busy)
        {
            if (forwardAction.ReadValue<float>() != 0)
            {
                GoForward();
            }
            else if (jumpAction.ReadValue<float>() != 0)
            {
                Jump();
            }
            else if (turnAction.ReadValue<float>() != 0)
            {
                Turn();
            }
        }
    }

    private void OnKeywordsRecognised(PhraseRecognizedEventArgs args)
    {
        Debug.Log("Keyword: " + args.text);
        keywords[args.text].Invoke();
    }

    void GoForward()
    {
        this.transform.position += this.transform.forward * theSpEEdOftheDog * forwardAction.ReadValue<float>() * Time.deltaTime;
    }

    void Turn()
    {
        StartCoroutine("TurnRoutine");
    }

    void Jump()
    {
        StartCoroutine("JumpUp");
    }

    IEnumerator TurnRoutine()
    {
        busy = true;
        if (turnAction.ReadValue<float>() == 1)
        {
            //turn left
            sphere.transform.position = (this.transform.position + (this.transform.right * (theSpEEdOftheDog/turningSpeed)));
            for (int i = 0; i < turningDegrees; i++)
            {
                yield return new WaitForFixedUpdate();
                this.transform.RotateAround(sphere.transform.position, sphere.transform.up, turningSpeed);
            }
        }
        else if (turnAction.ReadValue<float>() == -1)
        {
            //turn right
            sphere.transform.position = (this.transform.position + (-this.transform.right * (theSpEEdOftheDog/turningSpeed)));
            for (int i = 0; i < turningDegrees; i++)
            {
                yield return new WaitForFixedUpdate();
                this.transform.RotateAround(sphere.transform.position, sphere.transform.up, -turningSpeed);
            }
        }
        busy = false;
    }

    //Jumping needs to be fixed because the dog will jump under the ground or rotate strangely when facing the wrong direction
    private IEnumerator JumpUp()
    {
        busy = true;
        Debug.Log("hhy");
        sphere.transform.position = (new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z) + this.transform.forward * theSpEEdOftheDog / 3);
        for (int i = 0; i < jumpingDegrees; i++)
        {
            yield return new WaitForFixedUpdate();
            this.transform.RotateAround(sphere.transform.position, sphere.transform.right, jumpingSpeed);
            this.transform.Rotate(-this.transform.right * jumpingSpeed);
        }
        busy = false;
    }
}
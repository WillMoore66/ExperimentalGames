using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] int framesToRevert;
    [SerializeField] int collidedDogSpeed;
    [SerializeField] int framesOfSlow;

    float defaultDogSpeed;

    int frameRevertCounter;
    int slowTimeCounter;
    Vector3 oldPos;

    private void Start()
    {
        oldPos = player.transform.position;
        defaultDogSpeed = player.GetComponent<CharacterContoller>().theSpEEdOftheDog;
    }

    private void FixedUpdate()
    {
        if (frameRevertCounter >= framesToRevert)
        {
            oldPos = player.transform.position;
            frameRevertCounter = 0;
        }
        else
        {
            frameRevertCounter++;
        }

        if (slowTimeCounter > 0) 
        {
            slowTimeCounter--;
        }

        slowDown();

        Debug.Log(player.GetComponent<CharacterContoller>().theSpEEdOftheDog);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collisionTest");
        player.transform.position = oldPos;
        player.GetComponent<CharacterContoller>().theSpEEdOftheDog = collidedDogSpeed;
        slowTimeCounter = framesOfSlow;
    }

    void slowDown()
    {
        //this is a pretty inefficient way of doing this because it sets it each frame
        if (slowTimeCounter > 0)
        {
            player.transform.GetComponent<CharacterContoller>().theSpEEdOftheDog = collidedDogSpeed;
        }
        else
        {
            player.transform.GetComponent<CharacterContoller>().theSpEEdOftheDog = defaultDogSpeed;
        }
    }
}

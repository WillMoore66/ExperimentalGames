using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CrowdGenerator : MonoBehaviour
{
    [SerializeField] private List<Sprite> people;
    public float speed = 1f;
    public float distance = 1f;
    private bool movingUp = true;
    private int count = 0;
    private bool react = false;

    private void Start()
    {
        foreach (Transform crowdMember in this.transform.GetComponentsInChildren<Transform>())
        {
            //this shouldnt be checked twice like this because its probably very slow
            if (crowdMember.gameObject.GetComponent<SpriteRenderer>())
            {
                //  1/2 chance to randomize the crowd member's sprite, otherwise delete them
                if (Random.Range(0, 2) == 0)
                {
                    crowdMember.gameObject.GetComponent<SpriteRenderer>().sprite = people[Random.Range(0, people.Count)];
                }
                else
                {
                    Destroy(crowdMember.gameObject);
                }
            }
        }
    }

    void Update()
    {
        if (react && count < 4)
        {
            // Move the object up or down according to the flag and the speed
            transform.Translate(Vector3.up * (movingUp ? 1 : -1) * speed * Time.deltaTime);

            // If the object has reached the maximum or minimum distance, reverse the direction and increment the count
            if (transform.position.y >= distance || transform.position.y <= -distance)
            {
                movingUp = !movingUp;
                count++;
            }
        }
        else
        {
            // Reset the react flag and the count
            react = false;
            count = 0;
        }
    }

    // The React function that can be called from other scripts
    public void React()
    {
        react = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdGenerator : MonoBehaviour
{
    [SerializeField] private List<Sprite> people;

    private void Start()
    {
        foreach (Transform crowdMember in this.transform.GetComponentsInChildren<Transform>())
        {
            //this shouldnt be checked twice like this because its probably very slow
            if (crowdMember.gameObject.GetComponent<SpriteRenderer>())
            {
                crowdMember.gameObject.GetComponent<SpriteRenderer>().sprite = people[Random.Range(0, people.Count)];
            }
        }
    }
}

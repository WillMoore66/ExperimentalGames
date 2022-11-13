using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailMovement : MonoBehaviour
{
    [SerializeField] private GameObject joint;
    int counter = 0;
    bool goingLeft = false;

    void FixedUpdate()
    {
        if (counter % 10 == 0)
        {
            goingLeft = !goingLeft;
        }
        counter++;
    }

    private void Update()
    {
        if (this.transform.parent)
        {
            //Future: tail movement should depend on how well the player is doing
            if (goingLeft)
            {
                this.transform.RotateAround(joint.transform.position, -this.transform.up, 1f);
            }
            else
            {
                this.transform.RotateAround(joint.transform.position, this.transform.up, 1f);
            }
        }
    }
}

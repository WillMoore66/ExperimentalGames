//CHANGE BEFORE PROJECT FINISH
using UnityEngine;

public class LineFollower : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] float forceStrength;
    [SerializeField] LineRenderer line;

    private int index = 0; // the current index of the line segment
    private Vector3 target; // the current target position
    private float targetDistFromPlayer;

    void Start()
    {
        // get the first target position from the line renderer
        target = line.GetPosition(index);
    }

    void Update()
    {
        //THIS IS POOR FOR PERFORMANCE BECAUSE IT ITERATES THROUGH THE WHOLE LINE TO FIND THE CLOSEST SEGMENT
        //for each segment in the line, find the closest point to the player
        for (int i = 0; i < line.positionCount-1; i++)
        {
            Vector3 potentialTarget = FindClosestPoint(player.transform.position, line.GetPosition(index), line.GetPosition(index + 1));
            float potTargetDistFromPlayer = Mathf.Abs((potentialTarget - player.transform.position).magnitude);
            targetDistFromPlayer = Mathf.Abs((target - player.transform.position).magnitude);

            if (potTargetDistFromPlayer < targetDistFromPlayer) 
            {
            target = potentialTarget;
            }

            index++;
        }
        index = 0;
        this.transform.position = target;

        Vector3 force = target - player.transform.position * forceStrength;

        //add a force to the player that moves them towards the point
        player.GetComponent<Rigidbody>().AddForce(new Vector3(force.x,0,force.z));
    }

    // Calculate the closest point using the formula from https://en.wikipedia.org/wiki/Distance_from_a_point_to_a_line#Vector_formulation
    Vector3 FindClosestPoint(Vector3 pointA, Vector3 lineStart, Vector3 lineEnd)
    {
        Vector3 lineDirection = lineEnd - lineStart;
        float t = Vector3.Dot(pointA - lineStart, lineDirection) / Vector3.Dot(lineDirection, lineDirection);
        t = Mathf.Clamp01(t); // Clamp t to [0, 1] to get the closest point on the line segment

        Vector3 closestPoint = lineStart + t * lineDirection;
        return closestPoint;
    }
}
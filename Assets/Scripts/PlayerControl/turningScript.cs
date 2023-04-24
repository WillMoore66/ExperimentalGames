using UnityEngine;

public class turningScript : MonoBehaviour
{
    // The amount of torque to apply per frame
    public float torque = 10f;

    // The direction of torque to apply per frame
    public Vector3 torqueDirection = Vector3.up;

    public float targetAngle = 90f;
    private float currentAngle = 0f;
    private Rigidbody rb;
    private bool turning;
    private bool startedTurning;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    void Update()
    {
        if (startedTurning) 
        {
            Turn();
        }
    }

    void Turn()
    {
        // Check if the current angle is less than the target angle
        if (currentAngle < targetAngle)
        {
            // Apply torque to the rigidbody
            rb.AddTorque(torqueDirection * torque);

            // Update the current angle by calculating the delta angle between the previous and current rotation
            currentAngle += Quaternion.Angle(transform.rotation, rb.rotation);
            turning = true;
        }
        else
        {
            // Stop applying torque
            torque = 0f;
            rb.angularVelocity = Vector3.zero;
            turning = false;
        }
    }
}
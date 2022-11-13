using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Camera : MonoBehaviour
{
    private Transform target;
    private CinemachineVirtualCamera cam;
    private float distanceToTarget;
    [SerializeField] private float zoomSensitivity;

    private void Start()
    {
        cam = this.GetComponent<CinemachineVirtualCamera>();
        target = cam.Follow;
    }

    void Update()
    {
        distanceToTarget = zoomSensitivity*Vector3.Distance(this.transform.position, target.transform.position);
        cam.m_Lens.FieldOfView = 70-Mathf.Clamp(distanceToTarget, 10, 45);
    }
}

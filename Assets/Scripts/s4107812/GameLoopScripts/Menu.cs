using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Menu : MonoBehaviour
{
    // Abstract parent class of menu

    [SerializeField]
    public Vector3 cameraPosition, cameraRotation;

    // Moves camera to another menu
    protected IEnumerator MoveCameraTo(Menu newMenu) {
        // I don't want to call Camera.main 400 times
        Camera theCamera = Camera.main;

        // Get lerping positions and rotations
        Vector3 priorCamPos = theCamera.transform.position;
        Vector3 priorCamRot = theCamera.transform.eulerAngles;
        Vector3 desCamPos = newMenu.cameraPosition;
        Vector3 desCamRot = newMenu.cameraRotation;

        // Flag and loop for movement
        float progress = 0f;
        while (progress < 1f) {
            // Interpolation effect
            progress = progress + (0.25f + (0.75f * progress)) * Time.deltaTime;
            // Move Camera
            theCamera.transform.position = Vector3.Lerp(priorCamPos, desCamPos, progress);
            theCamera.transform.eulerAngles = Vector3.Lerp(priorCamRot, desCamRot, progress);
            // Wait a single frame
            yield return new WaitForEndOfFrame();
        }

        // Make sure it's in the correct place
        theCamera.transform.position = desCamPos;
        theCamera.transform.eulerAngles = desCamRot;
    }
}

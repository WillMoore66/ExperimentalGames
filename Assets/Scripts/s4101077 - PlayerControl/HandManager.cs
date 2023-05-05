using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class HandManager : MonoBehaviour
{
    [SerializeField] private GameObject hand;
    [SerializeField] private GameObject dog;
    [SerializeField] float defaultHandDist;
    const int maxHandDist = 50;

    Camera cam;
    private bool followingMouse;
    private int scrubCounter;
    private Vector3 oldHandPos;


    private void Start()
    {
        cam = Camera.main;
        followingMouse = true;
        Cursor.visible = false;
        StartCoroutine("BackToMenuTimer");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Move cob to mouse position
        if (followingMouse)
        {
            int layerMask = LayerMask.GetMask("Default");
            Ray targetRay = cam.ScreenPointToRay(new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, 0));
            RaycastHit targetHit;

            if (Physics.Raycast(targetRay, out targetHit, maxHandDist, layerMask))   //shouldnt be able to hit currently selected cobs 
            {
                hand.transform.position = targetRay.GetPoint(targetHit.distance - hand.GetComponent<Renderer>().bounds.size.magnitude);

                if (hand.transform.position != oldHandPos)
                {
                scrubCounter++;
                }
            }
            else
            {
                hand.transform.position = targetRay.GetPoint(defaultHandDist);
                if (scrubCounter - 2 > 0)
                {
                    scrubCounter -= 2;
                }
            }
        }

        //Debug.Log(scrubCounter);

        if (scrubCounter > 80)
        {
            //play dog happy animation
            Debug.Log("the dog is very happy!");
            dog.transform.Rotate(10, 5, 2);
        }
        else
        {
        dog.transform.rotation = Quaternion.Euler(-15, -180, 0);
        }

        oldHandPos = hand.transform.position;
    }

    IEnumerator BackToMenuTimer()
    {
        yield return new WaitForSeconds(10f);
        Cursor.visible= true;
        SceneManager.LoadScene("DeclanSceneButActuallyWorks");
    }
}

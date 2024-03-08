using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerInteractionController : MonoBehaviour
{
    void Update()
    {
        PhysicsRaycasts();
    }
    void PhysicsRaycasts()
    {
        Vector3 centreOfScreen =
        new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0);
        float distanceToFireRay = 20;
        Ray centreOfScreenRay =
        Camera.main.ScreenPointToRay(centreOfScreen);
        RaycastHit hit;
        if (Physics.Raycast(centreOfScreenRay, out hit, distanceToFireRay))
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Raycast hit: " + hit.transform.name);
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadLook : MonoBehaviour
{

    public float MaxAngle;
    public float MinAngle;
    public Transform HeadObject, TargetObject, HeadForward;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Executes after the animator
    void LateUpdate()
    {
        Vector3 Direction = TargetObject.position - HeadObject.position;
        float angle = Vector3.SignedAngle(Direction, HeadForward.forward, HeadForward.up);
        if(angle < MaxAngle && angle > MinAngle)
        {
            HeadObject.LookAt(TargetObject);

            HeadObject.Rotate(0, 180, 0);
        }

      
    }
}

// Code Reference: https://www.youtube.com/watch?v=ZodFnkBjSeY&t=19s
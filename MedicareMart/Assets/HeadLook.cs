using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadLook : MonoBehaviour
{
    public Transform HeadObject, TargetObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Executes after the animator
    void LateUpdate()
    {
       HeadObject.LookAt(TargetObject);

       HeadObject.Rotate(0, 180, 0);
    }
}

// Code Reference: https://www.youtube.com/watch?v=ZodFnkBjSeY&t=19s
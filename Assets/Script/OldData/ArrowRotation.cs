using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowRotation : MonoBehaviour
{
    public float rotationArrowSpeedX;
    public float rotationArrowSpeedY;
    public float rotationArrowSpeedZ;
    
    void Update()
    {
        transform.Rotate(new Vector3(rotationArrowSpeedX, rotationArrowSpeedY, rotationArrowSpeedZ));
    }
}

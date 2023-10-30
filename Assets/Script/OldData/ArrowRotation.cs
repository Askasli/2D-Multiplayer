using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowRotation : MonoBehaviour
{
    public float rotationArrowSpeedX;
    public float rotationArrowSpeedY;
    public float rotationArrowSpeedZ;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(rotationArrowSpeedX, rotationArrowSpeedY, rotationArrowSpeedZ));
    }
}

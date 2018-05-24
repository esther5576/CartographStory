using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatController : MonoBehaviour
{
    //Speed calculations
    private float currentSpeed;
    private Vector3 lastPosition;

    void FixedUpdate()
    {
        
    }

    //Calculate the current speed in m/s
    private void CalculateSpeed()
    {
        //Calculate the distance of the Transform Object between the fixedupdate calls with 
        currentSpeed = (transform.position - lastPosition).magnitude / Time.deltaTime;

        //Save the position for the next update
        lastPosition = transform.position;
    }

    public float CurrentSpeed
    {
        get
        {
            return this.currentSpeed;
        }
    }
}

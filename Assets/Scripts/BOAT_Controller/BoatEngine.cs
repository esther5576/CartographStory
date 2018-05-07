using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatEngine : MonoBehaviour
{
    private float WaterJetRotation_Y = 0;
    public Transform waterJetTransform;
    public float accelerationSpeed = 200000;

    private Rigidbody boatRigibody;

    // Use this for initialization
    void Start()
    {
        boatRigibody = this.GetComponent<Rigidbody>();
    }


    public void Update()
    {
        //Steer left
        if (Input.GetKey(KeyCode.Q))
        {
            WaterJetRotation_Y = waterJetTransform.localEulerAngles.y + 2f;

            if (WaterJetRotation_Y > 30f && WaterJetRotation_Y < 270f)
            {
                WaterJetRotation_Y = 30f;
            }

            Vector3 newRotation = new Vector3(0f, WaterJetRotation_Y, 0f);

            waterJetTransform.localEulerAngles = newRotation;
        }
        //Steer right
        else if (Input.GetKey(KeyCode.D))
        {
            WaterJetRotation_Y = waterJetTransform.localEulerAngles.y - 2f;

            if (WaterJetRotation_Y < 330f && WaterJetRotation_Y > 90f)
            {
                WaterJetRotation_Y = 330f;
            }

            Vector3 newRotation = new Vector3(0f, WaterJetRotation_Y, 0f);

            waterJetTransform.localEulerAngles = newRotation;


            float v = Input.GetAxis("Vertical");
            boatRigibody.AddForce(waterJetTransform.forward * v * accelerationSpeed * Time.deltaTime);
        }
    }
}

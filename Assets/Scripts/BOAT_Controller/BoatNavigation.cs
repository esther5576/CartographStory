using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatNavigation: MonoBehaviour
{
    public float turnSpeed = 10000f;
    public float accelerationSpeed = 200000;

    private Rigidbody boatRigibody;

	// Use this for initialization
	void Start ()
    {
        boatRigibody = this.GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        boatRigibody.AddTorque(0f, h * turnSpeed * Time.deltaTime, 0f);
        boatRigibody.AddForce(transform.forward * v * accelerationSpeed * Time.deltaTime);
	}
}

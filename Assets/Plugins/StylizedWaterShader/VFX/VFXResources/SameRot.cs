using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SameRot : MonoBehaviour
{

    public Transform target;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        this.transform.localEulerAngles = new Vector3(90, target.localEulerAngles.y, target.localEulerAngles.z);
    }
}

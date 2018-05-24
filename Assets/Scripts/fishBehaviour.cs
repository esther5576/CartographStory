using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fishBehaviour : MonoBehaviour {

    public float vitesse = 1.2f;
    public Transform target;
    public Transform fish;
    public int rayon;
    float distance;
    

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {

        float distance = Vector3.Distance(target.position, fish.position);
        

        if (distance > rayon)
        {
            transform.LookAt(target);
            transform.position += transform.forward * vitesse * Time.deltaTime;
        }
        else
        {
            Vector3 oldposition = transform.position;

            transform.RotateAround(target.position, Vector3.up, 20 * Time.deltaTime);

            Vector3 newposition = transform.position;
            var direction = newposition - oldposition;
            direction = Vector3.RotateTowards(transform.forward, direction, 3 * Time.deltaTime, 1);
            
            transform.forward = direction;
        }
		
	}
}

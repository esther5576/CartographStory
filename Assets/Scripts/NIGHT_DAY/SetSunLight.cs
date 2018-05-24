using UnityEngine;
using System.Collections;

public class SetSunLight : MonoBehaviour
{
	public Transform stars;

	// Use this for initialization
	void Start () 
	{

	}


	// Update is called once per frame
	void Update () 
	{

		stars.transform.rotation = transform.rotation;

	}
}

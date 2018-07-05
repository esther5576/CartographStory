using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterFollowPlayer : MonoBehaviour
{
    public GameObject myPlayer;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        this.transform.position = new Vector3(myPlayer.transform.position.x, this.transform.position.y, myPlayer.transform.position.z);
    }
}

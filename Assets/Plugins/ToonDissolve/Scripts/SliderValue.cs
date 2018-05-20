using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderValue : MonoBehaviour {
    public GameObject shield;
    public ParticleSystem effect;
    bool played = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        shield.GetComponent<Renderer>().material.SetFloat("_DisAmount", GetComponent<Slider>().value);
       
        if(GetComponent<Slider>().value > 0.4 && !played)
        {
            played = true;
            effect.Play();
        }
        if (GetComponent<Slider>().value < 0.2)
        {
            played = false;
        }
    }
}

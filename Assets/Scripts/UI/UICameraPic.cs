using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICameraPic : MonoBehaviour
{

    public Camera PictureCamera;
    public float zoomMax;
    public float zoomMin;
    public Slider zoomSlide;


	// Use this for initialization
	void Start ()
    {
        
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(PictureCamera.enabled)
        {
            if((PictureCamera.fieldOfView - zoomMin) == 0)
            {
                zoomSlide.value = 0;
            }
            else
            {
                zoomSlide.value = ((PictureCamera.fieldOfView - zoomMin) / (zoomMax - zoomMin));
            }
        }
    }
}

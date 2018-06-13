using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour
{

    #region Outside camera controls
    UnityStandardAssets.Cameras.AutoCam _myAutoCam;
    public float waitTime;
    public float maxWaitTime = 5f;
    #endregion

    #region Change Between cameras
    public GameObject OutsideCamera;
    public GameObject PictureCamera;
    public BoatNavigation BoatControls;

    public bool activateCam = false;
    #endregion

    // Use this for initialization
    void Start ()
    {
        _myAutoCam = OutsideCamera.GetComponent<UnityStandardAssets.Cameras.AutoCam>();
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
        #region outisde camera controller
        if (OutsideCamera.GetComponentInChildren<Camera>().enabled == true)
        {
            if (CrossPlatformInputManager.GetAxis("Mouse X") == 0)
            {
                if (waitTime < maxWaitTime)
                {
                    waitTime += Time.deltaTime;
                }
                else
                {
                    _myAutoCam.followPlayer = true;

                }
            }
            else
            {
                _myAutoCam.followPlayer = false;

                waitTime = 0;
            }
        }
        #endregion

        if(Input.GetKeyDown(KeyCode.C))
        {
            activateCam = !activateCam;

            if(activateCam)
            {
                OutsideCamera.GetComponentInChildren<Camera>().enabled = false;
                PictureCamera.SetActive(true);
                BoatControls.enabled = false;
            }
            else
            {
                OutsideCamera.GetComponentInChildren<Camera>().enabled = true;
                PictureCamera.SetActive(false);
                BoatControls.enabled = true;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableDisableMouse : MonoBehaviour
{
    public CanvasGroup cameraUIPIC;
    public GameObject MapPrefab;
    public GameObject JournalPrefab;
	// Use this for initialization
	void Start ()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    // Update is called once per frame
    void Update ()
    {
        if (cameraUIPIC.alpha == 1 || MapPrefab.activeSelf || JournalPrefab.activeSelf)
        {
            Debug.Log("cacaaaaaaaaaaaaa");
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        } else if (cameraUIPIC.alpha == 0 && !MapPrefab.activeSelf && !JournalPrefab.activeSelf)
        {
            Debug.Log("pipiiiiiiiiiiii");
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
	}
}

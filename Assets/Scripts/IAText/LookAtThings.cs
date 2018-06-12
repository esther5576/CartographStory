﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;
using DG.Tweening;
using System.Linq;

public class LookAtThings : MonoBehaviour
{
    Camera cam;
    MachineCall myMachineCall;
    PictureSystem myPictureSystem;

    void Start()
    {
        cam = GetComponent<Camera>();
        myMachineCall = GameObject.Find("GAME_MANAGER").GetComponent<MachineCall>();
        myPictureSystem = GetComponent<PictureSystem>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Ray ray = cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
            RaycastHit hit;
            Debug.DrawRay(this.transform.position, this.transform.forward, Color.red);
            if (Physics.Raycast(ray, out hit))
            {
                Debug.DrawLine(this.transform.position, hit.transform.position, Color.green);
                print("I'm looking at " + hit.transform.name);
                myPictureSystem.IDOfIlsandToAdd = hit.transform.GetComponent<ObjectsID>().ID;

                if (!myMachineCall.objectsSeen.Contains(hit.transform.GetComponent<ObjectsID>().ObjectDescription))
                {
                    myMachineCall.objectsSeen.Add(hit.transform.GetComponent<ObjectsID>().ObjectDescription);
                }

                myPictureSystem.TakePic();
            }
            else
            {
                print("I'm looking at nothing!");
            }
        }

    }
}

using System.Collections;
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
                ObjectsID ScriptOnItem = hit.transform.GetComponent<ObjectsID>();
                myPictureSystem.IDOfIlsandToAdd = ScriptOnItem.ID;

                int randomDescription = Random.Range(0, ScriptOnItem.ObjectDescription.Count);
                myPictureSystem.TakePic(ScriptOnItem.ObjectDescription[randomDescription]);
            }
            else
            {
                print("I'm looking at nothing!");
            }
        }

    }
}

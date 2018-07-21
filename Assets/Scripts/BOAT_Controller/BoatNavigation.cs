using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BoatNavigation: MonoBehaviour
{
    public float turnSpeed = 10000f;
    public float accelerationSpeed = 200000;
    [HideInInspector]
    public float actualSpeed = 0;
    
    private Rigidbody boatRigibody;

    public List<Transform> _l_partsToTurn;
    public Vector3 rightTurn = new Vector3(0,-40,0);
    public Vector3 leftTurn = new Vector3(0, 40, 0);
    public float voileSpeedRot = 1;

    public List<Transform> _l_voiles;


    // Use this for initialization
    void Start ()
    {
        boatRigibody = this.GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if(v > 0 && actualSpeed < 1)
        {
            actualSpeed += v * Time.deltaTime;
        }
        else if (v < 0 && actualSpeed > 0)
        {
            actualSpeed += v * Time.deltaTime;
        }

        if(actualSpeed > 1)
        {
            actualSpeed = 1;
        }

        if (actualSpeed < 0)
        {
            actualSpeed = 0;
        }

        foreach (Transform t in _l_voiles)
        {
            if (actualSpeed < 0.1f)
            {
                t.localScale = new Vector3(1, 0.1f, 1);
            }
            else
            {
                t.localScale = new Vector3(1, actualSpeed, 1);
            }
        }

        boatRigibody.AddTorque(0f, h * turnSpeed * Time.deltaTime, 0f);
        boatRigibody.AddForce(transform.forward * actualSpeed * accelerationSpeed * Time.deltaTime);

        //Debug.Log(boatRigibody.velocity.magnitude);

        if (h > 0)
        {
            for (int i = 0; i < _l_partsToTurn.Count; i++)
            {
                DOTween.Kill("BoatRotLeft" + 1);
                DOTween.Kill("BoatRotCenter" + 1);
                _l_partsToTurn[i].transform.DOLocalRotate(rightTurn, voileSpeedRot).SetEase(Ease.OutQuart).SetId("BoatRotRight" + i);
            }
        }
        else if (h < 0)
        {
            for (int i = 0; i < _l_partsToTurn.Count; i++)
            {
                DOTween.Kill("BoatRotRight" + 1);
                DOTween.Kill("BoatRotCenter" + 1);
                _l_partsToTurn[i].transform.DOLocalRotate(leftTurn, voileSpeedRot).SetEase(Ease.OutQuart).SetId("BoatRotLeft" + i);
            }
        }
        else
        {
            for (int i = 0; i < _l_partsToTurn.Count; i++)
            {
                DOTween.Kill("BoatRotLeft" + 1);
                DOTween.Kill("BoatRotRight" + 1);
                _l_partsToTurn[i].transform.DOLocalRotate(Vector3.zero, voileSpeedRot).SetEase(Ease.OutQuart).SetId("BoatRotCenter" + i);
            }
        }

        if(Input.GetKeyDown(KeyCode.Keypad1))
        {
            accelerationSpeed = accelerationSpeed/2;
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            accelerationSpeed = accelerationSpeed * 2;
        }
    }
}

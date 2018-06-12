using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BoatNavigation: MonoBehaviour
{
    public float turnSpeed = 10000f;
    public float accelerationSpeed = 200000;

    public float maxSpeed = 8f;

    private Rigidbody boatRigibody;

    public List<Transform> _l_partsToTurn;
    public Vector3 rightTurn = new Vector3(0,-40,0);
    public Vector3 leftTurn = new Vector3(0, 40, 0);
    public float voileSpeedRot = 1;

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

        boatRigibody.AddTorque(0f, h * turnSpeed * Time.deltaTime, 0f);
        boatRigibody.AddForce(transform.forward * v * accelerationSpeed * Time.deltaTime);

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
	}
}

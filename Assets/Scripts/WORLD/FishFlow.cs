using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FishFlow : MonoBehaviour
{
    public float vitesse = 1.2f;
    public Texture2D tex;

    float timeOutsidePath = 1;
    public float timeOutsidePathMax = 1;
    public int state = 0;
    public int randomnessChanceXY = 100;
    float maxRandomDirXY = 10;
    float minRandomDirXY = -10;

    float timeOutsidePathZ = 1;
    public float timeOutsidePathMaxZ = 1;
    public int randomnessChanceZ = 100;
    float maxRandomDirZ = 50;
    float minRandomDirZ = 0;
    float minminRandomDirZ = -50;
    float maxUnderPossible = -30.0f;

    // Use this for initialization
    void Start ()
    {

        

    }
	
	// Update is called once per frame

	void Update ()
    {
        if (state == 0)
        {
            // 1) copier la position du poisson en x et z et multiplier par 10 (entier)
            Vector3 posFish = new Vector3(transform.position.x * 10, transform.position.y, transform.position.z * 10);
            int coordX = (int)transform.position.x * 10;
            int coordZ = (int)transform.position.z * 10;
            print(coordX + " " + coordZ);

            // 2) récuppérer la valeur du pixel correspondant

            Color c = tex.GetPixel(coordX, coordZ);
            print(c);
            if (c.r > 0.48f && c.r < 0.52f && c.b > 0.48f && c.b < 0.52f)
            {

            }
            else
            {
                // modifier le transform.forward à l'aide de la couleur
                // R (x): 0 - 128 correspond à -128 - 0, 128 à 256 correzspond à 0 - 128
                // B (z): 

                transform.forward = new Vector3(c.r - 0.5f, 0.0f, c.b - 0.5f);
            }

            // avancer
            transform.position += transform.forward * vitesse * Time.deltaTime;

            //test each frame if change of state
            int i = Random.Range(0, randomnessChanceXY);
            if(i == randomnessChanceXY/2)
            {
                Debug.Log("IM FREEE");
                DOTween.Kill("changeXY");
                DOTween.Kill("changeZ");
                timeOutsidePath = timeOutsidePathMax;
                state = 1;
                float y = Random.Range(minRandomDirXY, maxRandomDirXY);
                this.transform.DOLocalRotate(new Vector3(this.transform.localEulerAngles.x, this.transform.localEulerAngles.y + y, this.transform.localEulerAngles.z), 1).SetId("changeXY");
            }

            int z = Random.Range(0, randomnessChanceZ);
            if(z == randomnessChanceZ/2)
            {
                Debug.LogWarning("IM GOING DOOOWN");
                DOTween.Kill("changeXY");
                DOTween.Kill("changeZ");
                timeOutsidePathZ = timeOutsidePathMaxZ;
                state = 2;

                float zz = 0;
                if (this.transform.position.y < maxUnderPossible)
                {
                    zz = Random.Range(minminRandomDirZ, minRandomDirZ);
                }
                else
                {
                    zz = Random.Range(minRandomDirZ, maxRandomDirZ);
                }
                
                this.transform.DOLocalRotate(new Vector3(zz, this.transform.localEulerAngles.y, this.transform.localEulerAngles.z), 1).SetId("changeZ");
            }

           
        }
        else if(state == 1)
        {
            //Fish doesn't follow flow for a few seconds
            timeOutsidePath -= Time.deltaTime;
            transform.position += transform.forward * vitesse * Time.deltaTime;

            if(timeOutsidePath < 0)
            {
                state = 0;
            }
        }
        else if (state == 2)
        {
            //Fish doesn't follow flow for a few seconds
            timeOutsidePathZ -= Time.deltaTime;
            transform.position += transform.forward * vitesse * Time.deltaTime;

            if (timeOutsidePathZ < 0)
            {
                state = 0;
            }
        }
    }
}

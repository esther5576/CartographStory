using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishFlow : MonoBehaviour {



    public float vitesse = 1.2f;
    public Texture2D tex;
    


    // Use this for initialization
    void Start () {

        

    }
	
	// Update is called once per frame

	void Update () {

        // 1) copier la position du poisson en x et z et multiplier par 10 (entier)
        Vector3 posFish = new Vector3(transform.position.x * 10, transform.position.y, transform.position.z * 10);
        int coordX = (int)transform.position.x * 10;
        int coordZ = (int)transform.position.z * 10;
        print(coordX + " " + coordZ);

        // 2) récuppérer la valeur du pixel correspondant

        Color c = tex.GetPixel(coordX, coordZ);
        print(c);
        if( c.r > 0.48f && c.r < 0.52f && c.b > 0.48f && c.b < 0.52f )
        {

        }
        else
        {
            // modifier le transform.forward à l'aide de la couleur
            // R (x): 0 - 128 correspond à -128 - 0, 128 à 256 correzspond à 0 - 128
            // B (z): 

            transform.forward = new Vector3(c.r - 0.5f, 0.0f, c.b - 0.5f);
        }


        // 
        // avancer

        transform.position += transform.forward * vitesse * Time.deltaTime;



    }
}

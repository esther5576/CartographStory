using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishBancActivator : MonoBehaviour
{
    List<GameObject> myfishes = new List<GameObject>();
	// Use this for initialization
	void Start ()
    {
        foreach (Transform t in this.transform.GetComponentsInChildren<Transform>())
        {
            if (t == this.transform)
            {

            }
            else
            {
                myfishes.Add(t.gameObject);
                t.gameObject.SetActive(false);
            }
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            foreach(GameObject w in myfishes)
            {
                w.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            foreach (GameObject w in myfishes)
            {
                w.SetActive(false);
            }
        }
    }
}

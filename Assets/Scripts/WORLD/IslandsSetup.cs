using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandsSetup : MonoBehaviour
{
    
	// Use this for initialization
	void Start ()
    {
		foreach(ObjectsID obj in FindObjectsOfType<ObjectsID>())
        {
            if (obj.IsIsland == true)
            {
                Island island = new Island();
                island.ID = obj.ID;
                DataManager.AllIslands.Add(island);
            }
        }

        Debug.Log("ALL ISLANDS : " + DataManager.AllIslands.Count);
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}

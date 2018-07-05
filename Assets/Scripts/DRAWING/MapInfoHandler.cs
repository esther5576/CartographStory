using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MapInfoHandler : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler
{
	public MapManager ManagerScript;
	public int ID;
	public Image MapSprite;
	private bool DragOn;

	// Use this for initialization
	void Start ()
	{
		if (MapSprite == null) MapSprite = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (DragOn)
		{
			transform.position = Input.mousePosition;
		}

		if (Input.GetMouseButtonUp(0))
		{
			DragOn = false;
		}
	}
	
	public void OnPointerDown(PointerEventData data)
	{
		if (Input.GetMouseButton(0))
		{
			DragOn = true;
		}
	}

    public void OnPointerEnter(PointerEventData eventData)
    {
        ManagerScript.NarrativeTextMap.text = DataManager.AllIslands[ID].NarrativText;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
	public List<Sprite> AllMapTextures;
	public List<MapInfoHandler> AllMapScripts;

	public int MapsUsed;

	public Drawable DrawManager;

	public Transform MapContainer;
	
	[Header("Buttons")]
	public Button ResetButton;
	public Button CloseBigMapButton;
	public Button CloseDrawButton;
	public Button MapValidationButton;


	[Header("GameObjects")]
	public GameObject DrawSystemParent;
	public GameObject BigMapParent;
	public GameObject MapPrefab;


	// Use this for initialization
	void Awake ()
	{
		ResetButton.onClick.AddListener(ResetDraw);
		CloseBigMapButton.onClick.AddListener(CloseMap);
		CloseDrawButton.onClick.AddListener(CloseDraw);
		MapValidationButton.onClick.AddListener(ValidateDraw);
		DrawManager.Reset_Canvas_On_Play = false;
		DrawManager.CanDraw = false;
		MapsUsed = 0;
	}

	void ResetDraw ()
	{
		DrawManager.ResetCanvas();
	}

	public void CreateNewDraw ()
	{
		if (MapsUsed < AllMapTextures.Count - 1)
		{
			StartDraw(MapsUsed);
			GameObject NewMap = Instantiate(MapPrefab, MapContainer);
			AllMapScripts.Add(NewMap.GetComponent<MapInfoHandler>());
			AllMapScripts[MapsUsed].ID = MapsUsed;
			AllMapScripts[MapsUsed].ManagerScript = this;
			AllMapScripts[MapsUsed].MapSprite.sprite = AllMapTextures[MapsUsed];
			MapsUsed++;
			DrawManager.ResetCanvas();
			CloseDrawButton.interactable = false;
		}
		else
		{
			Debug.Log("No more map avaible");
		}
	}

	public void StartDraw (int ID)
	{
		DrawManager.InitDrawer(AllMapTextures[ID]);
		DrawManager.CanDraw = true;
		OpenDraw();
		CloseMap();
	}

	public void ValidateDraw ()
	{
		CloseDrawButton.interactable = true;
		DrawManager.CanDraw = false;
		OpenMap();
		CloseDraw();
	}

	public void OpenDraw ()
	{
		DrawSystemParent.SetActive(true);
	}

	public void CloseDraw ()
	{
		DrawSystemParent.SetActive(false);
	}

	public void OpenMap ()
	{
		BigMapParent.SetActive(true);
	}

	public void CloseMap ()
	{
		BigMapParent.SetActive(false);
	}

	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.R))
		{
			CreateNewDraw();
		}

		if (Input.GetKeyDown(KeyCode.M))
		{
			if (BigMapParent.activeInHierarchy) CloseMap();
			else OpenMap();
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
	public List<MapInfoHandler> AllMapScripts;

	public int MapsUsed;

	public Drawable DrawManager;

	public Transform MapContainer;
	
	[Header("Buttons")]
	public Button ResetButton;
	public Button CloseBigMapButton;
	public Button CloseJournalButton;
	public Button MapValidationButton;


	[Header("GameObjects")]
	public GameObject DrawSystemParent;
	public GameObject BigMapParent;
	public GameObject MapPrefab;


	// Use this for initialization
	void Awake ()
	{
        DataManager.AllIslands.Add(new Island());
        DataManager.AllIslands.Add(new Island());
        ResetButton.onClick.AddListener(ResetDraw);
		CloseBigMapButton.onClick.AddListener(CloseMap);
        CloseJournalButton.onClick.AddListener(CloseJournal);
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
        OpenJournal();
		CloseMap();
	}

	public void ValidateDraw ()
	{
        CloseJournalButton.interactable = true;
		DrawManager.CanDraw = false;
        DataManager.AllIslands[MapsUsed]
		OpenMap();
        CloseJournal();
	}


	public void OpenJournal ()
	{
		DrawSystemParent.SetActive(true);
	}

	public void CloseJournal ()
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

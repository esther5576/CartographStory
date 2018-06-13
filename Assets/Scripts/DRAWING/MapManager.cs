using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
	public List<MapInfoHandler> AllMapScripts;

    public List<Image> AllPicturesRenderersOrigin;
    public List<Image> AllPicturesRenderersWithText;
    public Text NarrativeText;
    public Text NarrativeTextMap;
    public Drawable DrawManager;

	public Transform MapContainer;
	
	[Header("Buttons")]
	public Button ResetButton;
	public Button CloseBigMapButton;
	public Button CloseJournalButton;
	public Button MapValidationButton;
	public Button DrawEditButton;


    [Header("GameObjects")]
	public GameObject DrawSystemParent;
	public GameObject BigMapParent;
	public GameObject MapPrefab;

    int IslandSelected;

    [Header("CameraFix")]
    public GameObject CameraToDesactivate;
    public CameraManager ScriptToDesactivate;

	// Use this for initialization
	void Awake ()
	{
        ResetButton.onClick.AddListener(ResetDraw);
        ResetButton.interactable = false;
        CloseBigMapButton.onClick.AddListener(CloseMap);
        CloseBigMapButton.interactable = true;
        CloseJournalButton.onClick.AddListener(CloseJournal);
        CloseJournalButton.interactable = true;
        MapValidationButton.onClick.AddListener(ValidateDraw);
        MapValidationButton.interactable = false;
        DrawEditButton.onClick.AddListener(StartDraw);
		DrawManager.Reset_Canvas_On_Play = false;
		DrawManager.CanDraw = false;
	}

	void ResetDraw ()
	{
		DrawManager.ResetCanvas();
	}

	public void StartDraw ()
	{
		DrawManager.CanDraw = true;
        ResetButton.interactable = true;
        DrawEditButton.interactable = false;
        MapValidationButton.interactable = true;
        if (DataManager.AllIslands[IslandSelected].Drawed)
        {
            DrawManager.InitDrawer(DataManager.AllIslands[IslandSelected].Drawing);
        }
        else
        {
            DrawManager.ResetCanvas();
        }
	}

	public void ValidateDraw ()
	{
		DrawManager.CanDraw = false;
        ResetButton.interactable = false;
        DrawEditButton.interactable = true;
        MapValidationButton.interactable = false;
        if (!DataManager.AllIslands[IslandSelected].Drawed)
        {
            DataManager.AllIslands[IslandSelected].Drawing = DrawManager.SpriteShowed.sprite;
            DataManager.AllIslands[IslandSelected].Drawed = true;
            CreateDrawInstanceOnMap();
            CloseJournal();
            OpenMap();
        }
        else
        {
            DataManager.AllIslands[IslandSelected].Drawing = DrawManager.SpriteShowed.sprite;
            AllMapScripts[IslandSelected].MapSprite.sprite = DataManager.AllIslands[IslandSelected].Drawing;
        }
    }

    public void CreateDrawInstanceOnMap ()
    {
        GameObject NewMap = Instantiate(MapPrefab, MapContainer);
        AllMapScripts.Add(NewMap.GetComponent<MapInfoHandler>());
        AllMapScripts[IslandSelected].ID = IslandSelected;
        AllMapScripts[IslandSelected].ManagerScript = this;
        AllMapScripts[IslandSelected].MapSprite.sprite = DataManager.AllIslands[IslandSelected].Drawing;
    }

	public void OpenJournal ()
	{
		DrawSystemParent.SetActive(true);
        SetJournal(IslandSelected);
        CameraToDesactivate.SetActive(false);
        ScriptToDesactivate.enabled = false;
        //Cursor.visible = true;
        //Cursor.lockState = CursorLockMode.None;
    }

	public void CloseJournal ()
	{
		DrawSystemParent.SetActive(false);
        CameraToDesactivate.SetActive(true);
        ScriptToDesactivate.enabled = true;
       // Cursor.visible = false;
       // Cursor.lockState = CursorLockMode.Locked;
    }

	public void OpenMap ()
	{
		BigMapParent.SetActive(true);
        CameraToDesactivate.SetActive(false);
        ScriptToDesactivate.enabled = false;
       // Cursor.visible = true;
       // Cursor.lockState = CursorLockMode.None;
    }

	public void CloseMap ()
	{
		BigMapParent.SetActive(false);
        CameraToDesactivate.SetActive(true);
        ScriptToDesactivate.enabled = true;
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
    }

    public void SetJournal (int PageNumber)
    {
        if (DataManager.AllIslands[PageNumber].NarrativText == "Aucun texte chargé")
        {
            for (int i = 0; i < AllPicturesRenderersOrigin.Count; i++)
            {
                if (i < DataManager.AllIslands[PageNumber].Pictures.Count)
                {
                    AllPicturesRenderersOrigin[i].gameObject.SetActive(true);
                    AllPicturesRenderersOrigin[i].sprite = DataManager.AllIslands[PageNumber].Pictures[i];
                }
                else
                {
                    AllPicturesRenderersOrigin[i].gameObject.SetActive(false);
                }
            }

            for (int i = 0; i < AllPicturesRenderersWithText.Count; i++)
                AllPicturesRenderersWithText[i].gameObject.SetActive(false);

            NarrativeText.gameObject.SetActive(false);
        }
        else
        {
            for (int i = 0; i < AllPicturesRenderersOrigin.Count; i++)
            {
                if (i < DataManager.AllIslands[PageNumber].Pictures.Count)
                {
                    AllPicturesRenderersWithText[i].gameObject.SetActive(true);
                    AllPicturesRenderersWithText[i].sprite = DataManager.AllIslands[PageNumber].Pictures[i];
                }
                else
                {
                    AllPicturesRenderersWithText[i].gameObject.SetActive(false);
                }
            }

            for (int i = 0; i < AllPicturesRenderersWithText.Count; i++)
                AllPicturesRenderersOrigin[i].gameObject.SetActive(false);

            NarrativeText.text = DataManager.AllIslands[PageNumber].NarrativText;
            NarrativeText.gameObject.SetActive(true);
        }

        if (DataManager.AllIslands[PageNumber].Drawing != null)
            DrawManager.SpriteShowed.sprite = DataManager.AllIslands[PageNumber].Drawing;
        else
            DrawManager.InitDrawer(DrawManager.reset_sprite);
    }

	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.J))
		{
            if (DrawSystemParent.activeInHierarchy)
            {
                CloseJournal();
            }
            else
            {
                OpenJournal();
            }

        }

		if (Input.GetKeyDown(KeyCode.M))
		{
			if (BigMapParent.activeInHierarchy)
            {
                CloseMap();
            }
            else
            {
                OpenMap();
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) IslandSelected = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2)) IslandSelected = 1;
        if (Input.GetKeyDown(KeyCode.Alpha3)) IslandSelected = 2;

    }

    public void RemoveIslandPicture(int ID)
    {
        DataManager.AllIslands[IslandSelected].Pictures.RemoveAt(ID);
        if (DataManager.AllIslands[IslandSelected].NarrativText == "Aucun texte chargé")
        {
            AllPicturesRenderersOrigin[ID].gameObject.SetActive(false);
        }
        else
        {
            AllPicturesRenderersWithText[ID].gameObject.SetActive(false);
        }
    }
}

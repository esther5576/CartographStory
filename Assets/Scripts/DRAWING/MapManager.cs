﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
	public List<MapInfoHandler> AllMapScripts;
    public MachineCall MachineCallScript;
    public List<PictureManager> AllPicturesRenderersOrigin;
    public List<PictureManager> AllPicturesRenderersWithText;
    public Text NarrativeText;
    public Text NarrativeTextMap;
    public Drawable DrawManager;
	public Transform MapContainer;
    public Sprite FondWithText;
    public Sprite FondWithoutText;
    public SpriteRenderer Fond;

	[Header("Buttons")]
	public Button ResetButton;
	public Button CloseBigMapButton;
	public Button CloseJournalButton;
	public Button MapValidationButton;


    [Header("GameObjects")]
	public GameObject DrawSystemParent;
	public GameObject BigMapParent;
	public GameObject MapPrefab;

    public int IslandSelected;

    [Header("CameraFix")]
    public GameObject CameraToDesactivate;
    public CameraManager ScriptToDesactivate;

    public GameObject player;

	// Use this for initialization
	void Awake ()
	{
        ResetButton.onClick.AddListener(ResetDraw);
        ResetButton.interactable = true;
        CloseBigMapButton.onClick.AddListener(CloseMap);
        CloseBigMapButton.interactable = true;
        CloseJournalButton.onClick.AddListener(CloseJournal);
        CloseJournalButton.interactable = true;
        MapValidationButton.onClick.AddListener(ValidateDraw);
        MapValidationButton.interactable = true;
		DrawManager.Reset_Canvas_On_Play = false;
		DrawManager.CanDraw = true;
        StartCoroutine(CreateAllMaps());
	}

    IEnumerator CreateAllMaps ()
    {
        yield return new WaitForEndOfFrame();
        for (int i = 0; i < DataManager.AllIslands.Count; i++)
        {
            GameObject NewMap = Instantiate(MapPrefab, MapContainer);
            AllMapScripts.Add(NewMap.GetComponent<MapInfoHandler>());
            AllMapScripts[i].ID = i;
            AllMapScripts[i].ManagerScript = this;
            AllMapScripts[i].gameObject.SetActive(false);
        }
    }

	void ResetDraw ()
	{
		DrawManager.ResetCanvas();
        DataManager.AllIslands[IslandSelected].Drawing = DrawManager.SpriteShowed.sprite;
        DataManager.AllIslands[IslandSelected].TextureDraw = DrawManager.drawable_texture;
        DataManager.AllIslands[IslandSelected].DrawStarted = true;
	}

	public void StartDraw ()
	{
        if (DataManager.AllIslands[IslandSelected].DrawStarted)
        {
            DrawManager.InitDrawer(DataManager.AllIslands[IslandSelected].Drawing, DataManager.AllIslands[IslandSelected].TextureDraw);
        }
        else
        {
            ResetDraw();
        }
	}

	public void ValidateDraw ()
	{
        DataManager.AllIslands[IslandSelected].Drawing = DrawManager.SpriteShowed.sprite;
        DataManager.AllIslands[IslandSelected].DrawValidated = true;
        
        CreateDrawInstanceOnMap();
        OpenMap();
        SetNarrativText();
    }

    public void SetNarrativText ()
    {
        if (DataManager.AllIslands[IslandSelected].DrawValidated)
        {
            if (DataManager.AllIslands[IslandSelected].PicturesNarratives.Count > 0)
            {
                string AllText = "";
                foreach (string Description in DataManager.AllIslands[IslandSelected].PicturesNarratives)
                    AllText += Description + "\n";
                DataManager.AllIslands[IslandSelected].NarrativText = AllText;
            }
            else
            {
                DataManager.AllIslands[IslandSelected].NarrativText = "Analyzing data ... Brrzzt ...";
            }
        }

        NarrativeText.text = DataManager.AllIslands[IslandSelected].NarrativText;
        NarrativeTextMap.text = DataManager.AllIslands[IslandSelected].NarrativText;
    }

    public void CreateDrawInstanceOnMap ()
    {
       AllMapScripts[IslandSelected].MapSprite.sprite = DataManager.AllIslands[IslandSelected].Drawing;
       AllMapScripts[IslandSelected].gameObject.SetActive(true);
    }

	public void OpenJournal ()
	{
        CheckNavigationCameraDeActivation();
        DrawSystemParent.SetActive(true);
        SetNarrativText();
        SetJournal(IslandSelected);
        CloseMap();
    }

    public void CloseJournal ()
	{
        DrawSystemParent.SetActive(false);
        CheckNavigationCameraActivation();
    }

    public void OpenMap ()
	{
        CheckNavigationCameraDeActivation();
        BigMapParent.SetActive(true);
        CloseJournal();
    }

	public void CloseMap ()
	{
		BigMapParent.SetActive(false);
        CheckNavigationCameraActivation();
    }

    public void CheckNavigationCameraDeActivation ()
    {
        if (ScriptToDesactivate.activateCam)
            ScriptToDesactivate.SwitchCam();

        if (!BigMapParent.activeInHierarchy && !DrawSystemParent.activeInHierarchy)
        {
            CameraToDesactivate.SetActive(false);
            ScriptToDesactivate.enabled = false;
        }
    }

    void CheckNavigationCameraActivation()
    {
        if (!BigMapParent.activeInHierarchy && !DrawSystemParent.activeInHierarchy)
        {
            CameraToDesactivate.SetActive(true);
            ScriptToDesactivate.enabled = true;
        }
    }

    public void SetJournal (int PageNumber)
    {
        // Setter le dessin
        StartDraw();

        // Setter les photos et le texte
        if (DataManager.AllIslands[PageNumber].NarrativText == "Aucun texte chargé")
        {
            for (int i = 0; i < AllPicturesRenderersOrigin.Count; i++)
            {
                if (i < DataManager.AllIslands[PageNumber].Pictures.Count)
                {
                    AllPicturesRenderersOrigin[i].ParentToChange.gameObject.SetActive(true);
                    AllPicturesRenderersOrigin[i].PictureImage.sprite = DataManager.AllIslands[PageNumber].Pictures[i];
                }
                else
                {
                    AllPicturesRenderersOrigin[i].ParentToChange.gameObject.SetActive(false);
                }
            }

            for (int i = 0; i < AllPicturesRenderersWithText.Count; i++)
                AllPicturesRenderersWithText[i].ParentToChange.gameObject.SetActive(false);

            NarrativeText.gameObject.SetActive(false);
            Fond.sprite = FondWithoutText;
        }
        else
        {
            for (int i = 0; i < AllPicturesRenderersOrigin.Count; i++)
            {
                if (i < DataManager.AllIslands[PageNumber].Pictures.Count)
                {
                    AllPicturesRenderersWithText[i].ParentToChange.gameObject.SetActive(true);
                    AllPicturesRenderersWithText[i].PictureImage.sprite = DataManager.AllIslands[PageNumber].Pictures[i];
                }
                else
                {
                    AllPicturesRenderersWithText[i].ParentToChange.gameObject.SetActive(false);
                }
            }

            for (int i = 0; i < AllPicturesRenderersWithText.Count; i++)
                AllPicturesRenderersOrigin[i].ParentToChange.gameObject.SetActive(false);

            NarrativeText.text = DataManager.AllIslands[PageNumber].NarrativText;
            NarrativeText.gameObject.SetActive(true);
            Fond.sprite = FondWithText;
        }
    }

	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.J))
		{
            if (DrawSystemParent.activeInHierarchy)
            {
                CameraToDesactivate.transform.position = player.transform.position;
                CloseJournal();
            }
            else
                OpenJournal();

        }

		if (Input.GetKeyDown(KeyCode.M))
		{
            if (BigMapParent.activeInHierarchy)
            {
                CameraToDesactivate.transform.position = player.transform.position;
                CloseMap();
            }
            else
                OpenMap();
        }

        if (DrawSystemParent.activeInHierarchy)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow)) PreviousPage();
            if (Input.GetKeyDown(KeyCode.RightArrow)) NextPage();
        }
        
    }

    public void RemoveIslandPicture(int ID)
    {
        DataManager.AllIslands[IslandSelected].Pictures.RemoveAt(ID);
        DataManager.AllIslands[IslandSelected].PicturesDescription.RemoveAt(ID);
        DataManager.AllIslands[IslandSelected].PicturesNarratives.RemoveAt(ID);

        if (DataManager.AllIslands[IslandSelected].NarrativText == "Aucun texte chargé")
            AllPicturesRenderersOrigin[ID].ParentToChange.gameObject.SetActive(false);
        else
            AllPicturesRenderersWithText[ID].ParentToChange.gameObject.SetActive(false);
    }

    public void NextPage ()
    {
        int temp = IslandSelected;
        IslandSelected++;
        if (IslandSelected > DataManager.AllIslands.Count - 1) IslandSelected = 0;
        if (IslandSelected != temp)  SetJournal(IslandSelected);
    }

    public void PreviousPage ()
    {
        int temp = IslandSelected;
        IslandSelected--;
        if (IslandSelected < 0) IslandSelected = DataManager.AllIslands.Count - 1;
        if (IslandSelected != temp) SetJournal(IslandSelected);
    }

    public void SendAnalyseImage (Texture2D ImageTexture, string InfluenceText, int IslandID)
    {
        StartCoroutine(MachineCallScript.sendImageAnalyseRequest(
          receiveNarrativText,
          ImageTexture,
          InfluenceText,
          false,
          10,
          5000,
          IslandID,
          reveiveErrorNarrativText
          ));
    }

    public void receiveNarrativText(string narrativText, int IslandID)
    {
        DataManager.AllIslands[IslandID].PicturesNarratives.Add(narrativText);
        SetNarrativText();
    }

    public void reveiveErrorNarrativText(string errorMessage, int IslandID)
    {
        Debug.Log(errorMessage);
        DataManager.AllIslands[IslandID].PicturesNarratives.Add("Bzzrt ... Error zzzt ...Brrzzt ... traduction .. rrzzt");
    }
}

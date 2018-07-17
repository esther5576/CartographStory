using System.Collections;
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

	[Header("Buttons")]
	public Button ResetButton;
	public Button CloseBigMapButton;
	public Button CloseJournalButton;
	public Button MapValidationButton;


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
        CreateDrawInstanceOnMap();
        OpenMap();
        DataManager.AllIslands[IslandSelected].NarrativText = NarrivTextRequest();
    }

    string NarrivTextRequest ()
    {
        MachineCallScript.objectsSeen.Clear();
        foreach (string Description in DataManager.AllIslands[IslandSelected].PicturesDescription)
            MachineCallScript.objectsSeen.Add(Description);
        return MachineCallScript.SendRequest();
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
        }
    }

	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.J))
		{
            if (DrawSystemParent.activeInHierarchy)
                CloseJournal();
            else
                OpenJournal();

        }

		if (Input.GetKeyDown(KeyCode.M))
		{
			if (BigMapParent.activeInHierarchy)
                CloseMap();
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

        if (DataManager.AllIslands[IslandSelected].NarrativText == "Aucun texte chargé")
            AllPicturesRenderersOrigin[ID].ParentToChange.gameObject.SetActive(false);
        else
            AllPicturesRenderersWithText[ID].ParentToChange.gameObject.SetActive(false);
    }

    public void NextPage ()
    {
        int temp = IslandSelected;
        IslandSelected++;
        if (IslandSelected > 2) IslandSelected = 2;
        if (IslandSelected != temp)  SetJournal(IslandSelected);
    }

    public void PreviousPage ()
    {
        int temp = IslandSelected;
        IslandSelected--;
        if (IslandSelected < 0) IslandSelected = 0;
        if (IslandSelected != temp) SetJournal(IslandSelected);
    }
}

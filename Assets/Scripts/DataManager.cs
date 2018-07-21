﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataManager
{
    public static List<Island> AllIslands = new List<Island>();
    public static int MaxPicturePerIsland = 5;
}

public class Island
{
    public int ID;
    public bool DrawStarted = false;
    public string IslandFinalName = "Name the island";
    public Sprite Drawing;
    public Texture2D TextureDraw;
    public List<Sprite> Pictures = new List<Sprite>();
    public List<string> PicturesDescription = new List<string>();
    public List<string> PicturesNarratives = new List<string>();
    public string NarrativText = "Aucun texte chargé";
}

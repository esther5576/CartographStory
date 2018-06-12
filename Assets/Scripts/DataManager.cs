using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataManager
{
    public static List<Island> AllIslands = new List<Island>();
}

public class Island
{
    public int ID;
    public string IslandFinalName = "Name the island";
    public Sprite Drawing;
    public List<Sprite> Pictures = new List<Sprite>();
    public string Text = "Aucun texte chargé";
}

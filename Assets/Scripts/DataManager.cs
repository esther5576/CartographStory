using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataManager
{
    public static List<Island> AllIslands = new List<Island>();
}

public class Island
{
    Sprite Drawing;
    List<Sprite> Pictures = new List<Sprite>();
    string Text = "Aucun texte chargé";
}

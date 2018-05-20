using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintMapHolder : MonoBehaviour {
    public static Dictionary<Collider, RenderTexture> paintTextures = new Dictionary<Collider, RenderTexture>();
    Texture2D fadeMap;
    float timer;
    float nextTime = 0.4f;
    public bool FadeOverTime;
    // Use this for initialization
    void Start () {
        CreateFadeTexture();
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.time > timer && FadeOverTime)
        {
            timer = Time.time + nextTime;
            foreach (KeyValuePair<Collider, RenderTexture> texture in paintTextures)
            {
                DrawTexture2(texture.Value);
            }
        }
	}
    void CreateFadeTexture()
    {
        fadeMap = new Texture2D(1, 1);
        fadeMap.SetPixel(0, 0, new Color(255, 255, 255, 0.04f));
        fadeMap.Apply();
    }
    void DrawTexture2(RenderTexture rt)
    {

        RenderTexture.active = rt; // activate rendertexture for drawtexture;
        GL.PushMatrix();                       // save matrixes
        GL.LoadPixelMatrix(0, rt.height, rt.height, 0);      // setup matrix for correct size

        // draw brushtexture
        Graphics.DrawTexture(new Rect(0, 0, rt.height, rt.height), fadeMap);
        //Graphics.DrawTexture(new Rect(posX - brushTexture.width / brushSize, (rt.height - posY) - brushTexture.height / brushSize, brushTexture.width / (brushSize * 0.5f), brushTexture.height / (brushSize * 0.5f)), fadeMap);
        GL.PopMatrix();
        RenderTexture.active = null;// turn off rendertexture



    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowTrail : MonoBehaviour
{
   
    public int resolution = 512;
   
  
    public float brushMin;
    public float brushMax;
    public Texture2D brushTexture;
  
    Vector2 stored;
    Vector2 pixelUV;
    Texture2D whiteMap;
    float brushSize;

    void Start()
    {
        CreateClearTexture();// clear white texture to draw on
       
       
    }

    void Update()
    {
       
        Debug.DrawRay(transform.position, Vector3.down * 20f, Color.magenta);
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit))
          
            {
                Collider coll = hit.collider;
                if (coll != null && hit.distance < 0.2f)
                {
                    brushSize = Random.Range(brushMin, brushMax);
                  
                    if (!PaintMapHolder.paintTextures.ContainsKey(coll)) // if there is already paint on the material, add to that
                    {
                        Renderer rend = hit.transform.GetComponent<Renderer>();
                    PaintMapHolder.paintTextures.Add(coll, getWhiteRT());
                        rend.material.SetTexture("_PaintMap", PaintMapHolder.paintTextures[coll]);
                    }
              
                if (stored != hit.lightmapCoord) // stop drawing on the same point
                    {
                        stored = hit.lightmapCoord;

                    pixelUV = hit.lightmapCoord;
                    pixelUV.y *= resolution;
                    pixelUV.x *= resolution;
                    DrawTexture(PaintMapHolder.paintTextures[coll], pixelUV.x, pixelUV.y);
                   


                }
                }
            }
        
      

    }

    void DrawTexture(RenderTexture rt, float posX, float posY)
    {

        RenderTexture.active = rt; // activate rendertexture for drawtexture;
        GL.PushMatrix();                       // save matrixes
        GL.LoadPixelMatrix(0, resolution, resolution, 0);      // setup matrix for correct size

        // draw brushtexture
        Graphics.DrawTexture(new Rect(posX - brushTexture.width / brushSize, (rt.height - posY) - brushTexture.height / brushSize, brushTexture.width / (brushSize * 0.5f), brushTexture.height / (brushSize * 0.5f)), brushTexture);
        GL.PopMatrix();
        RenderTexture.active = null;// turn off rendertexture


    }

   

    RenderTexture getWhiteRT()
    {
        RenderTexture rt = new RenderTexture(resolution, resolution, 32);
        Graphics.Blit(whiteMap, rt);
        return rt;
    }

    void CreateClearTexture()
    {
        whiteMap = new Texture2D(1, 1);
        whiteMap.SetPixel(0, 0, Color.white);
        whiteMap.Apply();
    }

   
}
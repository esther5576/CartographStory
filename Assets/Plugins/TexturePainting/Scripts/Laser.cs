using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public GameObject pfx;
    public int resolution = 512;
    Texture2D whiteMap;
    public float brushMin;
    public float brushMax;
    public Texture2D brushTexture;
    public Texture2D eraseTexture;
    Vector2 stored;
  
    LineRenderer lineRenderer;
    float brushSize;

    void Start()
    {
        CreateClearTexture();// clear white texture to draw on
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Debug.DrawRay(transform.position, Vector3.down * 20f, Color.magenta);
            RaycastHit hit;
            //if (Physics.Raycast(transform.position, Vector3.down, out hit))
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)) // delete previous and uncomment for mouse painting
            {
                Collider coll = hit.collider;
                if (coll != null)
                {
                    brushSize = Random.Range(brushMin, brushMax);
                    lineRenderer.SetPosition(0, transform.position);
                    lineRenderer.SetPosition(1, hit.point);
                    GameObject fx = Instantiate(pfx, hit.point, Quaternion.identity) as GameObject;
                    Destroy(fx, 1f);
                    if (!PaintMapHolder.paintTextures.ContainsKey(coll)) // if there is already paint on the material, add to that
                    {
                        Renderer rend = hit.transform.GetComponent<Renderer>();
                        PaintMapHolder.paintTextures.Add(coll, getWhiteRT());
                        rend.material.SetTexture("_PaintMap", PaintMapHolder.paintTextures[coll]);
                    }
                    if (stored != hit.lightmapCoord) // stop drawing on the same point
                    {
                        stored = hit.lightmapCoord;
                        Vector2 pixelUV = hit.lightmapCoord;
                        pixelUV.y *= resolution;
                        pixelUV.x *= resolution;
                        if (Input.GetKey(KeyCode.LeftShift))
                        {
                            Erase(PaintMapHolder.paintTextures[coll], pixelUV.x, pixelUV.y);
                        }
                        else
                        {
                            DrawTexture(PaintMapHolder.paintTextures[coll], pixelUV.x, pixelUV.y);
                        }
                        
                    }
                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            lineRenderer.SetPosition(0, transform.position);
           lineRenderer.SetPosition(1, transform.position);
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
    void Erase(RenderTexture rt, float posX, float posY)
    {

        RenderTexture.active = rt; // activate rendertexture for drawtexture;
        GL.PushMatrix();                       // save matrixes
        GL.LoadPixelMatrix(0, resolution, resolution, 0);      // setup matrix for correct size

        // draw brushtexture
        Graphics.DrawTexture(new Rect(posX - eraseTexture.width / brushSize, (rt.height - posY) - eraseTexture.height / brushSize, eraseTexture.width / (brushSize * 0.5f), eraseTexture.height / (brushSize * 0.5f)), eraseTexture);
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
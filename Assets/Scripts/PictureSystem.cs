using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class PictureSystem : MonoBehaviour
{
    public Image debugImage;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(Input.GetKeyDown(KeyCode.Space))
        {
            TakePic();
        }
	}

    public void TakePic()
    {
        string imageName = "screensotTest.png";

        //TAKE SCREENSHOT
        /*ScreenCapture.CaptureScreenshot(imageName);
        print(Application.persistentDataPath);*/

        Debug.Log(Application.persistentDataPath);
        ScreenCapture.CaptureScreenshot(Application.persistentDataPath + System.DateTime.Now.Month + System.DateTime.Now.Day + System.DateTime.Now.Hour + System.DateTime.Now.Minute + System.DateTime.Now.Second + ".png");

        /*//READ DATA FROM FILE
        byte[] data = File.ReadAllBytes(Application.persistentDataPath + "/" + imageName);

        // CREATE THE TEXTURE
        Texture2D screenshotTexture = new Texture2D(Screen.width, Screen.height);

        //LOAD THE IMAGE
        screenshotTexture.LoadImage(data);

        //CREATE A SPRITE
        Sprite screenshotSprite = Sprite.Create(screenshotTexture, new Rect(0, 0, Screen.width, Screen.height), new Vector2(0.5f, 0.5f));

        //SETTHE SPRITE ON THE PREVIEW SCREEN
        if (debugImage != null)
        {
            debugImage.GetComponent<Image>().sprite = screenshotSprite;
            debugImage.color = new Color(1, 1, 1, 1);
        }*/
    }
}

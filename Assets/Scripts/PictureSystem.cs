using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class PictureSystem : MonoBehaviour
{
    public Image debugImage;
    public UnityEngine.PostProcessing.PostProcessingBehaviour postprocessing;
    public UnityEngine.PostProcessing.PostProcessingProfile profileNoVigPic;
    public UnityEngine.PostProcessing.PostProcessingProfile profileVigPic;

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
        postprocessing.profile = profileNoVigPic;
        StartCoroutine(waitFrame());
    }

    IEnumerator waitFrame()
    {
        yield return new WaitForEndOfFrame();
        // CREATE THE TEXTURE WITH SCREENSHOT
        Texture2D screenshotTexture = ScreenCapture.CaptureScreenshotAsTexture();
        //CREATE A SPRITE
        Sprite screenshotSprite = Sprite.Create(screenshotTexture, new Rect(0, 0, screenshotTexture.width, screenshotTexture.height), new Vector2(0f, 0f));

        //SETTHE SPRITE ON THE PREVIEW SCREEN
        if (debugImage != null)
        {
            debugImage.GetComponent<Image>().sprite = screenshotSprite;
            debugImage.color = new Color(1, 1, 1, 1);
        }
        yield return new WaitForEndOfFrame();
        postprocessing.profile = profileVigPic;
    }
}

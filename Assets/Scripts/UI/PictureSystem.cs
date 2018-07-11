using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using DG.Tweening;

//test
public class PictureSystem : MonoBehaviour
{
    public Image debugImage;
    public UnityEngine.PostProcessing.PostProcessingBehaviour postprocessing;
    public UnityEngine.PostProcessing.PostProcessingProfile profileNoVigPic;
    public UnityEngine.PostProcessing.PostProcessingProfile profileVigPic;
    [HideInInspector]
    public int IDOfIlsandToAdd;

    public CanvasGroup camerapreview;
    public Image imagepreview;
    public CanvasGroup cameraPicUI;

    bool inPreviewMode = false;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void TakePic(string description)
    {
        if (!inPreviewMode)
        {
            postprocessing.profile = profileNoVigPic;
            StartCoroutine(waitFrame(description));
            inPreviewMode = true;
            cameraPicUI.alpha = 0;
        }
    }

    IEnumerator waitFrame(string description)
    {
        yield return new WaitForEndOfFrame();

        int theID = DataManager.AllIslands.FindIndex(a => a.ID == IDOfIlsandToAdd);
        Debug.Log(theID + " THE ISLAND " + DataManager.AllIslands[theID].ID);

        if (DataManager.AllIslands[theID].Pictures.Count < DataManager.MaxPicturePerIsland)
        {

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

       
            DataManager.AllIslands[theID].Pictures.Add(screenshotSprite);
            DataManager.AllIslands[theID].PicturesDescription.Add(description);

            #region Kill tweens
            DOTween.Kill("save01");
            DOTween.Kill("save02");
            DOTween.Kill("save03");
            DOTween.Kill("dontsave01");
            DOTween.Kill("dontsave02");
            DOTween.Kill("dontsave03");
            #endregion

            imagepreview.transform.DOLocalMoveY(38.7f, 0).SetEase(Ease.InBack).SetId("setup01");
            imagepreview.transform.DOScale(1f, 0).SetId("setup02");
            DOTween.To(() => camerapreview.alpha, x => camerapreview.alpha = x, 1, 0.5f).SetId("setup03");
            imagepreview.sprite = screenshotSprite;

            yield return new WaitForEndOfFrame();
            postprocessing.profile = profileVigPic;
            cameraPicUI.alpha = 1;
        }
        else
        {
            Debug.LogWarning("YOU HAVE NO MORE SPACE FOR THIS ISLAND!");

            yield return new WaitForEndOfFrame();
            postprocessing.profile = profileVigPic;
        }
    }

    public void SaveImage()
    {
        #region Kill tweens
        DOTween.Kill("setup01");
        DOTween.Kill("setup02");
        DOTween.Kill("setup03");
        #endregion

        DOTween.To(() => camerapreview.alpha, x => camerapreview.alpha = x, 0, 0.5f).SetDelay(1).SetId("save01").OnComplete(EnableMorePics);
        imagepreview.transform.DOLocalMoveY(-500, 1).SetEase(Ease.InBack).SetId("save02");
        imagepreview.transform.DOScale(0.1f, 1f).SetId("save03");
    }

    public void DontSaveImage()
    {
        #region Kill tweens
        DOTween.Kill("setup01");
        DOTween.Kill("setup02");
        DOTween.Kill("setup03");
        #endregion

        Debug.Log("Don't save picture");

        DOTween.To(() => camerapreview.alpha, x => camerapreview.alpha = x, 0, 0.5f).SetDelay(1).SetId("dontsave01").OnComplete(EnableMorePics);
        imagepreview.transform.DOLocalMoveY(500, 1).SetEase(Ease.InBack).SetId("dontsave02");
        imagepreview.transform.DOScale(0.1f, 1).SetId("dontsave03");

        int theID = DataManager.AllIslands.FindIndex(a => a.ID == IDOfIlsandToAdd);
        DataManager.AllIslands[theID].Pictures.RemoveAt(DataManager.AllIslands[theID].Pictures.Count - 1);
        DataManager.AllIslands[theID].PicturesDescription.RemoveAt(DataManager.AllIslands[theID].PicturesDescription.Count - 1);
    }

    public void EnableMorePics()
    {
        inPreviewMode = false;
    }
}
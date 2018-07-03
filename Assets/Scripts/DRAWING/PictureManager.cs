using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PictureManager : MonoBehaviour, IPointerClickHandler
{
    public int ID;
    public Image PictureImage;
    public RectTransform ParentToChange;
    public Vector2 AnchoredPositionOrigin;
    public Vector2 SizeDeltaOrigin;
    public Vector2 AnchoreMaxOrigin;
    public Vector2 AnchoreMinOrigin;
    public Vector3 RotationOrigin;

    Vector2 AnchoredPositionFullScreen;
    Vector2 SizeDeltaFullScreen;
    Vector2 AnchoreMaxFullScreen;
    Vector2 AnchoreMinFullScreen;
    Vector3 RotationFullScreen;

    bool IsFullScreen = false;

    public float AnimationDuration = 0.5f;

    // Use this for initialization
    void Awake ()
    {
        ParentToChange = transform.parent.gameObject.GetComponent<RectTransform>();
        PictureImage = GetComponent<Image>();
        AnchoredPositionOrigin = ParentToChange.anchoredPosition;
        SizeDeltaOrigin = ParentToChange.sizeDelta;
        AnchoreMaxOrigin = ParentToChange.anchorMax;
        AnchoreMinOrigin = ParentToChange.anchorMin;
        RotationOrigin = ParentToChange.rotation.eulerAngles;
        AnchoredPositionFullScreen = Vector2.zero;
        SizeDeltaFullScreen = Vector2.zero;
        AnchoreMinFullScreen = Vector2.zero;
        AnchoreMaxFullScreen = Vector2.one;
        RotationFullScreen = Vector3.zero;
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        IsFullScreen = !IsFullScreen;
        if (IsFullScreen)
        {
            GoFullScreen();
        }
        else
        {
            GoOriginSize();
        }
    }

    void GoFullScreen()
    {
        DOTween.Kill("PictureOriginSize" + ID);
        Transform papa = ParentToChange.parent;
        ParentToChange.parent = null;
        ParentToChange.parent = papa;
        DOTween.To(() => ParentToChange.anchoredPosition, x => ParentToChange.anchoredPosition = x, AnchoredPositionFullScreen, AnimationDuration).SetId("PictureFullScreen" + ID);
        DOTween.To(() => ParentToChange.sizeDelta, x => ParentToChange.sizeDelta = x, SizeDeltaFullScreen, AnimationDuration).SetId("PictureFullScreen" + ID);
        DOTween.To(() => ParentToChange.anchorMax, x => ParentToChange.anchorMax = x, AnchoreMaxFullScreen, AnimationDuration).SetId("PictureFullScreen" + ID);
        DOTween.To(() => ParentToChange.anchorMin, x => ParentToChange.anchorMin = x, AnchoreMinFullScreen, AnimationDuration).SetId("PictureFullScreen" + ID);
        //ParentToChange.DOAnchorPos(AnchoredPositionFullScreen, AnimationDuration).SetId("PictureFullScreen" + ID);
        //ParentToChange.DOSizeDelta(SizeDeltaFullScreen, AnimationDuration).SetId("PictureFullScreen" + ID);
        //ParentToChange.DOAnchorMax(AnchoreMaxFullScreen, AnimationDuration).SetId("PictureFullScreen" + ID);
        //ParentToChange.DOAnchorMin(AnchoreMinFullScreen, AnimationDuration).SetId("PictureFullScreen" + ID);
        ParentToChange.DORotate(RotationFullScreen, AnimationDuration).SetId("PictureFullScreen" + ID);
    }

    void GoOriginSize ()
    {
        DOTween.Kill("PictureFullScreen" + ID);
        DOTween.To(() => ParentToChange.anchoredPosition, x => ParentToChange.anchoredPosition = x, AnchoredPositionOrigin, AnimationDuration).SetId("PictureFullScreen" + ID);
        DOTween.To(() => ParentToChange.sizeDelta, x => ParentToChange.sizeDelta = x, SizeDeltaOrigin, AnimationDuration).SetId("PictureFullScreen" + ID);
        DOTween.To(() => ParentToChange.anchorMax, x => ParentToChange.anchorMax = x, AnchoreMaxOrigin, AnimationDuration).SetId("PictureFullScreen" + ID);
        DOTween.To(() => ParentToChange.anchorMin, x => ParentToChange.anchorMin = x, AnchoreMinOrigin, AnimationDuration).SetId("PictureFullScreen" + ID);
        //ParentToChange.DOAnchorPos(AnchoredPositionOrigin, AnimationDuration).SetId("PictureFullScreen" + ID);
        //ParentToChange.DOSizeDelta(SizeDeltaOrigin, AnimationDuration).SetId("PictureOriginSize" + ID);
        //ParentToChange.DOAnchorMax(AnchoreMaxOrigin, AnimationDuration).SetId("PictureOriginSize" + ID);
        //ParentToChange.DOAnchorMin(AnchoreMinOrigin, AnimationDuration).SetId("PictureOriginSize" + ID);
        ParentToChange.DORotate(RotationOrigin, AnimationDuration).SetId("PictureOriginSize" + ID);
    }
}

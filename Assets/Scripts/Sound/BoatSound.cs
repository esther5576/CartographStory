using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BoatSound : MonoBehaviour
{
    public FMODUnity.StudioEventEmitter bellSound;
    public FMODUnity.StudioEventEmitter voileSound;
    public FMODUnity.StudioEventEmitter WavesOnMoveSound;
    public FMODUnity.StudioEventEmitter BasedynamicWind;
    public FMODUnity.StudioEventEmitter CrackdynamicWind;
    public FMODUnity.StudioEventEmitter ChangeDirection;
    public FMODUnity.StudioEventEmitter ClipSail;
    public FMODUnity.StudioEventEmitter SailUp;
    public FMODUnity.StudioEventEmitter SailDown;
    BoatNavigation boatControls;

    public float windRandomParam;
    public float crackRandomParam;


    float actualSpeedPrev = 0;

    // Use this for initialization
    void Start ()
    {
        boatControls = this.GetComponent<BoatNavigation>();
        DOTween.To(() => windRandomParam, x => windRandomParam = x, 0.5f, 1).OnComplete(CreateNewRandomWind);
        DOTween.To(() => crackRandomParam, x => crackRandomParam = x, 0.5f, 1).OnComplete(CreateNewRandomCrack);
    }
	
	// Update is called once per frame
	void Update ()
    {
		if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            bellSound.Play();
        }

        if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyUp(KeyCode.Q) || Input.GetKeyUp(KeyCode.D))
        {
            float r = Random.Range(0.0f, 1.0f);
            Debug.Log(r);
            ChangeDirection.Play();
            ChangeDirection.SetParameter("SailDirection", r);
        }

        if(Input.GetKeyUp(KeyCode.Z) || Input.GetKeyUp(KeyCode.S))
        {
            if (boatControls.actualSpeed < 1 && boatControls.actualSpeed > 0)
            {
                Debug.Log("STOP2");
                ClipSail.Play();
                SailUp.Stop();
                SailDown.Stop();
            }
        }

        if(actualSpeedPrev < boatControls.actualSpeed && (Input.GetKey(KeyCode.Z)))
        {
            if(!SailUp.IsPlaying())
            {
                SailUp.Play();
                Debug.Log("test");
            }

            //Debug.Log(actualSpeedPrev + "  " + boatControls.actualSpeed);
            actualSpeedPrev = boatControls.actualSpeed;

            if(actualSpeedPrev >= 1)
            {
                SailUp.Stop();
                Debug.Log("STOPZ");
                ClipSail.Play();
            }
        }

        if (actualSpeedPrev > boatControls.actualSpeed && (Input.GetKey(KeyCode.S)))
        {
            if (!SailDown.IsPlaying())
            {
                SailDown.Play();
            }

            //Debug.Log(actualSpeedPrev + "  " + boatControls.actualSpeed);
            actualSpeedPrev = boatControls.actualSpeed;

            if (actualSpeedPrev <= 0)
            {
                ClipSail.Play();
                SailDown.Stop();
            }
        }

        voileSound.SetParameter("OuvertureVoile", boatControls.actualSpeed);
        WavesOnMoveSound.SetParameter("BoatGoing", boatControls.actualSpeed);

        BasedynamicWind.SetParameter("WindForce", windRandomParam);
        CrackdynamicWind.SetParameter("DynamicBoatCrack", crackRandomParam);
    }

    void CreateNewRandomWind()
    {
        DOTween.To(() => windRandomParam, x => windRandomParam = x, Random.Range(0f,1f), 1).OnComplete(CreateNewRandomWind);
    }

    void CreateNewRandomCrack()
    {
        DOTween.To(() => crackRandomParam, x => crackRandomParam = x, Random.Range(0f, 1f), 1).OnComplete(CreateNewRandomCrack);
    }
}

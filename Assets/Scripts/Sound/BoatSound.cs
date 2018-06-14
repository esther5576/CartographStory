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
    BoatNavigation boatControls;

    public float windRandomParam;
    public float crackRandomParam;

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleHasTriggered : MonoBehaviour
{
    public ParticleSystem ps;


    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }

    private void OnParticleTrigger()
    {
        // particles
        List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();

        // get
        int numEnter = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
        SplashSoundTriggered(numEnter);

    }

    public void SplashSoundTriggered (int NumberOfSpash)
    {

    }
}

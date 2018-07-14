using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpFishManager : MonoBehaviour
{
    public ParticleSystem JumpFishParticleSystem;

    public float ChanceToTrigger; 

    private void OnTriggerEnter(Collider other)
    {
        if (Random.Range(0.0f,1.0f) <= ChanceToTrigger)
        {
            if (!JumpFishParticleSystem.isPlaying)
                JumpFishParticleSystem.Play();
        }
    }
}

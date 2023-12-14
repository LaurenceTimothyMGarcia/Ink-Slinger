using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticleSystem : MonoBehaviour
{
    public ParticleSystem absorbInk;
    public ParticleSystem useInk;
    public ParticleSystem meleeSlash;

    public void PlayAbsorbInk()
    {
        absorbInk.Play();
    }

    public void PlayUseInk()
    {
        useInk.Play();
    }

    public void PlayUseMeleeSlash()
    {
        meleeSlash.Play();
    }
}

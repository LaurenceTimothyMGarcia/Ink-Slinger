using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticleSystem : MonoBehaviour
{
    public ParticleSystem absorbInk;
    public ParticleSystem useInk;
    public ParticleSystem meleeSlash;

    public ParticleSystem meleePoweredSlash;

    public ParticleSystem inkAOE1;
    public ParticleSystem inkAOE2;
    public ParticleSystem inkAOE3;
    public ParticleSystem inkAOE4;


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

    public void PlayUseMeleePoweredSlash()
    {
        meleePoweredSlash.Play();
    }

    public void PlayInkAOE()
    {
        inkAOE1.Play();
        inkAOE2.Play();
        inkAOE3.Play();
        inkAOE4.Play();
    }
}

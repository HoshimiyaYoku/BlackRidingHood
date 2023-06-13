using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleSM : MonoBehaviour
{
    public ParticleSystem dustEffect, stoneEffect, boomEffect;

    public void AttackStartEffect()
    {
        dustEffect.Stop();
        stoneEffect.Stop();
        boomEffect.Play();
    }

    public void AttackEndEffect()
    {
        dustEffect.Play();
        stoneEffect.Play();
        boomEffect.Stop();
    }

}

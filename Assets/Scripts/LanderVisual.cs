using System;
using UnityEngine;

public class LanderThrusterVisual : MonoBehaviour
{
    [SerializeField] private ParticleSystem rightParticleSystem;
    [SerializeField] private ParticleSystem leftParticleSystem;
    [SerializeField] private ParticleSystem midParticleSystem;
    [SerializeField] private GameObject landerExplosionVFX;

    private Lander lander;
    private void Awake()
    {
        lander = GetComponent<Lander>();
        lander.OnUpForce += Lander_OnUpForce;
        lander.OnLeftForce += Lander_OnLeftForce;
        lander.OnRightForce += Lander_OnRightForce;
        lander.OnBeforeForce += Lander_OnBeforeForce;

        SetEnableThrusterParticleSystem(midParticleSystem, false);
        SetEnableThrusterParticleSystem(leftParticleSystem, false);
        SetEnableThrusterParticleSystem(rightParticleSystem, false);
    }

    private void Start()
    {
        lander.OnLanded += Lander_OnLanded;
    }

    private void Lander_OnLanded(object sender, Lander.OnLandedEventArgs e)
    {
        switch (e.landingType)
        {
            case Lander.LandingType.TooFastLanding:
            case Lander.LandingType.TooSteepAngle:
            case Lander.LandingType.WrongLanding:
                Instantiate(landerExplosionVFX, transform.position, Quaternion.identity);
                gameObject.SetActive(false);
                break;
        }
    }

    private void Lander_OnBeforeForce(object sender, EventArgs e)
    {
        SetEnableThrusterParticleSystem(midParticleSystem, false);
        SetEnableThrusterParticleSystem(leftParticleSystem, false);
        SetEnableThrusterParticleSystem(rightParticleSystem, false);
    }

    private void Lander_OnRightForce(object sender, EventArgs e)
    {
        SetEnableThrusterParticleSystem(midParticleSystem, false);
        SetEnableThrusterParticleSystem(leftParticleSystem, false);
        SetEnableThrusterParticleSystem(rightParticleSystem, true);
    }

    private void Lander_OnLeftForce(object sender, EventArgs e)
    {
        SetEnableThrusterParticleSystem(midParticleSystem, false);
        SetEnableThrusterParticleSystem(leftParticleSystem, true);
        SetEnableThrusterParticleSystem(rightParticleSystem, false);
    }

    private void Lander_OnUpForce(object sender, EventArgs e)
    {
        SetEnableThrusterParticleSystem(midParticleSystem,true);
        SetEnableThrusterParticleSystem(leftParticleSystem,true);
        SetEnableThrusterParticleSystem(rightParticleSystem,true);
    }

    private void SetEnableThrusterParticleSystem(ParticleSystem particleSystem, bool enabled)
    {
        ParticleSystem.EmissionModule emissionModule = particleSystem.emission;
        emissionModule.enabled = enabled;
    }
}



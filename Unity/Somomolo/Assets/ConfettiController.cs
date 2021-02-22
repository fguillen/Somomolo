using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfettiController : MonoBehaviour
{
    public static ConfettiController instance;
    [SerializeField] ParticleSystem confettiParticleSystem;
    ParticleSystem.Particle[] confettiParticles;
    [SerializeField] float sweepForce;

    void Awake()
    {
        instance = this;

        if(confettiParticles == null || confettiParticles.Length < confettiParticleSystem.main.maxParticles)
        {
            confettiParticles = new ParticleSystem.Particle[confettiParticleSystem.main.maxParticles];
        }
    }

    [ContextMenu("SweepConfetti")]
    public void SweepConfetti()
    {
        var particlesCount = confettiParticleSystem.GetParticles(confettiParticles);

        for (int i = 0; i < particlesCount; i++)
        {
            confettiParticles[i].velocity += Vector3.left * sweepForce;
        }

        confettiParticleSystem.SetParticles(confettiParticles, particlesCount);
    }
}

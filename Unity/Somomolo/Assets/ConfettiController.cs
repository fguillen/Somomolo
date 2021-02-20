using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfettiController : MonoBehaviour
{
    [SerializeField] ParticleSystem confettiParticleSystem;
    ParticleSystem.Particle[] confettiParticles;
    [SerializeField] float sweepForce;

    void Awake()
    {
        if(confettiParticles == null || confettiParticles.Length < confettiParticleSystem.main.maxParticles)
        {
            confettiParticles = new ParticleSystem.Particle[confettiParticleSystem.main.maxParticles];
        }
    }

    [ContextMenu("SweepConfetti")]
    void SweepConfetti()
    {
        print("SweepConfetti");

        var particlesCount = confettiParticleSystem.GetParticles(confettiParticles);

        print($"particles amount: ${particlesCount}");

        for (int i = 0; i < particlesCount; i++)
        {
            print($"velocity before: ${confettiParticles[i].velocity}");
            confettiParticles[i].velocity += Vector3.left * sweepForce;
            print($"velocity after: ${confettiParticles[i].velocity}");
        }

        confettiParticleSystem.SetParticles(confettiParticles, particlesCount);
    }
}

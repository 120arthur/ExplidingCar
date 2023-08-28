using System.Collections;
using UnityEngine;

/// <summary>
/// Enumerates the different types of Particles.
/// </summary>
public enum ParticleType
{
    Explosion
}

/// <summary>
/// Controls behavior of Particles in the game.
/// </summary>
public class ParticleEffectHandler : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem m_particleSystem;
    [SerializeField]
    private float m_particleDuration;

    private WaitForSeconds m_waitToDisable;

    private bool m_isActive;

    public bool IsActive => m_isActive;

    public void InitPartice(Vector3 position)
    {
        m_waitToDisable = new WaitForSeconds(m_particleDuration);
        StartParticle(position);
    }

    /// <summary>
    /// Start playing the particle effect at the specified position.
    /// </summary>
    /// <param name="position">The position at which to start the particle effect.</param>
    public void StartParticle(Vector3 position)
    {
        transform.position = position;
        m_isActive = true;
        m_particleSystem.Play();
        StartCoroutine(EndParticle());
    }

    /// <summary>
    /// Coroutine to wait for the particle effect duration and then stop it.
    /// </summary>
    /// <returns>An IEnumerator used for the coroutine.</returns>
    private IEnumerator EndParticle()
    {
        yield return m_waitToDisable;
        m_isActive = false;
        m_particleSystem.Stop();
    }
}

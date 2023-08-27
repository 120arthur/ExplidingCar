using System.Collections;
using UnityEngine;

public enum ParticleType
{
    Explosion
}

public class ParticleHandler : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem m_particleSystem;
    [SerializeField]
    private float m_particleDuration;

    private WaitForSeconds m_WaitToDisable;

    private bool m_isActive;

    public bool IsActive => m_isActive;

    private void Start()
    {
        m_WaitToDisable = new WaitForSeconds(m_particleDuration);
    }

    public void StartParticle(Vector3 position)
    {
        transform.position = position;
        m_isActive = true;
        m_particleSystem.Play();
        StartCoroutine(EndParticle());
    }

    public IEnumerator EndParticle()
    {
        yield return m_WaitToDisable;
        m_isActive = false;
        m_particleSystem.Stop();
    }
}

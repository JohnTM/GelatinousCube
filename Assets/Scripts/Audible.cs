using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audible : MonoBehaviour
{
    [SerializeField]
    private AudibleType m_type;

    public void SoundEvent(string name, Vector3 position)
    {
        SoundEvent(m_type, name, position);
    }

    public static void SoundEvent(AudibleType type, string name, Vector3 position)
    {
        foreach (var t in type.soundTypes)
        {
            if (t.name == name && t.clips.Length > 0)
            {
                int randClipIdx = Random.Range(0, t.clips.Length);
                float randVolume = Random.Range(t.minVolume, t.maxVolume);
                AudioSource.PlayClipAtPoint(t.clips[randClipIdx], position, randVolume);
            }
            if (t.particles)
            {
                ParticleSystem particles = Instantiate(t.particles);
                particles.transform.position = position;
                particles.Play();
            }

            
        }
    }
}

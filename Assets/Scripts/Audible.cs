using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audible : MonoBehaviour
{
    [SerializeField]
    private AudibleType m_type;

    public void SoundEvent(string name, Vector3 position)
    {
        foreach (var type in m_type.soundTypes)
        {
            if (type.name == name && type.clips.Length > 0)
            {
                int randClipIdx = Random.Range(0, type.clips.Length);
                float randVolume = Random.Range(type.minVolume, type.maxVolume);
                AudioSource.PlayClipAtPoint(type.clips[randClipIdx], position, randVolume);
            }
        }
    }
}

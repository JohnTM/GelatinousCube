using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class AudibleType : ScriptableObject {

#if UNITY_EDITOR
    [MenuItem("Assets/Create/Audible Type")]
    public static void CreateAsset()
    {
        ScriptableObjectUtility.CreateAsset<AudibleType>();
    }
#endif

    [System.Serializable]
    public class SoundSettings
    {
        public string name;
        public AudioClip[] clips;
        public ParticleSystem particles;
        public bool alignParticlesWithNormal;
        public float minPitch = 0.8f;
        public float maxPitch = 1.2f;
        public float minVolume = 0.8f;
        public float maxVolume = 1.2f;
    }

    public SoundSettings[] soundTypes;
}

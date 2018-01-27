using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AudibleType : ScriptableObject {

    [MenuItem("Assets/Create/Audible Type")]
    public static void CreateAsset()
    {
        ScriptableObjectUtility.CreateAsset<AudibleType>();
    }

    [System.Serializable]
    public class SoundSettings
    {
        public string name;
        public AudioClip[] clips;
        public float minPitch = 0.8f;
        public float maxPitch = 1.2f;
        public float minVolume = 0.8f;
        public float maxVolume = 1.2f;
    }

    public SoundSettings[] soundTypes;
}

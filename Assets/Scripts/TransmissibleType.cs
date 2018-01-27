using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class TransmissibleType : ScriptableObject
{
#if UNITY_EDITOR
    [MenuItem("Assets/Create/Transmissible Type")]
    public static void CreateAsset()
    {
        ScriptableObjectUtility.CreateAsset<TransmissibleType>();
    }
#endif

    public Material material;
    public float timeToDrain = 1.0f;
    public bool hasCollisions;
    public float bouyancy;
    public float damage;
    public bool attractBugs;
    public bool hasRigidbody;
    public float density;
    public Color beamColor;
    public Vector2 animateUVs;

    public AudioClip drainClip;
    public AudioClip infuseClip;

    public AudibleType audibleType;
}
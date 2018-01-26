using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
 
public class TransmissibleType : ScriptableObject
{
    [MenuItem("Assets/Create/Transmissible Type")]
    public static void CreateAsset()
    {
        ScriptableObjectUtility.CreateAsset<TransmissibleType>();
    }

    public Material material;
    public float timeToDrain = 1.0f;
    public bool hasCollisions;
    public float bouyancy;
    public float damage;
    public float lightEmission;
    public Color lightColor;
    public bool hasRigidbody;
    public float density;
    public Vector2 animateUVs;    
}
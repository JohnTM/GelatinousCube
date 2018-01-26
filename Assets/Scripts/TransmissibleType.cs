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
    public float timeToDrain;
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAndroidResolution : MonoBehaviour {

    public int hRenderRes = 1480;
    public int vRenderRes = 720;

    // Use this for initialization
#if UNITY_ANDROID
    void Start () {
        Screen.SetResolution(hRenderRes, vRenderRes , true);
    }
#endif

}

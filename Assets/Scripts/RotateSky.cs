using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSky : MonoBehaviour {

    public float rotationSpeed;
    private float rotation = 0;
	
	// Update is called once per frame
	void Update () {
        rotation += Time.deltaTime * rotationSpeed;
        if (rotation > 360)
        {
            rotation = rotation - 360f;
        }
        RenderSettings.skybox.SetFloat("_Rotation", rotation);
	}
}

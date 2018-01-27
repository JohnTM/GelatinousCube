using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrackPlayer : MonoBehaviour {

    [SerializeField] Transform playerObject;
    [SerializeField] float smoothTime = 1.0f;
    private Vector3 cameraOffset;
    private Vector3 velocity = Vector3.zero;

	// Use this for initialization
	void Start () {
        cameraOffset = (transform.position - playerObject.position);
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = Vector3.SmoothDamp(transform.position, (playerObject.position + cameraOffset), ref velocity, smoothTime);
	}
}

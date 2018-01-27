using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {

    public bool isDrainPressed
    {
        get { return Input.GetButton("Fire1"); }
    }

    public bool isInfusePressed
    {
        get { return Input.GetButton("Fire2"); }
    }

    public Vector2 movement
    {
        get {  return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")); }
    }

    public bool wasJumpPressed
    {
        get { return Input.GetButtonDown("Jump"); }
    }
   


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

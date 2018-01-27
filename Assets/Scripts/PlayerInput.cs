using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class PlayerInput : MonoBehaviour {

    public bool isDrainPressed
    {
        get
        {
            bool drain = Input.GetMouseButton(0);

            if (InputManager.ActiveDevice != null && InputManager.ActiveDevice.LeftTrigger.IsPressed)
            {
                drain = true;
            }

            return drain;
        }
    }

    public bool isInfusePressed
    {
        get
        {
            bool infuse = Input.GetMouseButton(1);

            if (InputManager.ActiveDevice != null && InputManager.ActiveDevice.RightTrigger.IsPressed)
            {
                infuse = true;
            }

            return infuse;
        }
    }

    public Vector2 movement
    {
        get
        {
            Vector2 move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            if (InputManager.ActiveDevice != null && InputManager.ActiveDevice.LeftStick.Vector.magnitude > 0.1f)
            {
                move = InputManager.ActiveDevice.LeftStick.Vector.normalized;
            }

            return move;
        }
    }

    public bool wasJumpPressed
    {
        get
        {
            bool jump = Input.GetButtonDown("Jump");
            if (InputManager.ActiveDevice != null && InputManager.ActiveDevice.Action1.WasPressed)
            {
                jump = true;
            }
            return jump;
         }
    }
   


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

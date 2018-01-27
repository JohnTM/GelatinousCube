using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foot : MonoBehaviour
{

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider collider)
    {
        Audible audible = collider.GetComponent<Audible>();

        if (audible)
        {
            audible.SoundEvent("Footstep", transform.position);
        }

        Transmissible t = collider.GetComponent<Transmissible>();

        if (t && t.type.audibleType)
        {
            Audible.SoundEvent(t.type.audibleType, "Footstep", transform.position);
        }
    }
}

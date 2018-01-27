using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foot : MonoBehaviour
{
    private PlayerController m_player;

	// Use this for initialization
	void Start () {
        m_player = GetComponentInParent<PlayerController>();
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

        if (t && t.type.audibleType && t.progress == 1 && m_player.isGrounded == true && m_player.inFluid == false)
        {
            Audible.SoundEvent(t.type.audibleType, "Footstep", transform.position);
        }
    }
}

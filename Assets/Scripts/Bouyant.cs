using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouyant : MonoBehaviour
{
    [SerializeField]
    private float m_floatySpeed = 5;

    private Rigidbody m_rigidbody;

	// Use this for initialization
	void Start ()
    {
        m_rigidbody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ApplyFloatyForce(Rigidbody source)
    {
        if (source.GetComponent<Collider>().bounds.Contains(transform.position))
        {
            Vector3 velocity = m_rigidbody.velocity;
            velocity.y = Mathf.Lerp(velocity.y, m_floatySpeed, 0.1f);
            m_rigidbody.velocity = velocity;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouyant : MonoBehaviour
{
    [SerializeField]
    private float m_floatySpeed = 5;

    private Rigidbody m_rigidbody;

    private Collider[] m_colliders = new Collider[10];

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
        int count = Physics.OverlapSphereNonAlloc(transform.position, 0.01f, m_colliders);

        for (int i = 0; i < count; i++)
        {
            if (m_colliders[i].attachedRigidbody == m_rigidbody)
            {
                Vector3 velocity = m_rigidbody.velocity;
                velocity.y = Mathf.Lerp(velocity.y, m_floatySpeed, 0.1f);
                m_rigidbody.velocity = velocity;
                break;
            }
                
        }
    }
}

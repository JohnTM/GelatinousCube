using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float m_maxForce = 100;

    [SerializeField]
    private float m_drag = 10;

    [SerializeField]
    private float m_friction = 0.1f;

    [SerializeField]
    private float m_maxVelocity = 5;

    private Rigidbody m_rigidbody;

    private CapsuleCollider m_collider;

    private float m_angle;

    public float angle
    {
        get { return m_angle; }
    }




	// Use this for initialization
	void Start ()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        m_collider = GetComponent<CapsuleCollider>();
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        Vector2 localDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        // TODO: Check grounded state

        Vector3 force = (Camera.main.transform.right * localDir.x + Camera.main.transform.forward * localDir.y);
        force.y = 0;
        force.Normalize();

        m_rigidbody.AddForce(force * m_maxForce);

        Vector3 velocity = m_rigidbody.velocity;
        float vy = velocity.y;
        velocity.y = 0;
        

        if (velocity.magnitude > m_maxVelocity)
        {
            velocity = velocity.normalized * m_maxVelocity;
        }

        m_rigidbody.drag = velocity.magnitude / m_drag;

        if (velocity.magnitude > 0.25f)
        {
            m_angle = Mathf.Atan2(velocity.x, velocity.z) * Mathf.Rad2Deg;
        }

        transform.localEulerAngles = new Vector3(0, m_angle, 0);

        velocity *= m_friction;

        velocity.y = vy;

        m_rigidbody.velocity = velocity;

        
	}
}

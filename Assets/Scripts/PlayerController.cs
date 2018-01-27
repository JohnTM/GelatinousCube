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

    [SerializeField]
    private float m_jumpSpeed = 10;

    [SerializeField]
    private float m_waterJumpSpeed = 5;

    [SerializeField]
    private LayerMask m_groundMask;

    [SerializeField]
    private bool m_fallPreventionEnabled = true;

    private Rigidbody m_rigidbody;

    private CapsuleCollider m_collider;

    private PlayerInput m_input;

    private float m_angle;

    [SerializeField]
    private bool m_grounded;

    public bool isGrounded
    {
        get { return m_ground; }
    }

    private GameObject m_ground;

    private Vector3 m_groundNormal;

    private Animator m_animator;

    private float m_stunTimer;

    public bool inFluid
    {
        get
        {
            if (m_grounded && m_ground)
            {
                Transmissible t = m_ground.GetComponent<Transmissible>();

                if (t && t.progress == 1.0 && t.type.bouyancy > 0)
                {
                    return true;
                }
            }

            return false;
        }
    }


    public float angle
    {
        get { return m_angle; }
    }

	// Use this for initialization
	void Start ()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        m_collider = GetComponent<CapsuleCollider>();
        m_input = GetComponent<PlayerInput>();
        m_animator = GetComponentInChildren<Animator>();
	}
	
    public void ApplyKnockback(float duration, Vector3 impulse)
    {
        m_stunTimer = duration;
        m_rigidbody.AddForce(impulse, ForceMode.Impulse);
    }

	// Update is called once per frame
	void FixedUpdate ()
    {
        bool prevGrounded = m_grounded;

        m_grounded = false;
        m_ground = null;

        // Check if we're floating in something
        Collider[] colliders = Physics.OverlapSphere(m_rigidbody.position, m_collider.radius, m_groundMask);

        foreach (var c in colliders)
        {
            Transmissible t = c.GetComponent<Transmissible>();
            if (t && t.progress == 1 && t.type.bouyancy > 0)
            {
                m_grounded = true;
                m_ground = t.gameObject;
                m_groundNormal = Vector3.up;
            }
        }

        if (m_grounded == false)
        {
            RaycastHit hit;
            if (Physics.SphereCast(m_rigidbody.position, m_collider.radius, Vector3.down, out hit, m_collider.height / 2 - m_collider.radius + 0.01f, m_groundMask, QueryTriggerInteraction.Ignore))
            {
                if (hit.normal.y > 0.5f)
                {
                    m_grounded = true;
                    m_ground = hit.collider.gameObject;
                    m_groundNormal = hit.normal;
                }
            }
        }

        Vector2 localDir = m_input.movement;

        // TODO: Check grounded state

        Vector3 force = (Camera.main.transform.right * localDir.x + Camera.main.transform.forward * localDir.y);

        if (m_stunTimer > 0)
        {
            m_stunTimer -= Time.deltaTime;
            force.x = 0;
            force.z = 0;
        }

        force.y = 0;        

        if (force.magnitude > 0)
        {
            force.Normalize();

            bool abyss = false;
            if (m_fallPreventionEnabled)
            {
                RaycastHit hit;
                if (!Physics.SphereCast(m_rigidbody.position + force * m_collider.radius * 2, m_collider.radius, Vector3.down, out hit, 100, m_groundMask, QueryTriggerInteraction.Ignore))
                {
                    Debug.DrawLine(m_rigidbody.position + force * m_collider.radius * 2, m_rigidbody.position + force * m_collider.radius * 2 + Vector3.down * 100);
                    abyss = true;
                }
                else
                {
                    Transmissible t = hit.collider.GetComponent<Transmissible>();
                    if (t && t.progress == 0)
                    {
                        abyss = true;
                    }
                }
            }

            if (m_grounded)
            {
                Vector3 right = Vector3.Cross(force, Vector3.up);
                force = Vector3.Cross(m_groundNormal, right);
            }

            if (abyss)
            {
                force = Vector3.zero;
            }

            m_rigidbody.AddForce(force * m_maxForce);
        }

        Vector3 velocity = m_rigidbody.velocity;
        float vy = velocity.y;
        velocity.y = 0;
        

        if (velocity.magnitude > m_maxVelocity)
        {
            velocity = velocity.normalized * m_maxVelocity;
        }

        //m_rigidbody.drag = velocity.magnitude / m_drag;

        if (velocity.magnitude > 0.25f && m_stunTimer <= 0)
        {
            m_angle = Mathf.Atan2(velocity.x, velocity.z) * Mathf.Rad2Deg;
        }

        transform.localEulerAngles = new Vector3(0, m_angle, 0);

        velocity *= m_friction;

        if (m_grounded && m_ground)
        {
            if (prevGrounded == false)
            {
                Audible a = m_ground.GetComponent<Audible>();
                if (a)
                {
                    a.SoundEvent("Land", transform.position);
                }

                Transmissible t = m_ground.GetComponent<Transmissible>();
                if (t && t.type.audibleType && t.progress == 1)
                {
                    Audible.SoundEvent(t.type.audibleType, "Land", transform.position);
                }
            }

            if (inFluid)
            {
                if (m_input.wasJumpPressed)
                {
                    velocity.y = Mathf.Lerp(velocity.y, m_waterJumpSpeed, 0.25f);
                }
                else
                {
                    velocity.y = vy;
                }

                m_animator.SetFloat("Speed", 0.0f);
                m_animator.SetFloat("Speed2", 0.1f);
            }
            else
            {
                velocity.y = 0;

                if (m_input.wasJumpPressed)
                {
                    m_grounded = false;
                    // TODO: check water
                    velocity.y = m_jumpSpeed;
                    m_animator.SetTrigger("Jump");
                }

                m_animator.SetFloat("Speed", velocity.magnitude / m_maxVelocity);
                m_animator.SetFloat("Speed2", 0.1f + (velocity.magnitude / m_maxVelocity) * 1.5f);
            }
        }
        else
        {
            velocity.y = vy;

            m_animator.SetFloat("Speed", 0.5f);
            m_animator.SetFloat("Speed2", 0.1f);
        }

        m_animator.SetFloat("YSpeed", velocity.y);
        m_animator.SetBool("Grounded", m_grounded && !inFluid); 

        m_rigidbody.velocity = velocity;        
	}
}

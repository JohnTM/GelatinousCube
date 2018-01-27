using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TransmissibleEvent : UnityEvent<Transmissible>
{ }


[RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(Collider)), RequireComponent(typeof(Highlightable))]
public class Transmissible : MonoBehaviour
{
    public delegate void OnTransmissibleComplete(Transmissible t);

    public UnityEvent onFilled;

    [SerializeField]
    private TransmissibleType m_type;

    public TransmissibleType type
    {
        get { return m_type; }   
        set
        {
            m_type = value;
            foreach (var r in m_renderers)
            {
                r.material = m_type.material;
                r.material.SetFloat("_Height", r.bounds.size.y * 1.35f);
            }

        }     
    }

    [SerializeField]
    private float m_progress = 1.0f;

    public float progress
    {
        get { return m_progress; }
    }


    private Renderer[] m_renderers;
    private Collider[] m_colliders;
    private Rigidbody m_rigidbody;

    public enum State
    {
        Full,
        Draining,
        DrainCancelled,
        Infusing,
        InfuseCancelled,
        Empty
    }

    private State m_state;

    public State state
    {
        get { return m_state; }
    }

    private OnTransmissibleComplete m_callback;

    // Use this for initialization
    void Start()
    {
        m_renderers = GetComponentsInChildren<Renderer>();
        m_colliders = GetComponentsInChildren<Collider>();
        m_rigidbody = GetComponent<Rigidbody>();

        type = m_type;

        if (m_progress == 0)
        {
            m_state = State.Empty;
        }    
        else if (m_progress == 1)
        {
            m_state = State.Full;
        }
    }   
	
    public void MakeEmpty()
    {
        m_state = State.Empty;
        m_callback = null;
        m_progress = 0.0f;
    }

    public TransmissibleType BeginDrain(OnTransmissibleComplete callback)
    {
        if (m_state == State.Full)
        {
            m_state = State.Draining;
            m_callback = callback;
            return m_type;
        }
        return null;
    }

    public void EndDrain()
    {
        if (m_state == State.Draining && m_progress > 0.2f)
        {
            m_state = State.DrainCancelled;
        }
    }

    public bool BeginInfuse(TransmissibleType type, OnTransmissibleComplete callback)
    {
        if (m_state == State.Empty)
        {
            this.type = type;
            m_state = State.Infusing;
            m_callback = callback;
            return true;
        }
        return false;
    }

    public void EndInfuse()
    {
        if (m_state == State.Infusing && m_progress < 0.8f)
        {
            m_state = State.InfuseCancelled;
        }
    }

	// Update is called once per frame
	void Update ()
    {
        // Update appearance
		foreach (var r in m_renderers)
        {
            r.material.SetFloat("_Progress", m_progress);  
            if (m_type.animateUVs != Vector2.zero)
            {
                Vector2 offset = r.material.GetTextureOffset("_MainTex");
                offset += m_type.animateUVs * Time.deltaTime;
                r.material.SetTextureOffset("_MainTex", offset);
            }    
        }

        float drainDelta = 1.0f / m_type.timeToDrain * Time.deltaTime;

        switch (m_state)
        {
            case State.Infusing:
                m_progress = Mathf.Clamp01(m_progress + drainDelta);
                if (m_progress == 1.0f)
                {
                    m_callback(this);
                    m_callback = null;
                    m_state = State.Full;
                    if (onFilled != null)
                    {
                        onFilled.Invoke();
                    }
                }
                break;
            case State.Draining:
                m_progress = Mathf.Clamp01(m_progress - drainDelta);
                if (m_progress == 0.0f)
                {
                    m_callback(this);
                    m_callback = null;
                    m_state = State.Empty;
                }
                break;
            case State.InfuseCancelled:
                m_progress = Mathf.Clamp01(m_progress - drainDelta * 2.0f);
                if (m_progress == 0.0f) m_state = State.Empty;
                break;
            case State.DrainCancelled:
                m_progress = Mathf.Clamp01(m_progress + drainDelta * 2.0f);
                if (m_progress == 1.0f) m_state = State.Full;
                break;
        }

        // Inert properties
        if (m_progress < 1.0f)
        {
            foreach (var c in m_colliders)
            {
                c.isTrigger = true;
            }

            if (m_rigidbody)
            {
                m_rigidbody.isKinematic = true;
            }
        }   
        else if (m_progress == 1.0f)
        {
            foreach (var c in m_colliders)
            {
                c.isTrigger = m_type.hasCollisions == false || m_type.bouyancy > 0;
            }

            if (m_rigidbody)
            {
                m_rigidbody.isKinematic = !m_type.hasRigidbody;
                //m_rigidbody.SetDensity(m_type.density);
            }
        }        
              
	}

    void OnTriggerStay(Collider c)
    {
        Bouyant b = c.gameObject.GetComponent<Bouyant>();
        if (b && m_type.bouyancy > 0 && m_progress > 0.0f)
        {
            b.ApplyFloatyForce(m_rigidbody);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transmissible : MonoBehaviour
{
    public delegate void OnTransmissibleComplete(Transmissible t);

    [SerializeField]
    private TransmissibleType m_type;

    public TransmissibleType type
    {
        get { return m_type; }        
    }

    [SerializeField]
    private float m_progress = 1.0f;

    private Renderer[] m_renderers;

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

        foreach (var r in m_renderers)
        {
            r.material = m_type.material;
        }

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
        if (m_state == State.Draining)
        {
            m_state = State.DrainCancelled;
        }
    }

    public bool BeginInfuse(TransmissibleType type, OnTransmissibleComplete callback)
    {
        if (m_state == State.Empty)
        {
            m_type = type;
            foreach (var r in m_renderers)
            {
                r.material = m_type.material;
            }
            m_state = State.Infusing;
            m_callback = callback;
            return true;
        }
        return false;
    }

    public void EndInfuse()
    {
        if (m_state == State.Infusing)
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

	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powered : MonoBehaviour
{
    [SerializeField]
    protected bool m_recievesPower;

    public bool recievesPower
    {
        get { return m_recievesPower; }
    }

    [SerializeField]
    protected bool m_providesPower;

    public bool providesPower
    {
        get { return m_providesPower; }
    }

    [SerializeField]
    protected float m_internalPowerLevel;

    public float internalPowerLevel
    {
        get { return m_internalPowerLevel; }
        set { m_internalPowerLevel = value; }
    }

    [SerializeField]
    protected float m_powerLevel;

    [SerializeField]
    protected List<Powered> m_sources = new List<Powered>();

    public float powerLevel
    {
        get { return m_powerLevel; }
    }


    // Use this for initialization
    protected virtual void Start()
    {
        if (m_providesPower)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 0.1f);
            foreach (var c in colliders)
            {
                Powered p = c.GetComponentInParent<Powered>();                
                if (p && p != this && p.m_recievesPower)
                {
                    Debug.Log(p.gameObject.name);
                    p.m_sources.Add(this);
                    break;
                }
            }
        }
    }

    public void AddSource(Powered powered)
    {
        m_sources.Add(powered);
    }
	
    void UpdatePowerLevel()
    {
        m_powerLevel = m_internalPowerLevel;

        foreach (Powered p in m_sources)
        {
            if (p.m_providesPower && m_recievesPower && p.m_powerLevel > 0)
            {
                m_powerLevel = Mathf.Max(m_powerLevel, p.m_powerLevel);
            }
        }
    }

    protected virtual void UpdateInternalPowerLevel()
    {

    }

	// Update is called once per frame
	protected virtual void Update ()
    {
        UpdateInternalPowerLevel();
        UpdatePowerLevel();
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : Powered
{
    [SerializeField]
    private Transform m_switchModel;

    private int m_active;

	// Use this for initialization
	protected override void Start ()
    {
        base.Start();	
	}

    // Update is called once per frame
    protected override void Update ()
    {
        base.Update();
	}

    protected override void UpdateInternalPowerLevel()
    {
        m_internalPowerLevel = (m_active > 0) ? 1 : 0;    
    }

    void OnTriggerEnter(Collider c)
    {
        m_active++;
    }

    void OnTriggerExit(Collider c)
    {
        m_active--;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class Wire : Powered
{
    [SerializeField]
    private WireEnd m_terminus;

    [SerializeField]
    private Material m_unpoweredMaterial;

    [SerializeField]
    private Material m_poweredMaterial;

    private LineRenderer m_lineRenderer;

    void Awake()
    {
       
    }

    protected override void Start()
    {        
        if (Application.isPlaying)
        {
            base.Start();
            //m_terminus.AddSource(this);

            if (m_recievesPower)
            {
                Collider[] colliders = Physics.OverlapSphere(transform.position, 0.1f);
                foreach (var c in colliders)
                {
                    Powered p = c.GetComponentInParent<Powered>();
                    if (p && p != this && p.providesPower)
                    {
                        m_sources.Add(p);
                        break;
                    }
                }
            }
        }
    }

    protected override void UpdateInternalPowerLevel()
    {
        m_terminus.internalPowerLevel = m_powerLevel;
    }

    protected override void Update()
    {
        base.Update();

        m_lineRenderer = GetComponent<LineRenderer>();
        if (m_lineRenderer && m_terminus && m_poweredMaterial && m_unpoweredMaterial)
        {
            m_lineRenderer.positionCount = 2;
            m_lineRenderer.SetPosition(0, transform.position);
            m_lineRenderer.SetPosition(1, m_terminus.transform.position);

            m_lineRenderer.material = m_powerLevel > 0 ? m_poweredMaterial : m_unpoweredMaterial;
        }
    }
}

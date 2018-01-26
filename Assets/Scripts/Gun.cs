using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField]
    private Transform m_emitter;

    [SerializeField]
    private LayerMask m_detectionMask;

    [SerializeField]
    private float m_detectionDistance = 3.0f;

    [SerializeField]
    private float m_detectionSpread = 30.0f;

    [SerializeField]
    private int m_rays = 10;

    [SerializeField]
    private Transmissible m_tank;

    private Transmissible m_target;

    private TransmissibleType m_currentType;

    public bool isDrainPressed
    {
        get { return Input.GetMouseButton(0); }
    }

    public bool isInfusePressed
    {
        get { return Input.GetMouseButton(1); }
    }

    // Use this for initialization
    void Start () {
		
	}

    public void DetectObjects()
    {
        if (m_target)
        {
            m_target.GetComponent<Highlightable>().highlighted = false;
        }

        Collider[] objects = Physics.OverlapSphere(transform.position, m_detectionDistance, m_detectionMask);

        Transmissible minTarget = null;
        float minDist = 0;

        foreach (var p in objects)
        {
            Vector3 dir = (p.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
            float diff = Mathf.DeltaAngle(GetComponent<PlayerController>().angle, angle);

            if (Mathf.Abs(diff) < m_detectionSpread)
            {
                float dist = Vector3.Distance(p.transform.position, transform.position);

                if (minTarget == null || dist < minDist)
                {
                    minTarget = p.GetComponent<Transmissible>();
                    minDist = dist;
                }
            }
        }

        if (m_target != minTarget)
        {
            if (m_target)
            {
                if (m_target.state == Transmissible.State.Infusing)
                {
                    m_target.EndInfuse();
                    m_tank.EndDrain();
                }
                else if (m_target.state == Transmissible.State.Draining)
                {
                    m_target.EndDrain();
                    m_tank.EndInfuse();
                }
            }
        }

        if (minTarget)
        {
            m_target = minTarget;
            m_target.GetComponent<Highlightable>().highlighted = true;
        }
        else
        {
            m_target = null;
        }

        if (isDrainPressed && m_target && m_currentType == null)
        {
            if (m_target.state == Transmissible.State.Full)
            {
                TransmissibleType type = m_target.BeginDrain((Transmissible t) =>
                {
                    m_currentType = m_target.type;
                });

                m_tank.BeginInfuse(m_target.type, (Transmissible t) => { });
            }
        }
        else if (!isDrainPressed && m_target && m_target.state == Transmissible.State.Draining)
        {
            m_target.EndDrain();
            m_tank.EndInfuse();
        }
        else if (isInfusePressed && m_target && m_target.state == Transmissible.State.Empty && m_currentType != null)
        {
            m_target.BeginInfuse(m_currentType, (Transmissible t) =>
            {
                // TODO: update visuals
                m_currentType = null;
            });
            m_tank.BeginDrain((Transmissible t) => { });
        }
        else if (!isInfusePressed && m_target && m_target.state == Transmissible.State.Infusing)
        {
            m_target.EndInfuse();
            m_tank.EndDrain();
        }         

        //m_prevObjects = objects;
    }

	// Update is called once per frame
	void FixedUpdate ()
    {
        DetectObjects();	
	}
}

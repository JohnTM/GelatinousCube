using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField]
    private Transform m_emitter;

    [SerializeField]
    private Transform m_gunModel;

    [SerializeField]
    private LayerMask m_detectionMask;

    [SerializeField]
    private float m_detectionDistance = 3.0f;

    [SerializeField]
    private float m_verticalDetectionDistance = 1.0f;

    [SerializeField]
    private float m_detectionSpread = 30.0f;

    [SerializeField]
    private int m_rays = 10;

    [SerializeField]
    private Transmissible m_tank;

    private Transmissible m_target;
    private Vector3 m_targetHitPoint;
    private Vector3 m_targetHitNormal;

    private TransmissibleType m_currentType;

    [SerializeField]
    private LineRenderer m_lineRenderer;

    public bool isDrainPressed
    {
        get { return Input.GetMouseButton(0); }
    }

    public bool isInfusePressed
    {
        get { return Input.GetMouseButton(1); }
    }

    private Vector3 m_gunScale;

    // Use this for initialization
    void Start () {
        m_gunScale = m_gunModel.localScale;
	}

    public void DetectObjects()
    {
        if (m_target)
        {
            m_target.GetComponent<Highlightable>().highlighted = false;
        }

        Collider[] objects = Physics.OverlapSphere(transform.position, m_detectionDistance, m_detectionMask, QueryTriggerInteraction.Collide);

        Transmissible minTarget = null;
        float minDist = 0;

        foreach (var p in objects)
        {
            Vector3 dir = (p.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
            float diff = Mathf.DeltaAngle(GetComponent<PlayerController>().angle, angle);
            float vdist = p.transform.position.y - transform.position.y;

            if (p.GetComponent<Transmissible>() == null) continue;
            if (p.GetComponent<Collider>().bounds.Intersects(GetComponent<Collider>().bounds)) continue;

            RaycastHit hit;
            if (Physics.Raycast(m_emitter.position, dir, out hit, 1000, m_detectionMask))
            {
                float dist = Vector3.Distance(p.transform.position, transform.position);

                if (minTarget == null || dist < minDist)
                {
                    minTarget = p.GetComponent<Transmissible>();
                    minDist = dist;
                    m_targetHitPoint = minTarget.transform.InverseTransformPoint(hit.point);
                    m_targetHitNormal = hit.normal;
                }
            }

            if (Mathf.Abs(diff) < m_detectionSpread || (vdist < 0 && Mathf.Abs(vdist) < m_verticalDetectionDistance))
            {
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

    Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;
        Vector3 p = uuu * p0;
        p += 3 * uu * t * p1;
        p += 3 * u * tt * p2;
        p += ttt * p3;
        return p;
    }

    void LateUpdate()
    {
        if (m_target && m_target.progress > 0 && m_target.progress < 1)
        {
            float direction = (m_target.state == Transmissible.State.Draining || m_target.state == Transmissible.State.InfuseCancelled) ? 1 : -1;
            int segments = 20;
            

            m_lineRenderer.enabled = true;
            m_lineRenderer.positionCount = segments;

            Vector3 p0 = m_emitter.position;
            Vector3 p1 = m_emitter.position + m_emitter.forward * 0.5f;
            Vector3 p2 = m_target.transform.TransformPoint(m_targetHitPoint) + m_targetHitNormal * 0.5f;
            Vector3 p3 = m_target.transform.TransformPoint(m_targetHitPoint);

            for (int i = 0; i < segments; i++)
            {
                float t1 = (float)i / segments;
                float t2 = (float)(i + 1) / segments;

                m_lineRenderer.SetPosition(i, CalculateBezierPoint(t1, p0, p1, p2, p3));
                m_lineRenderer.SetPosition(i, CalculateBezierPoint(t2, p0, p1, p2, p3));
            }
            float distance = Vector3.Distance(p0, p3);

            // TODO: cache this
            Keyframe[] widthKeys = new Keyframe[segments];            
            for (int i = 0; i < segments; i++)
            {
                widthKeys[i].time = (float)i / segments;
                widthKeys[i].value = Mathf.Abs(Mathf.Sin(((float)i / segments * Mathf.PI * distance) + Time.timeSinceLevelLoad * 10.0f * direction)) * 0.25f + 0.1f;
            }
            AnimationCurve curve = new AnimationCurve(widthKeys);
            m_lineRenderer.widthCurve = curve;

            m_gunModel.localScale = m_gunScale * (1.0f + 0.25f * Mathf.Abs(Mathf.Sin(Time.timeSinceLevelLoad * 10.0f * direction)));

            Color c = m_target.type.material.color; ;
            //c.a = 1.0f;

            m_lineRenderer.startColor = c;
            m_lineRenderer.endColor = c;
        }
        else
        {
            m_lineRenderer.enabled = false;
        }
    }

	// Update is called once per frame
	void FixedUpdate ()
    {
        DetectObjects();	
	}
}

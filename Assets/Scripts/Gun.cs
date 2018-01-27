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

    [SerializeField]
    private AnimationCurve m_soundCurve;

    private PlayerInput m_input;

    private Vector3 m_gunScale;

    private AudioSource m_source;

    private Animator m_animator;

    // Use this for initialization
    void Start () {
        m_gunScale = m_gunModel.localScale;
        m_input = GetComponent<PlayerInput>();
        m_source = GetComponent<AudioSource>();
        m_animator = GetComponentInChildren<Animator>();
	}

    Transmissible DetectObject()
    {
        Vector3 origin = transform.position;
        Vector3 dir = transform.forward;

        Transmissible minTarget = null;
        float minAngle = 0;

        for (int i = 0; i < m_rays; i++)
        {
            float rayAngle = -m_detectionSpread + (m_detectionSpread * 2.0f) * (i / (float)m_rays);
            Vector3 dir2 = Quaternion.AngleAxis(rayAngle, Vector3.up) * dir;
            Vector3 hitPoint = origin + dir2 * m_detectionDistance;
            Color hitColor = Color.gray;

            RaycastHit hit;
            if (Physics.Raycast(origin, dir2, out hit, m_detectionDistance, m_detectionMask))
            {
                Transmissible t = hit.collider.gameObject.GetComponent<Transmissible>();
                if (t)
                {
                    hitPoint = hit.point;

                    if (minTarget == null || Mathf.Abs(rayAngle) < minAngle)
                    {
                        minTarget = t;
                        minAngle = Mathf.Abs(rayAngle);
                        hitColor = Color.yellow;
                        m_targetHitPoint = minTarget.transform.InverseTransformPoint(hit.point);
                        m_targetHitNormal = hit.normal;
                    }
                }

            }
            Debug.DrawLine(origin, hitPoint, hitColor);
        }

        if (minTarget == null)
        {
            minAngle = 0;

            for (int i = 0; i < m_rays; i++)
            {
                float rayAngle = -m_detectionSpread + (m_detectionSpread * 2.0f) * (i / (float)m_rays);
                Vector3 dir2 = Quaternion.AngleAxis(30, transform.right) * Quaternion.AngleAxis(rayAngle, Vector3.up) * dir;
                Vector3 hitPoint = origin + dir2 * m_detectionDistance;
                Color hitColor = Color.gray;

                RaycastHit hit;
                if (Physics.Raycast(origin, dir2, out hit, m_detectionDistance, m_detectionMask))
                {
                    Transmissible t = hit.collider.gameObject.GetComponent<Transmissible>();
                    if (t)
                    {
                        hitPoint = hit.point;

                        if (minTarget == null || Mathf.Abs(rayAngle) < minAngle)
                        {
                            minTarget = t;
                            minAngle = Mathf.Abs(rayAngle);
                            hitColor = Color.yellow;
                            m_targetHitPoint = minTarget.transform.InverseTransformPoint(hit.point);
                            m_targetHitNormal = hit.normal;
                        }
                    }

                }
                Debug.DrawLine(origin, hitPoint, hitColor);
            }
        }

        if (minTarget)
        {
            // Reject direct down hits
            RaycastHit hit;
            if (Physics.Raycast(origin, -Vector3.up, out hit, m_detectionDistance, m_detectionMask))
            {
                if (hit.collider.GetComponent<Transmissible>() == minTarget)
                {
                    minTarget = null;
                }
            }

        }


        return minTarget;
    }

    public void DetectObjects()
    {
        if (m_target)
        {
            m_target.GetComponent<Highlightable>().highlighted = false;
        }

        Transmissible newTarget = DetectObject();

        if (m_target != newTarget)
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

        if (newTarget)
        {
            m_target = newTarget;
            m_target.GetComponent<Highlightable>().highlighted = true;

            m_source.pitch = m_target.progress + 0.5f;
            m_source.volume = m_soundCurve.Evaluate(m_target.progress);
        }
        else
        {
            m_target = null;
        }

        

        if (m_input.isDrainPressed && m_target && m_currentType == null)
        {
            if (m_target.state == Transmissible.State.Full)
            {
                m_source.clip = m_target.type.drainClip;
                m_source.Play();

                TransmissibleType type = m_target.BeginDrain((Transmissible t) =>
                {
                    m_currentType = t.type;                    
                });

                m_tank.BeginInfuse(m_target.type, (Transmissible t) => { });
            }
        }
        else if (!m_input.isDrainPressed && m_target && m_target.state == Transmissible.State.Draining)
        {
            m_target.EndDrain();
            m_tank.EndInfuse();            
        }
        else if (m_input.isInfusePressed && m_target && m_target.state == Transmissible.State.Empty && m_currentType != null)
        {
            m_source.clip = m_target.type.infuseClip;
            m_source.Play();

            m_target.BeginInfuse(m_currentType, (Transmissible t) =>
            {
                // TODO: update visuals
                m_currentType = null;
            });
            m_tank.BeginDrain((Transmissible t) => { });
        }
        else if (!m_input.isInfusePressed && m_target && m_target.state == Transmissible.State.Infusing)
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
            Vector3 p1 = m_emitter.position + m_emitter.up * 1.5f;
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
                widthKeys[i].value = Mathf.Abs(Mathf.Sin(((float)i / segments * Mathf.PI * distance) + Time.timeSinceLevelLoad * 10.0f * direction)) * 0.25f + 0.1f + ((float)i/segments * 0.5f);
            }
            AnimationCurve curve = new AnimationCurve(widthKeys);
            m_lineRenderer.widthCurve = curve;

            float scaleAmount = (1.0f + 0.5f * Mathf.Abs(Mathf.Sin(Time.timeSinceLevelLoad * 10.0f * direction)));
            m_gunModel.localScale = new Vector3(m_gunScale.x * scaleAmount, m_gunScale.y, m_gunScale.z * scaleAmount); 
            m_gunModel.gameObject.SetActive(true);
            m_animator.SetBool("Sucking", true);
            m_animator.SetLayerWeight(1, 1.0f);

            Color c = m_target.type.material.color; ;
            //c.a = 1.0f;

            
            m_lineRenderer.material.SetColor("_EmissionColor", m_tank.type.material.GetColor("_EmissionColor") * m_tank.type.material.GetFloat("_Emission") * 0.5f);
            m_lineRenderer.material.color = m_tank.type.beamColor;
            //m_lineRenderer.startColor = m_tank.type.beamColor;
            //m_lineRenderer.endColor = m_tank.type.beamColor;
        }
        else
        {
            m_lineRenderer.enabled = false;
            m_gunModel.gameObject.SetActive(false);
            m_animator.SetBool("Sucking", false);
            m_animator.SetLayerWeight(1, 0.0f);
        }
    }

	// Update is called once per frame
	void FixedUpdate ()
    {
        DetectObjects();	
	}
}

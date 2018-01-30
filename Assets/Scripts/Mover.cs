using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : Powered
{
    [SerializeField]
    private Transform m_target;
    private Rigidbody m_rigidbody;

    [SerializeField]
    private Vector3 m_finalPosition;

    [SerializeField]
    private Vector3 m_finalRotation;

    [SerializeField]
    private float m_duration = 3.0f;

    [SerializeField]
    private float m_state = 0.0f;

    [SerializeField]
    private bool m_revertWhenUnpowered;

    [SerializeField]
    private AnimationCurve m_easingCurve;

    private Vector3 m_initialRotation;
    private Vector3 m_initialPosition;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        m_initialRotation = m_target.transform.localEulerAngles;
        m_initialPosition = m_target.transform.localPosition;
        m_rigidbody = m_target.GetComponent<Rigidbody>();
	}

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        
        if (m_revertWhenUnpowered && m_powerLevel == 0)
        {
            m_state -= Time.deltaTime / m_duration;
            m_state = Mathf.Clamp01(m_state);
        }

        m_state += (m_powerLevel * Time.deltaTime / m_duration);


        Vector3 localPosition = Vector3.Lerp(m_initialPosition, m_finalPosition, m_easingCurve.Evaluate(m_state)); // );
        Quaternion localRotation = Quaternion.Euler(Vector3.Lerp(m_initialRotation, m_finalRotation, m_easingCurve.Evaluate(m_state))); // ));

        if (m_rigidbody)
        {
            m_rigidbody.MovePosition(transform.TransformPoint(localPosition));
            m_rigidbody.MoveRotation(transform.rotation * localRotation);
        }
        else
        {
            m_target.localPosition = localPosition;
            m_target.localRotation = localRotation;
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transmissible : MonoBehaviour
{
    [SerializeField]
    private TransmissibleType m_type;

    [SerializeField]
    private float m_state = 1.0f;

    private Renderer[] m_renderers;

    // Use this for initialization
    void Start()
    {
        m_renderers = GetComponentsInChildren<Renderer>();

        foreach (var r in m_renderers)
        {
            r.material = m_type.material;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        // Update appearance
		foreach (var r in m_renderers)
        {
            r.material.SetFloat("_Progress", m_state);      
        }
	}
}

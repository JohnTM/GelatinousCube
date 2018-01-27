using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

public class Highlightable : MonoBehaviour {

    private Outline m_outline;

    [SerializeField]
    private bool m_highlighted = false;

    public bool highlighted
    {
        get { return m_highlighted; }
        set { m_highlighted = value; }
    }

	// Use this for initialization
	void Start () {
        m_outline = GetComponent<Outline>();
	}
	
	// Update is called once per frame
	void Update () {
        m_outline.enabled = m_highlighted;
        //GetComponent<Renderer>().material.SetFloat("_Highlight", m_highlighted ? 1 : 0);
	}
}

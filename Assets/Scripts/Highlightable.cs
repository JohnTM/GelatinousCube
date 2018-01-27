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
        m_outline.enabled = true;
        //ADAM EDIT
        //m_outline.color = m_highlighted ? 0 : 1;
        //ADAM EDIT
        //GetComponent<Renderer>().material.SetFloat("_Highlight", m_highlighted ? 1 : 0);
	}
}

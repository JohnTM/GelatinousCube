using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class CreditsCrawl : MonoBehaviour {

    public float deltaY = 1;
    public float duration = 20;

    private float m_timer;

    private bool alreadyPressed;

	// Use this for initialization
	void Start () {
        m_timer = duration;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position += Vector3.up * deltaY * Time.deltaTime;
        
        if (m_timer > 0)
        {
            m_timer -= Time.deltaTime;
            if (m_timer <= 0)
            {
                alreadyPressed = true;
                Finished();
                return;
            }            
        }

        bool startPressed = InputManager.ActiveDevice != null && InputManager.ActiveDevice.Action1.WasPressed;

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return))
        {
            startPressed = true;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (startPressed && !alreadyPressed)
        {
            alreadyPressed = true;
            Finished();

        }
    }

    void Finished()
    {
        StartAndEndEffects startAndEndEffects = FindObjectOfType<StartAndEndEffects>();        
        startAndEndEffects.EndLevel();
    }
}

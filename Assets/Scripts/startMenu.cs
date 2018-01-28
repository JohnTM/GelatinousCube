using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class startMenu : MonoBehaviour {

    bool alreadyPressed = false;
    public StartAndEndEffects startAndEndEffects;
    public AudioClip startSound;

    // Update is called once per frame
    void Update()
    {
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
            Invoke("StartGame", 1.0f);
            if (startSound != null)
            {
                AudioSource audioSource = GetComponent<AudioSource>();
                audioSource.clip = startSound;
                audioSource.Play();
            }
        }
    }

    void StartGame()
    {
        startAndEndEffects.EndLevel();
    }
}

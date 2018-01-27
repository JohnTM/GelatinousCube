using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class exitGate : MonoBehaviour {

    public GameObject activationLight;
    public AudioClip activationSound;
    public GameObject gateModel;
    bool teleporterActive = false;
    bool teleporterAlreadyActivated = false;
    public StartAndEndEffects startAndEndEffectsController;

    public void ActivateTeleporter()
    {
        if (teleporterActive)
            return;
        teleporterActive = true;
        if (activationLight != null)
        {
            activationLight.SetActive(true);
        }
        if (activationSound != null)
        {
            AudioSource.PlayClipAtPoint(activationSound, Camera.main.transform.position);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if ((other.tag == "Player") && teleporterActive && !teleporterAlreadyActivated)
        {
            if (startAndEndEffectsController != null)
            {
                teleporterAlreadyActivated = true;
                startAndEndEffectsController.EndLevel();
            }
        }
    }



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetScene : MonoBehaviour
{
    void OnTriggerEnter(Collider c)
    {
        if (c.tag == "Player")
        {
            StartAndEndEffects startAndEndEffects = FindObjectOfType<StartAndEndEffects>();
            startAndEndEffects.nextLevel = SceneManager.GetActiveScene().name;
            startAndEndEffects.EndLevel();
        }
    }
}

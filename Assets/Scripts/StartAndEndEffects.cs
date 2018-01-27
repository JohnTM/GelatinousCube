using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartAndEndEffects : MonoBehaviour {

    public Material UIEnergyEffect;
    public float changeTime;
    public string nextLevel;
    bool endingLevel = false;
    public bool playStartScreenEffect = true;

	// Use this for initialization
	void Start () {
        if (playStartScreenEffect)
            StartCoroutine(ScreenEffect(0f, 1f));
	}

    IEnumerator ScreenEffect(float startValue, float endValue)
    {
        float lerpAmount = 0f;
        {
            while (lerpAmount < 1f)
            {
                lerpAmount += Time.deltaTime / changeTime;
                UIEnergyEffect.SetFloat("_Cutoff", Mathf.Lerp(startValue, endValue, lerpAmount));
                yield return new WaitForEndOfFrame();
            }
        }
        if (endingLevel)
            SceneManager.LoadScene(nextLevel);
        yield return null;
    }
	
	// Update is called once per frame
	public void EndLevel () {
        StartCoroutine(ScreenEffect(1f, 0f));
        endingLevel = true;
    }

    void OnDestroy()
    {
        UIEnergyEffect.SetFloat("_Cutoff", 1f);
    }



}

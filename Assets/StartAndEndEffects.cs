using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAndEndEffects : MonoBehaviour {

    public Material UIEnergyEffect;
    public float changeTime;

	// Use this for initialization
	void Start () {
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
        yield return null;
    }
	
	// Update is called once per frame
	public void EndLevel () {
        StartCoroutine(ScreenEffect(1f, 0f));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugzapperScript : MonoBehaviour {

    public GameObject swarmEffect;
    public ParticleSystem bugsPS;
    public AudioSource bugsAudio;
    public GameObject zapEffects;
    public float zapTime = 2f;
    bool alreadyUsed = false;

    public void ZapperCharged()
    {
        Transmissible thisTransmissable = GetComponent<Transmissible>();
        if (thisTransmissable.type.name == "Energy" && !alreadyUsed)
        {
            Debug.Log("Charged with energy!");
            StartCoroutine(ZapperActive());

        }
    }

    IEnumerator ZapperActive()
    {
        Vector3 swarmStartPos = bugsPS.gameObject.transform.position;
        Vector3 targetPos = transform.position;
        float startVolume = bugsAudio.volume;
        bugsPS.Stop();
        float lerpAmount = 0f;
        zapEffects.SetActive(true);
        while (lerpAmount < 1)
        {
            bugsPS.gameObject.transform.position = Vector3.Lerp(swarmStartPos, targetPos, lerpAmount);
            bugsAudio.volume = Mathf.Lerp(startVolume, 0f, lerpAmount);
            lerpAmount += Time.deltaTime / zapTime;
            yield return new WaitForEndOfFrame();
        }
        zapEffects.SetActive(false);
        swarmEffect.SetActive(false);
        alreadyUsed = true;
        yield return null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsectSwarm : MonoBehaviour {

    private float lastKnockbackTime;
    public float timeBetweenKnockbacks = 0.3f;
    public float knockbackForce = 100f;
    public Transform knockbackOrigin;

    public List<AudioClip> hurtSounds = new List<AudioClip>();

    void Start()
    {
        if (knockbackOrigin == null)
        {
            knockbackOrigin = transform;
        }
    }
    
    void OnTriggerStay(Collider other)
    {
        if (Time.time > (lastKnockbackTime + timeBetweenKnockbacks) && (other.tag == "Player"))
        {
            Rigidbody playerRB = other.GetComponent<Rigidbody>();
            {
                if (playerRB != null)
                {
                    playerRB.velocity = Vector3.zero;
                    Vector3 knockbackVector = new Vector3(other.transform.position.x - knockbackOrigin.position.x, 0, other.transform.position.z - knockbackOrigin.position.z) + (Vector3.up * 0.1f);
                    playerRB.AddForce(knockbackVector * knockbackForce, ForceMode.Impulse);
                    AudioSource.PlayClipAtPoint(hurtSounds[Random.Range(0, hurtSounds.Count - 1)], other.transform.position);
                    lastKnockbackTime = Time.time;
                }
            }
        }
    }

	
	// Update is called once per frame
	void Update () {
		
	}
}

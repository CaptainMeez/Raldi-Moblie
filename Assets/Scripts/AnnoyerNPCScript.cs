using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AI;

public class AnnoyerNPCScript : MonoBehaviour
{
    private NavMeshAgent agent;
	private AILocationSelectorScript wanderer;

    public AudioClip hit;

	private void Awake()
	{
		wanderer = FindObjectOfType<AILocationSelectorScript>();
		agent = GetComponent<NavMeshAgent>();

		Wander();
	}

	private void FixedUpdate()
	{
		if (agent.velocity.magnitude <= 1f)
			Wander();
	}

	private void Wander()
	{
		wanderer.GetNewTarget();
		agent.SetDestination(wanderer.transform.position);
	}
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
            base.GetComponent<AudioSource>().PlayOneShot(hit);
    }
}

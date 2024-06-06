using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour
{
	private float speed = 120;
	private float timeActive = 0f;
	private float lifeSpan;

	private Rigidbody rb;
	public UnlockedPlayerScript player;

	private void Start()
	{
		speed = speed * (Vector3.Distance(base.transform.position, FindObjectOfType<UnlockedPlayerScript>().transform.position) / 40); // Speed up by distance from player
		rb = base.GetComponent<Rigidbody>();
		rb.velocity = base.transform.forward * speed;
		lifeSpan = 30f;
	}

	private void Update()
	{
		rb.velocity = base.transform.forward * speed;
		lifeSpan -= Time.deltaTime;
		timeActive += Time.deltaTime;

		if (lifeSpan < 0f)
			UnityEngine.Object.Destroy(base.gameObject, 0f);
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameControllerScript.current.PlayerDamage(4);
            UnityEngine.Object.Destroy(base.gameObject, 0f);
        }
		else if (other.tag != "NPC" && other.tag != "NeilShield")
		{
			UnityEngine.Object.Destroy(base.gameObject, 0f);
		}
    }
}

using System;
using UnityEngine;

// Token: 0x020000B4 RID: 180
public class PearlScript : MonoBehaviour
{
	public GameControllerScript controller;

	public float speed;
	private float lifeSpan;
	public GameObject teleport;
	private bool alreadychecked = false;
	
	private int checks = 0;

	private Rigidbody rb;

	private void Awake()
	{
		controller = GameControllerScript.current;
	}

	private void Start()
	{
		rb = base.GetComponent<Rigidbody>();
		rb.velocity = (base.transform.forward * speed) + (base.transform.up * 10);
		lifeSpan = 30f;
	}

	private void Update()
	{
		//rb.velocity += base.transform.forward * speed;
		lifeSpan -= Time.deltaTime;

		if (lifeSpan < 0f)
			UnityEngine.Object.Destroy(base.gameObject, 0f);
	}

	private void OnTriggerEnter(Collider other)
	{
        bool notTrigger = false;
		if (!other.gameObject.name.ToLower().Contains("player") && !alreadychecked)
		{
            if (other.GetComponent<BoxCollider>() != null)
                notTrigger = !other.GetComponent<BoxCollider>().isTrigger;
            if (other.GetComponent<MeshCollider>() != null)
                notTrigger = !other.GetComponent<MeshCollider>().isTrigger;
            if (notTrigger)
            {
				if (!other.gameObject.name.ToLower().Contains("desk") && !other.gameObject.name.ToLower().Contains("combinedmesh"))
				{
					if (GameControllerScript.current.player.jumpRope)
						GameControllerScript.current.vanman.PlaySound(GameControllerScript.current.vanman.whereTheFuck);

					alreadychecked = true;

					int yPos = 4;

					if (transform.position.y < -1)
						yPos = yPos - 50;

					controller.player.cc.enabled = false;
					controller.player.gameObject.transform.position = new Vector3(teleport.transform.position.x, yPos, teleport.transform.position.z);
					controller.player.cc.enabled = true;
					controller.audioDevice.PlayOneShot(controller.aud_PearlLand);

					Destroy(gameObject);
				} else {
					controller.CollectItem(22);
					controller.audioDevice.PlayOneShot(controller.tf2nope);
					Destroy(gameObject);
				}
            }
		}
		if (other.gameObject.name.ToLower().Contains("nopearea"))
		{
			Destroy(gameObject);
			controller.CollectItem(22);
			checks -= 1;
			controller.audioDevice.PlayOneShot(controller.tf2nope);
		}
		if (other.gameObject.name.ToLower().Contains("neartrigger") && controller.notebooks >= 10 && !alreadychecked)
		{
			Destroy(gameObject);
			controller.CollectItem(22);
			checks -= 1;
			controller.audioDevice.PlayOneShot(controller.tf2nope);
		}
	}
}

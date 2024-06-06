using System;
using UnityEngine;
using UnityEngine.AI;
// Token: 0x020000CA RID: 202
public class CraftersScript : MonoBehaviour
{
	public AudioClip mexican;
	public Color craftCol;
	public Color lastCol;
	public GameObject[] sprites;
	public float cooldown = 25f;

	// Token: 0x060009AE RID: 2478 RVA: 0x000249ED File Offset: 0x00022DED
	private void Start()
	{
		audioDevice = base.GetComponent<AudioSource>();

		if (Mathf.RoundToInt(SettingsManager.FamilyFriendly) == 2)
			audioDevice.clip = mexican;

		sprite.SetActive(false);
	}

	// Token: 0x060009AF RID: 2479 RVA: 0x00024A08 File Offset: 0x00022E08
	private void Update()
	{
		if (forceShowTime > 0f)
		{
			forceShowTime -= Time.deltaTime;
		}

		if (cooldown > 0f)
			cooldown -= Time.deltaTime;

		if (gettingAngry)
		{
			anger += Time.deltaTime;
			if (anger >= 1f & !angry && cooldown <= 0f)
			{
				angry = true;

				preSpeed = agent.speed;

				sprites[0].SetActive(false);
				sprites[Mathf.RoundToInt(SettingsManager.FamilyFriendly)].SetActive(true);

				gc.HideChar("crafters");
				gc.StopAllEvents();
				gc.allowEvents = false;

				if (!gc.finaleMode)
				{
					lastCol = RenderSettings.ambientLight;

					if (GameControllerScript.current.mode != "hard")
					{
						if (Mathf.RoundToInt(SettingsManager.FamilyFriendly) == 2)
							RenderSettings.ambientLight = Color.yellow;
						else
							RenderSettings.ambientLight = craftCol;
					}
					else
					{
						if (Mathf.RoundToInt(SettingsManager.FamilyFriendly) == 2)
							GameControllerScript.current.targetSchoolColor = Color.yellow;
						else
							GameControllerScript.current.targetSchoolColor = craftCol;
					}
					
				}

				if (!gc.mrBeast.beasting && !gc.finaleMode)
					audioDevice.Play();
			}
		}
		else if (anger > 0f)
		{
			anger -= Time.deltaTime;
		}
		if (!angry)
		{
			if (((base.transform.position - agent.destination).magnitude <= 20f & (base.transform.position - GameControllerScript.current.player.transform.position).magnitude >= 60f) && cooldown < 0f || forceShowTime > 0f)
			{
				sprite.SetActive(true);
			}
			else
			{
				sprite.SetActive(false);
			}
		}
		else
		{
			agent.speed = agent.speed + 5f * Time.deltaTime;
			TargetPlayer();
		}
	}

	// Token: 0x060009B0 RID: 2480 RVA: 0x00024BAC File Offset: 0x00022FAC
	private void FixedUpdate()
	{
		Vector3 direction = GameControllerScript.current.player.transform.position - base.transform.position;
		RaycastHit raycastHit;
		
		if (Physics.Raycast(base.transform.position + Vector3.up * 2f, direction, out raycastHit, float.PositiveInfinity, 769, QueryTriggerInteraction.Ignore) & raycastHit.transform.tag == "Player" & craftersRenderer.isVisible & sprite.activeSelf)
		{
			gettingAngry = true;
		}
		else
		{
			gettingAngry = false;
		}
	}

	// Token: 0x060009B1 RID: 2481 RVA: 0x00024C65 File Offset: 0x00023065
	public void GiveLocation(Vector3 location, bool flee)
	{
		if (!angry && agent.isActiveAndEnabled)
		{
			agent.SetDestination(location);
			if (flee)
			{
				forceShowTime = 3f;
			}
		}
	}

	// Token: 0x060009B2 RID: 2482 RVA: 0x00024CA0 File Offset: 0x000230A0
	private void TargetPlayer()
	{
		agent.SetDestination(GameControllerScript.current.player.transform.position);
	}

	private float preSpeed = 0f;

	// Token: 0x060009B3 RID: 2483 RVA: 0x00024CBC File Offset: 0x000230BC
	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player" & angry)
		{
			agent.speed = preSpeed;
			
			GameControllerScript.current.player.cc.enabled = false;
			angry = false;
			gettingAngry = false;
			GameControllerScript.current.player.transform.position = new Vector3(5f, GameControllerScript.current.player.transform.position.y, 80f);
			baldiAgent.Warp(new Vector3(5f, baldi.position.y, 125f));
			GameControllerScript.current.player.transform.LookAt(new Vector3(baldi.position.x, GameControllerScript.current.player.transform.position.y, baldi.position.z));
			audioDevice.Stop();
			GameControllerScript.current.player.cc.enabled = true;

			sprites[0].SetActive(true);
			sprites[Mathf.RoundToInt(SettingsManager.FamilyFriendly)].SetActive(false);

			cooldown = 45f;

			if (gc.mode != "hard")
				gc.allowEvents = true;
 
			if (GameControllerScript.current.mode != "hard") 
				RenderSettings.ambientLight = lastCol;
			else
				GameControllerScript.current.targetSchoolColor = Color.white;

			gc.ShowChar();
			GameControllerScript.current.player.OnPlayerTeleport();
		}
	}

	public bool angry;
	public bool gettingAngry;

	public float anger;
	private float forceShowTime;

	public Transform baldi;

	public NavMeshAgent baldiAgent;

	// Token: 0x040006A6 RID: 1702
	public GameObject sprite;

	// Token: 0x040006A7 RID: 1703
	public GameControllerScript gc;

	// Token: 0x040006A8 RID: 1704
	[SerializeField]
	private NavMeshAgent agent;

	// Token: 0x040006A9 RID: 1705
	public Renderer craftersRenderer;

	// Token: 0x040006AA RID: 1706
	public SpriteRenderer spriteImage;

	// Token: 0x040006AB RID: 1707
	public Sprite angrySprite;

	// Token: 0x040006AC RID: 1708
	private AudioSource audioDevice;
}

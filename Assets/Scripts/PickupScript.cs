using System;
using UnityEngine;
using UnityEngine.InputSystem;

public enum ItemType{Normal, TimedPickup, CreditCard, GasCanister}

public class PickupScript : MonoBehaviour
{
	public ItemType itemType;
	public int id;

	public int storeid;
	public bool droppedItem = false;
	public SpriteRenderer sprite;
	public BeastCardArea bca;
	public float timeleft;
	private float j;

	private void Awake()
	{
		storeid = id;
		if (PlayerPrefs.GetFloat("item_delivery") == 1 || FindObjectOfType<GameControllerScript>().neilMode)
		{
			foreach (int n in FindObjectOfType<GameControllerScript>().neilitemstogas)
			{
				if (n == id)
				{
					id = 15;
					storeid = 15;
				}
			}
		}
		if (id == 15)
			itemType = ItemType.GasCanister;
		if (FindObjectOfType<GameControllerScript>().neilMode && itemType == ItemType.CreditCard)
			Destroy(gameObject);

		int index = 0;

		foreach(Transform ntransform in base.transform)
		{
			if (index == 0)
				sprite = ntransform.GetComponent<SpriteRenderer>();

			index++;
		}

		if (itemType == ItemType.TimedPickup)
			timeleft = 30;

		UpdateSprite();
	}

	public void UpdateSprite()
	{
		System.Collections.IEnumerator WaitForUpdate()
		{
			yield return null;
			
			sprite.sprite = GameControllerScript.current.itemSprites[id];
		}

		StartCoroutine(WaitForUpdate());
	}

	private void Update()
	{
		if (droppedItem && id == 0)
			Destroy(gameObject);

		if (RaldiInputManager.current.GetInteractDown() && Time.timeScale != 0f)
		{
			Ray ray = Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f));
			RaycastHit raycastHit;

			if (Physics.Raycast(ray, out raycastHit) && Vector3.Distance(base.transform.position, GameControllerScript.current.player.transform.position) < 15 && raycastHit.transform.gameObject.tag == "Item" && raycastHit.transform == base.transform)
			{
				GameControllerScript.current.audioDevice.PlayOneShot(GameControllerScript.current.pickup);

				if (itemType != ItemType.CreditCard)
				{
					if (itemType == ItemType.TimedPickup || id == 5)
						Destroy(gameObject);
					
					if (id != 5 && id != 18 && id != 15)
					{
						sprite.sprite = GameControllerScript.current.itemSprites[GameControllerScript.current.item[GameControllerScript.current.itemSelected]];

						int carryid = GameControllerScript.current.item[GameControllerScript.current.itemSelected];

						GameControllerScript.current.ResetItem();
						GameControllerScript.current.CollectItem(id, this);
						id = carryid;
					} 
					else if (id == 5 || id == 15)
					{
						GameControllerScript.current.CollectItem(id, this);
						if (id == 15)
							sprite.sprite = GameControllerScript.current.itemSprites[0];
							id = 0;
					}
					else 
					{
						if (GameControllerScript.current.item[GameControllerScript.current.itemSelected] == 0)
						{
							GameControllerScript.current.CollectItem(id, this);
							gameObject.SetActive(false);
						}
					}
				} 
				else 
				{
					if (!GameControllerScript.current.beastCardCollected)
					{
						GameControllerScript.current.GrabCard();
						bca.returnLabel.SetActive(true);
						sprite.sprite = GameControllerScript.current.itemSprites[0];
					} 
					else
					{
						bca.returnLabel.SetActive(false);
						sprite.sprite = GameControllerScript.current.itemSprites[13];
						GameControllerScript.current.beastTip.SetActive(false);
						bca.ui_Card.SetActive(false);
					}
					GameControllerScript.current.beastCardCollected = (!GameControllerScript.current.beastCardCollected);
				}
			}
		}
			
		if (itemType == ItemType.GasCanister || PlayerPrefs.GetFloat("item_delivery") == 1)
		{
			if (id == 0 && GameControllerScript.current.notebooks >= 2 && itemType != ItemType.CreditCard) 
			{
				timeleft += Time.deltaTime;

				if (timeleft > 30) 
				{
					id = storeid;
					sprite.sprite = GameControllerScript.current.itemSprites[storeid];
				}
			} 
			else if (id != 5)
				timeleft = 0;
		}

		if (itemType == ItemType.TimedPickup) 
		{
			j = Mathf.Round(timeleft * 20) / 10;
			timeleft -= Time.deltaTime;

			if (timeleft <= 10) 
			{
				if (Mathf.Round(j) > j)
					sprite.color = new Color(255,255,255,255);
				else
					sprite.color = new Color(255,0,0,255);
			}

			if (timeleft <= 0)
				Destroy(gameObject);
		}
		if (itemType == ItemType.CreditCard)
		{
			if (!GameControllerScript.current.beastCardCollected)
			{
				bca.returnLabel.SetActive(false);
				sprite.sprite = GameControllerScript.current.itemSprites[13];
				GameControllerScript.current.beastTip.SetActive(false);
				bca.ui_Card.SetActive(false);
			} 
			else 
			{
				bca.returnLabel.SetActive(true);
				sprite.sprite = GameControllerScript.current.itemSprites[0];
			}

			if (GameControllerScript.current.killedmrBeast)
				Destroy(gameObject);
		}
	}
}
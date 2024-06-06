using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020000BF RID: 191
public class ExitTriggerScript : MonoBehaviour
{
	// Token: 0x06000962 RID: 2402 RVA: 0x000219A0 File Offset: 0x0001FDA0
	private void OnTriggerEnter(Collider other)
	{
		int hasCard = 0;

		if (gc.beastCardCollected)
			hasCard = 1;

		if (this.gc.notebooks >= 10 & other.tag == "Player" && !gc.player.gameOver)
		{
			FindObjectOfType<PlayerStats>().Save();

			PlayerPrefs.SetInt("HadBeastCard", hasCard);
			PlayerPrefs.SetInt("CurSessionRank", gc.rank);
			PlayerPrefs.SetInt("CurSessionScore", gc.score);
			PlayerPrefs.SetFloat("CurSessionPlaytime", FindObjectOfType<GameControllerScript>().playTime);
			
			if(FindObjectOfType<GameControllerScript>().mode == "hard")
				SceneManager.LoadScene("Pizza Tower Rank");
			else
				SceneManager.LoadScene("Results");
		}
	}

	// Token: 0x040005F6 RID: 1526
	public GameControllerScript gc;
}

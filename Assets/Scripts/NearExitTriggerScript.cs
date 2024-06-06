using System;
using UnityEngine;

public class NearExitTriggerScript : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (gc.exitsReached < 3 & gc.finaleMode & other.tag == "Player")
		{
			gc.audioDevice.PlayOneShot(gc.aud_Exit);
			gc.GiveScore(300);
			gc.exitsReached++;
			es.Lower();

			gc.UpdateNotebookCount();
			
			GameControllerScript.current.baldi.speed += 3f;

            if (gc.baldi.isActiveAndEnabled) 
				gc.baldi.Hear(base.transform.position, 8f);
				
			if (gc.neilMode && gc.exitsReached == 3)
			{
				gc.neilambience.Stop();
				gc.baldi.gameObject.SetActive(false);
			}
			if (!(gc.neilMode || gc.mode == "hard"))
			{
				gc.player.playerVCam.m_Lens.FieldOfView += 40;
				gc.targetSchoolColor = new Color(gc.targetSchoolColor.r, ((gc.targetSchoolColor.g * 255) - 40) / 255, ((gc.targetSchoolColor.b * 255) - 40) / 255);

				if (gc.exitsReached == 2)
				{
					RenderSettings.fog = true;
					RenderSettings.fogColor = gc.endingFogColor;
					RenderSettings.ambientLight = Color.white;

					gc.musicBeat.beatHit.RemoveAllListeners();
					gc.endSong.Stop();
					gc.endSong.volume = 0;
					gc.schoolMusic.clip = gc.machineRev;
					gc.schoolMusic.loop = true;
					gc.schoolMusic.Play();
					gc.baldi.GetComponent<AudioSource>().pitch = 0.5f;
				}
				if (gc.exitsReached == 3)
				{
					gc.schoolMusic.clip = gc.machineLoop;
					gc.schoolMusic.loop = true;
					gc.schoolMusic.Play();
				}
			}
		}
	}

	// Token: 0x04000674 RID: 1652
	public GameControllerScript gc;

	// Token: 0x04000675 RID: 1653
	public EntranceScript es;
}

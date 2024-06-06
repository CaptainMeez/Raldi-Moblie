using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeilNearTrigger : MonoBehaviour
{
    public GameControllerScript gc;
    public EntranceScript corrispondingExit;
    public Transform neilSpawnPoint;
    public Transform neilNPC;
    public Transform holdPos;
    public Transform firstProjectSpawn;
    public AudioSource crackhouseTroubleSource;
    public AudioClip crackhouseRamble;
    public AudioClip crackhouseRambleCont;
    public AudioClip sansclick;
    bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!triggered && other.transform.gameObject.tag == "Player" && gc.finaleMode && gc.exitsReached == 3 && gc.neilMode)
        {
            IEnumerator Click()
            {
                if (GameControllerScript.current.player.isMonke)
                {
                    GameControllerScript.current.player.DisableMonke(false);
                    GameControllerScript.current.schoolMusic.Stop();
                }

                gc.audioDevice.PlayOneShot(sansclick);
                gc.sansblackscreen.SetActive(true);  
                gc.hud.SetActive(false);
                gc.baldi.gameObject.SetActive(false);
                corrispondingExit.Lower();
                corrispondingExit.wall.material = corrispondingExit.map;
                neilNPC.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
                neilNPC.gameObject.SetActive(true);
                neilNPC.position = neilSpawnPoint.position;
                gc.ClearItems();  
                gc.GasAmmo = 0;
                
                foreach(PickupScript pickup in GameObject.FindObjectsOfType<PickupScript>())
                {
                    pickup.gameObject.SetActive(false);
                }

                yield return new WaitForSeconds(0.3f);
                gc.audioDevice.PlayOneShot(sansclick);
                gc.sansblackscreen.SetActive(false);

                neilNPC.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = true;
                neilNPC.GetComponent<NeilScript>().PreBossAudio();

            crackhouseTroubleSource.clip = crackhouseRamble;
            crackhouseTroubleSource.Play();
            }

            StartCoroutine(Click());

            IEnumerator WaitTime()
            {
                yield return new WaitForSeconds(crackhouseRamble.length);
                if (!FindObjectOfType<NeilScript>().startedBoss)
                {
                    crackhouseTroubleSource.clip = crackhouseRambleCont;
                    crackhouseTroubleSource.Play();
                }
            }

            StartCoroutine(WaitTime());
            
            NeilProjectileScript project = GameObject.Instantiate<GameObject>(gc.neilProjectile, firstProjectSpawn.position, firstProjectSpawn.rotation).GetComponent<NeilProjectileScript>();
            project.player = gc.player.transform;
            project.holdPos = holdPos; 
            project.isFirst = true;
            project.transform.position = new Vector3(project.transform.position.x, 4, project.transform.position.z);
            project.isBanana = true;

            triggered = true;

            foreach(NeilNearTrigger triggers in FindObjectsOfType<NeilNearTrigger>())
            {
                triggers.triggered = true;
            }
        }
    }
}

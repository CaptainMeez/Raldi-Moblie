using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeilLeavingTrigger : MonoBehaviour
{
    public AudioSource music;
    public AudioClip[] vocals;

    public AudioSource glitchyness;

    public AudioSource neils;
    public Transform neil;

    public AudioClip staticc;

    public Transform playerSpawn;

    int curVocal;

    bool canDo = true;

    public AudioClip RUN;

    public GameObject staticFade;

    public AudioClip rise;

    private void Update()
    {
        if (!canDo)
            FindObjectOfType<PlayerMovementScript>().playerVCam.m_Lens.FieldOfView -= Time.deltaTime / 2;
    }

    public CharacterController cc;
    public PlayerMovementScript player;

    public AudioClip laugh;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            IEnumerator GetPlayer()
            {
                player.enabled = false;
                cc.enabled = false;
                player.disableMovement = true;

                yield return null;

                other.transform.position = playerSpawn.transform.position;
                other.transform.LookAt(new Vector3(neil.position.x, neil.position.y, neil.position.z));

                yield return null;

                player.enabled = true;
                cc.enabled = true;
                player.disableMovement = false;

                music.Stop();
                neils.Stop();

                music.PlayOneShot(staticc);
                
                music.PlayOneShot(vocals[curVocal]);

                curVocal++;

                RenderSettings.ambientLight = new Color(RenderSettings.ambientLight.r - 0.1f, RenderSettings.ambientLight.g - 0.1f, RenderSettings.ambientLight.b - 0.1f, 0);

                glitchyness.volume += 0.1f;

                yield return new WaitForSeconds(0.01f);

                if (curVocal == vocals.Length)
                {
                    canDo = false;
                    
                    print("THE FINAL");
                    glitchyness.volume = 1;
                    FindObjectOfType<PlayerStats>().data.interactedWithNeil = true;
				    FindObjectOfType<PlayerStats>().Save();

                    RenderSettings.ambientLight = Color.black;

                    music.clip = RUN;
                    music.loop = true;
                    music.Play();

                    FindObjectOfType<PlayerMovementScript>().walkSpeed = 0;
                    FindObjectOfType<PlayerMovementScript>().runSpeed = 0;

                    yield return new WaitForSeconds(vocals[vocals.Length - 1].length);

                    staticFade.SetActive(true);

                    music.PlayOneShot(rise);
                    music.PlayOneShot(laugh);

                    cc.enabled = false;
                    player.disableMovement = true;

                    player.gameObject.AddComponent<RealBillboard>();

                    yield return new WaitForSeconds(rise.length);

                    PlayerPrefs.SetFloat("PreventNewFiles", 1);

                    Application.Quit();
                }      
            }

            StartCoroutine(GetPlayer());
        }
    }
}

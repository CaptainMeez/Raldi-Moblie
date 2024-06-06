using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeilHammerable : MonoBehaviour
{
    bool canHammer = true;

    public GameObject highlight;

    void Update()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit rhit;

        if (GameControllerScript.current.item[GameControllerScript.current.itemSelected] == 16 && canHammer && Physics.Raycast(ray, out rhit) && rhit.collider.gameObject == gameObject && Vector3.Distance(base.transform.position, GameControllerScript.current.player.transform.position) < 15)
        {
            highlight.SetActive(true);

            if (Input.GetMouseButtonDown(0))
                Hammer();
        }
        else
            highlight.SetActive(false);
    }

    public Material wallBroke;
    public Animator playerCamera;

    public AudioClip boomBoom;
    public GameObject windy;

    public void Hammer()
    {
        canHammer = false;

        base.GetComponent<MeshRenderer>().material = wallBroke;
        playerCamera.SetTrigger("Shakey");

        GameControllerScript.current.audioDevice.PlayOneShot(boomBoom);
        windy.SetActive(true);

        IEnumerator Wait()
        {
            GameControllerScript.current.audioDevice.PlayOneShot(FindObjectOfType<NeilScript>().diee);

            FindObjectOfType<NeilScript>().theEndAnimation.SetActive(true);
            yield return new WaitForSeconds(2);
            FindObjectOfType<PlayerScript>().enabled = false;
            yield return new WaitForSeconds(3);

            FindObjectOfType<NeilScript>().destroyedEnviorment.SetActive(false); // OPTIMIZATION SHITS
            FindObjectOfType<NeilScript>().finaleCutscene.SetActive(true);

            GameControllerScript.current.schoolMusic.clip = FindObjectOfType<NeilScript>().theEnd;
            GameControllerScript.current.schoolMusic.Play();

            yield return new WaitForSeconds(5);
            FindObjectOfType<NeilScript>().theEndAnimation.SetActive(false);
        }

        StartCoroutine(Wait());
    }
}

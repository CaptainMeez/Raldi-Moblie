using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ManholeScript : MonoBehaviour
{
    public int teleportPositon;
    public GameObject fadeEffect;
    public GameObject label;
    public PlayerScript playerScript;
    public Transform player;
    public float interactDistance;

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f));
		RaycastHit raycastHit;

        if (!playerScript.inNotebook && !playerScript.diarrheaing && Physics.Raycast(ray, out raycastHit) && Vector3.Distance(player.position, base.transform.position) < interactDistance)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Leave();
            }

            label.SetActive(true);
        }
        else
        {
            label.SetActive(false);
        }
    }

    public void Leave()
    {
        fadeEffect.SetActive(true);

        if (!playerScript.diarrheaing)
            playerScript.cc.enabled = false;
        else
            playerScript.GetComponent<NavMeshAgent>().enabled = false;

        IEnumerator TeleportPlayerToPosition()
        {
            yield return new WaitForSeconds(1f);
            player.position = new Vector3(transform.position.x, teleportPositon + 4, transform.position.z);
            yield return new WaitForSeconds(1f);
            fadeEffect.SetActive(false);

            if (!playerScript.diarrheaing)
                playerScript.cc.enabled = true;
            else
                playerScript.GetComponent<NavMeshAgent>().enabled = true;
        }

        StartCoroutine(TeleportPlayerToPosition());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && playerScript.diarrheaing)
        {
            Leave();
        }
    }
}

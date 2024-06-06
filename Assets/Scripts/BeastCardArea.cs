using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeastCardArea : MonoBehaviour
{
    public GameObject ui_Card;
    public GameObject returnLabel;
    public Transform player;
    public float interactDistance = 15f;

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f));
		RaycastHit raycastHit;

        if (Physics.Raycast(ray, out raycastHit) && Vector3.Distance(player.position, base.transform.position) < interactDistance)
        {
            if (GameControllerScript.current.beastCardCollected)
                returnLabel.SetActive(true);
            else
                returnLabel.SetActive(false);

            if (Input.GetKeyDown(KeyCode.Mouse0) && GameControllerScript.current.beastCardCollected)
            {
                GameControllerScript.current.beastTip.SetActive(false);
                ui_Card.SetActive(false);
            }
        }
    }
}

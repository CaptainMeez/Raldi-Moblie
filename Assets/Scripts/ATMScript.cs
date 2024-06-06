using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ATMScript : MonoBehaviour
{
    public bool canInteract = true;

    public GameObject infoTick;
    public GameObject useLabel;
    public GameObject outOfOrder;

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f));
        RaycastHit raycastHit;

        if (Physics.Raycast(ray, out raycastHit) && Vector3.Distance(GameControllerScript.current.player.transform.position, base.transform.position) < GameControllerScript.current.player.interactDistance && canInteract)
        {
            useLabel.SetActive(true);

            if ((Input.GetKeyDown(KeyCode.Mouse1) || Input.GetKeyDown(KeyCode.E)) && GameControllerScript.current.beastCardCollected)
            {
                if(!GameControllerScript.current.mrBeast.beasting)
                    GameControllerScript.current.GiveScore(3000);
                GameControllerScript.current.AddMoney(1);
                infoTick.SetActive(false);

                ToggleOrder(true);
            }
        }
        else
        {
            useLabel.SetActive(false);
        }
    }

    public void ToggleOrder(bool order)
    {
        outOfOrder.SetActive(order);
        canInteract = !order;
    }
}

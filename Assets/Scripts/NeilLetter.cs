using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeilLetter : MonoBehaviour
{
    public GameObject letter;
    bool open = false;
    bool down = false;
    public AudioClip pageTurn;
    public GameObject reticles;

    private void Update()
	{
		if (Input.GetMouseButtonDown(0) && Time.timeScale != 0f)
		{
			Ray ray = Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f));
			RaycastHit raycastHit;

			if (Physics.Raycast(ray, out raycastHit) && Vector3.Distance(base.transform.position, GameControllerScript.current.player.transform.position) < 15 && raycastHit.transform == base.transform)
			{
                if (!open && !down)
                {
                    open = true;
                    letter.SetActive(true);
                    reticles.SetActive(false);
                    GameControllerScript.current.player.inNotebook = true;
                    GameControllerScript.current.canPause = false;
                    GameControllerScript.current.audioDevice.PlayOneShot(pageTurn);

                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
			}
		}

        if (!down && open && Input.GetKeyDown(KeyCode.Escape))
        {
            Exit();
        }
        if (GameControllerScript.current.notebooks > 0 || GameControllerScript.current.ishaanMode)
            gameObject.SetActive(false);
    }

    public void Exit()
    {
        down = true;
        reticles.SetActive(true);
        base.transform.position = new Vector3(base.transform.position.x, -20f, base.transform.position.z);
        GameControllerScript.current.player.inNotebook = false;
        StartCoroutine(GameControllerScript.current.ReEnablePauseDumbness());
        letter.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}

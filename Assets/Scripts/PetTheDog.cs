    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetTheDog : MonoBehaviour
{
    public PlayerScript player;
    public Animator playerAnimation;
    public Transform dogPetCamera;
    public GameObject label;
    public Camera playerSecondaryCamera;
    public AudioClip bark;
    public LayerMask layer;
    private LayerMask originalMask;
    public CoulsonEngine.Game.Dialogue.FileDialogueTrigger dialogue;

    bool pettedTheDog = false;
    void Update()
    {
        if (Vector3.Distance(player.transform.position, base.transform.position) < 10 && !pettedTheDog)
        {
            label.SetActive(true);
        }
        else
        {
            label.SetActive(false);
        }

        Ray ray = Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f));
		RaycastHit raycastHit;

        if (Physics.Raycast(ray, out raycastHit) && Vector3.Distance(player.transform.position, base.transform.position) < 20 && Input.GetKeyDown(KeyCode.E) && !pettedTheDog)
        {
            originalMask = GameControllerScript.current.cameraTransform.GetComponent<Camera>().cullingMask;
            GameControllerScript.current.cameraTransform.GetComponent<Camera>().cullingMask = layer;

            GameControllerScript.current.StopAllEvents();
            GameControllerScript.current.HideChar();
            GameControllerScript.current.allowEvents = false;

            pettedTheDog = true;
            player.playerVCam.gameObject.SetActive(false);
            dogPetCamera.gameObject.SetActive(true);
            playerSecondaryCamera.gameObject.SetActive(false);
            player.inNotebook = true;
            playerAnimation.Play("PlayerPet");

            IEnumerator Wait()
            {
                yield return new WaitForSeconds(3.3f);

                ApplyDiscount();
            }

            StartCoroutine(Wait());
        }
    }

    public void ApplyDiscount()
    {
        string[] machinePool = {"soda", "zesty", "pearl", "energy"};

        dialogue.dialogueFile = "dog_discount";
        dialogue.ReloadDialogueVarriable();

        if (UnityEngine.Random.Range(0, 3) != 1) // Sales
        {
            int machine = UnityEngine.Random.Range(0, 4);

            switch(machinePool[machine])
            {
                case "soda":
                    dialogue.dialogue.sentences[0].sentence = dialogue.dialogue.sentences[0].sentence.Replace("{machine}", "BSODA");
                    GameControllerScript.current.sodaDiscount = 2;
                    break;
                case "zesty":
                    dialogue.dialogue.sentences[0].sentence = dialogue.dialogue.sentences[0].sentence.Replace("{machine}", "Zesty Bar");
                    GameControllerScript.current.zestyDiscount = 2;
                    break;
                case "pearl":
                    dialogue.dialogue.sentences[0].sentence = dialogue.dialogue.sentences[0].sentence.Replace("{machine}", "Ender Pearl");
                    GameControllerScript.current.pearlDiscount = 2;
                    break;
                case "energy":
                    dialogue.dialogue.sentences[0].sentence = dialogue.dialogue.sentences[0].sentence.Replace("{machine}", "15 Second Energy");
                    GameControllerScript.current.energyDiscount = 2;
                    break;
            }

            dialogue.dialogue.sentences[0].sentence = dialogue.dialogue.sentences[0].sentence.Replace("{discount}", "50%");
            dialogue.onComplete.AddListener(OnSaleFinished);
            dialogue.TriggerDialogue();
        }
        else // No sale
        {
            string file = "dog_unsatisfied";

            if (UnityEngine.Random.Range(0, 1000) == 500)
                file = "dog_unsatisfied_secret";

            dialogue.dialogueFile = file;
            dialogue.onComplete.AddListener(OnSaleFinished);
            dialogue.ReloadDialogueVarriable();
            dialogue.TriggerDialogue();
        }
    }

    public void OnSaleFinished()
    {
        GameJolt.API.Trophies.TryUnlock(185668);
        player.inNotebook = false;
        player.playerVCam.gameObject.SetActive(true);
        dogPetCamera.gameObject.SetActive(false);
        playerSecondaryCamera.gameObject.SetActive(true );

        GameControllerScript.current.audioDevice.PlayOneShot(bark);
        GameControllerScript.current.allowEvents = true;
        GameControllerScript.current.ShowChar();

        GameControllerScript.current.cameraTransform.GetComponent<Camera>().cullingMask = originalMask;
    }
}

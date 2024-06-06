using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CoulsonEngine.Game.Dialogue;

public class TapeCase : MonoBehaviour
{
    [HideInInspector] public string[] tapes = {"Chipfloke", "MrBeast", "Polish Cow", "Neil"};
    public int curTape = 0;
    public bool alrHasTape = false;
    public DialogueManager manager;
    public DialogueSentence sentence;
    public bool allow = true;

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f));
		RaycastHit raycastHit;

        if (Physics.Raycast(ray, out raycastHit) && Vector3.Distance(GameControllerScript.current.player.transform.position, base.transform.position) < 15 && (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0)) && !alrHasTape && allow)
        {
            sentence.sentence = sentence.sentence.Replace("{char}", tapes[curTape]);
            DialogueSentence[] dial = {sentence};
            alrHasTape = true;

            Dialogue dialogue = new Dialogue();
            dialogue.sentences = dial;
            manager.StartDialogue(dialogue);

            sentence.sentence = sentence.sentence.Replace(tapes[curTape], "{char}");
        }
    }
}

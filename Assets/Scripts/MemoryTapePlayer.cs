using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

using CoulsonEngine.Game.Dialogue;

public class MemoryTapePlayer : MonoBehaviour
{
    public TapeCase tapes;
    public DialogueSentence sentence;
    public GameObject fade;
    public GameObject[] videos;
    public GameObject[] actualVideos;
    public GameObject wall;
    public GameObject door;
    public AudioClip scarey;

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f));
		RaycastHit raycastHit;

        if (Physics.Raycast(ray, out raycastHit) && Vector3.Distance(GameControllerScript.current.player.transform.position, base.transform.position) < 15 && (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0)) && tapes.alrHasTape && tapes.allow)
        {
            sentence.sentence = sentence.sentence.Replace("{char}", tapes.tapes[tapes.curTape]);
            DialogueSentence[] dial = {sentence};

            Dialogue dialogue = new Dialogue();
            tapes.manager.whenDone.AddListener(EnterTape);
            dialogue.sentences = dial;
            tapes.manager.StartDialogue(dialogue);

            sentence.sentence = sentence.sentence.Replace(tapes.tapes[tapes.curTape], "{char}");

            tapes.alrHasTape = false;
        }
    }

    public void EnterTape()
    {
        GameControllerScript.current.player.inNotebook = true;
        fade.SetActive(true);
        
        IEnumerator Play()
        {
            yield return new WaitForSeconds(4.5f);
            fade.SetActive(false);
                videos[tapes.curTape].SetActive(true);
                actualVideos[tapes.curTape].GetComponent<VideoPlayer>().loopPointReached += OnVideoEnd;
        }

        StartCoroutine(Play());
    }

    public void OnVideoEnd(UnityEngine.Video.VideoPlayer vp)
    {
        GameControllerScript.current.player.inNotebook = false;
        videos[tapes.curTape].SetActive(false);

        if (tapes.curTape == 3)
        {
            print("SCARE");
            tapes.allow = false;
            wall.SetActive(false);
            door.SetActive(true);
            GameControllerScript.current.schoolMusic.clip = scarey;
            GameControllerScript.current.schoolMusic.Play();

            FindObjectOfType<PlayerStats>().data.ishaanTimeWahoo = true;
            FindObjectOfType<PlayerStats>().Save();
        }

        tapes.curTape++;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FakeNeilHouse : MonoBehaviour
{
    public Transform spawnPos;
    public GameObject[] objectss;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return null;
        
        FindObjectOfType<PlayerStats>().TryLoad();
        
        if (FindObjectOfType<PlayerStats>().data.ishaanTimeWahoo)
            Teleport();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Material staticc;

    public void Transition()
    {
        foreach(MeshRenderer renderer in FindObjectsOfType<MeshRenderer>())
        {
            renderer.material = staticc;
        }
        
        IEnumerator trans()
        {
            yield return new WaitForSeconds(0.3f);
            
            FindObjectOfType<PlayerStats>().data.ishaanTimeWahoo = false;
            FindObjectOfType<PlayerStats>().data.ishaanModeUnlocked = true;
            FindObjectOfType<PlayerStats>().data.ishaanMenu = true;
            FindObjectOfType<PlayerStats>().Save();

            SceneManager.LoadSceneAsync("MainMenu");  
        }

        StartCoroutine(trans());
    }

    public void Teleport()
    {
        foreach(GameObject obj in objectss) {obj.SetActive(true);}
        RenderSettings.skybox = GameControllerScript.current.galaxySky;
        GameControllerScript.current.player.cc.enabled = false;
        GameControllerScript.current.player.transform.position = spawnPos.position;
        GameControllerScript.current.player.cc.enabled = true; 
        GameControllerScript.current.schoolMusic.Stop();
        GameControllerScript.current.hud.SetActive(false);
    }
}

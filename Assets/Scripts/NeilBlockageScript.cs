using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeilBlockageScript : MonoBehaviour
{
    public AudioSource audio;
    public AudioClip[] stone = new AudioClip[4];
    public AudioClip[] wood = new AudioClip[4];
    public AudioClip[] dirt = new AudioClip[4];
    public AudioClip brickbattlebass;
    public int wallid;
    public bool skipthiswall;
    public bool wallEnabled;
    private Transform minecraftanim;
    private Transform robloxanim;
    private NeilBlockageManagerScript manager;
    public Material[] materials = new Material[3];
    public Material stopsign;
    public Material scouttf2;
    public Material trollface;
    public AudioClip stopsignaudio;
    public AudioClip scoutlaugh;
    public AudioClip trollfaceaudio;
    public Shader defaultS;
    // Start is called before the first frame update
    void Start()
    {
        audio = gameObject.GetComponent<AudioSource>();
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        manager = gameObject.transform.parent.gameObject.GetComponent<NeilBlockageManagerScript>();
        minecraftanim = gameObject.transform.GetChild(3);
        robloxanim = gameObject.transform.GetChild(2);
    }

    // Update is called once per frame
    void Update()
    {   
        if (gameObject.GetComponent<MeshCollider>() != null)
            gameObject.GetComponent<MeshCollider>().enabled = wallEnabled;
    }


    public void Interaction()
    {
        manager.InitializeWalls();
        skipthiswall = true;
        manager.GenerateWalls();
    }
    public void Reset()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        skipthiswall = false;
        wallEnabled = false;
        ResetMinecraftAnim();
    }

    public void BuildWall()
    {
        int spawnwall = Random.Range(0,GameControllerScript.current.neilwallspawnchancemax);
        print("spawnchance: " + spawnwall);
        if (spawnwall == 0)
        {
            int animationtoplay = Random.Range(1,6);
            if (animationtoplay == 1)
            {
                ResetMinecraftAnim();
                StartCoroutine(MinecraftAnim(Random.Range(0,2)));
            } else if (animationtoplay == 2)
            {
                int r = Random.Range(0,255);
                int g = Random.Range(0,255);
                int b = Random.Range(0,255);
                Material blockMaterial = new Material(defaultS);
                blockMaterial.color = new Color32(((byte)r),((byte)g),((byte)b), 255);
                print(blockMaterial.color);
                for (int i = 0; i < 12; i++)
                {
                    print("RGB: " + r + " " + g + " " + b);
                    robloxanim.GetChild(i).GetComponent<Renderer>().material = blockMaterial;
                } 
                audio.PlayOneShot(brickbattlebass);
                base.GetComponent<Animator>().SetTrigger("robux");
            } else if (animationtoplay == 3) {
                audio.PlayOneShot(stopsignaudio);
                gameObject.GetComponent<MeshRenderer>().material = stopsign;
                gameObject.GetComponent<MeshRenderer>().enabled = true;
            } else if (animationtoplay == 4) {
                audio.PlayOneShot(scoutlaugh);
                gameObject.GetComponent<MeshRenderer>().material = scouttf2;
                gameObject.GetComponent<MeshRenderer>().enabled = true;
            } else if (animationtoplay == 5) {
                audio.PlayOneShot(trollfaceaudio);
                gameObject.GetComponent<MeshRenderer>().material = trollface;
                gameObject.GetComponent<MeshRenderer>().enabled = true;
            }
            wallEnabled = true;
        }
    }

    public IEnumerator MinecraftAnim(int blocktype)
    {
        minecraftanim.gameObject.SetActive(true);
        for (int i = 0; i < 16; i++)
        {
            if (blocktype == 1)
            {
                audio.PlayOneShot(wood[Random.Range(0,3)]);
                minecraftanim.transform.GetChild(i).gameObject.GetComponent<MeshRenderer>().material = materials[0];
            } else if (blocktype == 2)
            {
                audio.PlayOneShot(stone[Random.Range(0,3)]);
                minecraftanim.transform.GetChild(i).gameObject.GetComponent<MeshRenderer>().material = materials[1];
            } else {
                audio.PlayOneShot(dirt[Random.Range(0,3)]);
                minecraftanim.transform.GetChild(i).gameObject.GetComponent<MeshRenderer>().material = materials[2];
            }
            minecraftanim.transform.GetChild(i).gameObject.SetActive(true);
            yield return new WaitForSeconds(0.1f);
        }
    }
    public void ResetMinecraftAnim()
    {
        StopAllCoroutines();
        for (int i = 0; i < 16; i++)
        {
            minecraftanim.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}

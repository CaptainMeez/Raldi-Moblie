using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeilProjectileScript : MonoBehaviour
{
    private AudioSource source;
    private PlayerScript players;
    public Rigidbody rb;
    public Transform player;
    public Transform holdPos;
    public bool collectedByPlayer = false;
    public bool shootForward = false;
    private float lifeSpan = 30f;
    private bool hold;
    public bool isFirst = false;
    private float metaltimer = 10f;

    public bool isBanana;
    public bool isMetalBlade;

    public SpriteRenderer rendererr;

    int meme;

    public Sprite banana;
    public Sprite[] memeArray;
    public AudioClip[] throwArray;
    public Sprite metalblade1;
    public Sprite metalblade2;
    public AudioClip metalbladesound;

    public Color pickedUpColor;
    
    private void Awake()
    {
        players = FindObjectOfType<PlayerScript>();
        source = GetComponent<AudioSource>();

        if (!isBanana & !isMetalBlade)
            meme = UnityEngine.Random.Range(0, memeArray.Length);
        if (!isMetalBlade)
            rendererr.sprite = memeArray[meme];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isMetalBlade)
        {
            if (FindObjectOfType<UnlockedPlayerScript>() == null)
            {
                holdPos = players.holdPos;
            }
        }

        if (other.transform.tag == "Player" && !collectedByPlayer && !FindObjectOfType<GameControllerScript>().isHoldingNeilObject)
        {
            collectedByPlayer = true;
            FindObjectOfType<GameControllerScript>().isHoldingNeilObject = true;
            hold = true;
        }
        else if (other.transform.name.Contains("Wall") && shootForward && !banana)
        {
            GameObject.Instantiate<GameObject>(explosion, transform.position, transform.rotation);
            GameObject.Destroy(this.gameObject);
        }
    }

    public GameObject explosion;

    public void Update()
    {
        if (isMetalBlade)
        {
            metaltimer += Time.deltaTime;
            if (metaltimer > 0.2f)
                metaltimer = 0f;
            if (metaltimer > 0.1f)
                rendererr.sprite = metalblade1;
            else
                rendererr.sprite = metalblade2;
        }  
        if (isBanana && rendererr.sprite != banana)
            rendererr.sprite = banana;

        if (hold)
        {
            if (!isBanana)
                GameControllerScript.current.throwProjectileText.SetActive(true);
            else
            {
                Ray ray = Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f));
			    RaycastHit raycastHit;

                if (Physics.Raycast(ray, out raycastHit) && raycastHit.transform.gameObject.name.ToLower() == "neil")
                {
                    GameControllerScript.current.throwProjectileText.SetActive(true);
                }
                else
                {
                    GameControllerScript.current.throwProjectileText.SetActive(false);
                }
            }

            if (FindObjectOfType<UnlockedPlayerScript>() != null)
                holdPos = FindObjectOfType<UnlockedPlayerScript>().holdPos;
            base.transform.position = holdPos.position;
            base.transform.rotation = GameControllerScript.current.player.playerVCam.transform.rotation;
            if (FindObjectOfType<NeilScript>() != null)
            {
                if (FindObjectOfType<NeilScript>().phase2)
                    base.transform.rotation = FindObjectOfType<UnlockedPlayerScript>().PlayerCamera.transform.rotation;
            }
            rendererr.color = pickedUpColor;
        }
        else
        {
            if (!isBanana)
                GameControllerScript.current.throwProjectileText.SetActive(false);
        }
            
        if (collectedByPlayer && (Input.GetMouseButtonDown(0) || isMetalBlade) && !shootForward)
        {
            if (isBanana)
            {
                Ray ray = Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f));
			    RaycastHit raycastHit;

                if (Physics.Raycast(ray, out raycastHit) && raycastHit.transform.gameObject.name.ToLower() == "neil")
                {
                    Throw();
                }
            }
            else
                Throw();
            
        }

        if (shootForward)
        {
            this.rb.velocity = base.transform.forward * 100;
            this.lifeSpan -= Time.deltaTime;
            if (this.lifeSpan < 0f)
            {
                UnityEngine.Object.Destroy(base.gameObject, 0f);
            }
        }
    }

    public void Throw()
    {
        GameControllerScript.current.throwProjectileText.SetActive(false);
            shootForward = true;
            FindObjectOfType<GameControllerScript>().isHoldingNeilObject = false;
            hold = false;
            
            rendererr.color = Color.white;

            if (!isBanana && !isMetalBlade)
                source.PlayOneShot(throwArray[meme]);
            if (isMetalBlade)
                source.PlayOneShot(metalbladesound);
            if (!isFirst && !isMetalBlade)
            {
                IEnumerator SpawnNew()
                {
                    Transform targetPosition;

                    if (!FindObjectOfType<NeilScript>().phase2)
                        targetPosition = FindObjectOfType<NeilScript>().projPoints[UnityEngine.Random.Range(0, FindObjectOfType<NeilScript>().projPoints.Length)];
                    else
                        targetPosition = FindObjectOfType<NeilScript>().phase2Spawns[UnityEngine.Random.Range(0, FindObjectOfType<NeilScript>().phase2Spawns.Length)].transform;

                    // Prevent double pickups
                    if (targetPosition.position == FindObjectOfType<NeilScript>().lastPosition)
                    {
                        while(targetPosition.position == FindObjectOfType<NeilScript>().lastPosition)
                        {
                            if (!FindObjectOfType<NeilScript>().phase2)
                                targetPosition = FindObjectOfType<NeilScript>().projPoints[UnityEngine.Random.Range(0, FindObjectOfType<NeilScript>().projPoints.Length)];
                            else
                                targetPosition = FindObjectOfType<NeilScript>().phase2Spawns[UnityEngine.Random.Range(0, FindObjectOfType<NeilScript>().phase2Spawns.Length)].transform;
                        }
                    }

                    yield return null;

                    Transform thing = GameObject.Instantiate<GameObject>(GameControllerScript.current.neilProjectile, targetPosition.position, targetPosition.rotation).transform;
                    thing.GetComponent<NeilProjectileScript>().player = GameControllerScript.current.player.transform;

                    if (!FindObjectOfType<NeilScript>().phase2)
                        thing.GetComponent<NeilProjectileScript>().holdPos = FindObjectOfType<NeilNearTrigger>().holdPos; 
                    else
                        thing.GetComponent<NeilProjectileScript>().holdPos = FindObjectOfType<UnlockedPlayerScript>().holdPos;

                    FindObjectOfType<NeilScript>().lastPosition = thing.position;
                }

                StartCoroutine(SpawnNew());
            }
    }
}

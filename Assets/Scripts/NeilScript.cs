using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.IO;
using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using UnityEngine.SceneManagement;

public enum Phases
{
    Chasing, Attacking
}

public class NeilScript : MonoBehaviour
{
    private Phases phase2Phases;

    public bool immunetophase1damage;
    public float storetempo;
    public FluidMidi.SongPlayer midi;
    public FluidMidi.SongPlayer finaldrums;
    public FluidMidi.StreamingAsset crackLoop;
    public FluidMidi.StreamingAsset drumLoop;
    private NavMeshAgent agent;
    public AudioSource source;
    public AudioSource tf2sounds;
    public AudioSource crackhouseTrouble;
    public GameObject neilhealthobject;
    public float targetSpeed;
    public float shieldTime = 20f;
    private float regenTimer = 10f;
    public AudioClip hitSong;
    public AudioClip neilPreBoss;
    public AudioClip ishaanPreBoss;
    public AudioClip neilPostBoss;
    public AudioClip ishaanPostBoss;
    private bool targetPlayer = false;
    private bool phase2Started = false;
    private bool canRegen = false;
    public bool startedBoss = false;
    public bool shieldsUp = true;
    bool doSpeed = false;
    public bool ishaanMode;

    public Animator phase2Animator;

    public Vector3 lastPosition;

    public GameObject[] changestuff = new GameObject[3];
    public Sprite ishaanSprite;
    public Sprite ishaanShineSprite;

    public void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        playerTransform = GameController.player.transform;
        ishaanMode = GameController.ishaanMode;
        if (ishaanMode)
        {
            changestuff[0].GetComponent<SpriteRenderer>().sprite = ishaanSprite;
			changestuff[1].GetComponent<SpriteRenderer>().sprite = ishaanShineSprite;
			changestuff[2].GetComponent<Light>().color = Color.red;
        }

        holdPos = FindObjectOfType<NeilNearTrigger>().holdPos; 

        phase2Phases = Phases.Chasing;
    }

    private bool isRegening = false;
    private bool isVunerable = false;
    private bool drinkingPot = false;
    public AudioClip neilRegenning;
    public AudioClip ishaanRegenning;
    public AudioClip potDrink;
    public AudioClip hitWhileDrinking;
    public AudioClip phase2Intro;
    public AudioClip neilPhase2Aud;
    public AudioClip destroyReality;
    public AudioClip realityBoopBoops;
    public AudioClip worldBreak;
    public AudioClip neilLetsFuckingGo;
    public AudioSource farSource;

    public Animator neil2CutsceneAnim;

    public GameObject neilVCam;
    public GameObject neilAmbience;

    public GameObject normalEnviorment;
    public GameObject destroyedEnviorment;

    public Transform phase2NeilSpawn;
    public Transform phase2PlayerSpawn;
    public GameObject neilPhase2;
    public GameObject neilPhase2VCam;

    public FluidMidi.StreamingAsset phase2Song;

    public SpriteRenderer neilSprite;
    public GameObject round2Theme;
    public GameObject playerHealth;
    public GameObject phase2PlayerPrefab;

    public Transform playerTransform;

    public float neilHitStun;
    public bool neilHit;
    public void ActivatePhase2()
    {
        midi.Stop();
        finaldrums.Stop();
        regenTimer = 99999f;
        neilHitStun = 999f;
        GameControllerScript.current.player.inNotebook = true;
        agent.speed = 0;

        canTakeDamage = false;

        IEnumerator WaitTime()
        {
            phase2 = true;

            yield return new WaitForSeconds(1.5f);

            neilVCam.SetActive(true);
            GameController.player.playerVCam.gameObject.SetActive(false);
            
            GameController.audioDevice.PlayOneShot(phase2Intro);
            GameController.audioDevice.PlayOneShot(neilPhase2Aud);
            foreach(NeilProjectileScript projectile in FindObjectsOfType<NeilProjectileScript>())
            {
                Destroy(projectile.gameObject);
            }
            yield return new WaitForSeconds(21.24f);
            
            neil2CutsceneAnim.gameObject.SetActive(true);
            GameController.audioDevice.PlayOneShot(destroyReality);
            GameController.audioDevice.PlayOneShot(realityBoopBoops);

            yield return new WaitForSeconds(destroyReality.length);

            neilAmbience.SetActive(true);

            GameController.audioDevice.Stop();

            GameController.audioDevice.PlayOneShot(worldBreak);

            destroyedEnviorment = GameObject.Instantiate<GameObject>(GameControllerScript.current.phase2Prefab, new Vector3(1005, 5, 5), GameControllerScript.current.phase2Prefab.transform.rotation);
            normalEnviorment.SetActive(false);

            phase2NeilSpawn = GameObject.FindGameObjectWithTag("Phase2NeilSpawn").transform;
            phase2PlayerSpawn = GameObject.FindGameObjectWithTag("Phase2PlayerSpawn").transform;
            phase2Spawns = GameObject.FindGameObjectsWithTag("Phase2ProjectileSpawns");
            GameController.player.cc.enabled = false;
            GameController.player.transform.position = new Vector3(phase2PlayerSpawn.position.x, GameController.player.transform.position.y, phase2PlayerSpawn.position.z);
            GameController.player.cc.enabled = true;

            agent.enabled = false;
            base.transform.position = new Vector3(phase2NeilSpawn.position.x, base.transform.position.y, phase2NeilSpawn.position.z);
            agent.enabled = true;

            neilVCam.SetActive(false);
            neilPhase2.SetActive(true);
            neilSprite.gameObject.SetActive(false);
            
            neilPhase2VCam.SetActive(true);

            playerHealth.SetActive(true);
            GameController.hptarget = 28;
            GameController.hpgenerating = true;

            GameController.player.gameObject.SetActive(false);

            UnlockedPlayerScript newPlayer = GameObject.Instantiate<GameObject>(phase2PlayerPrefab, phase2PlayerSpawn.position, phase2PlayerPrefab.gameObject.transform.rotation).GetComponent<UnlockedPlayerScript>();
            newPlayer.PlayerCamera.gameObject.SetActive(false);
            holdPos = newPlayer.holdPos;
            
            playerTransform = newPlayer.transform;

            yield return new WaitForSeconds(4f);

            neil2CutsceneAnim.gameObject.SetActive(false);
            GameController.audioDevice.PlayOneShot(neilLetsFuckingGo);

            yield return new WaitForSeconds(neilLetsFuckingGo.length);

            newPlayer.PlayerCamera.gameObject.SetActive(true);

            neilPhase2VCam.SetActive(false);

            round2Theme.SetActive(true);

            canTakeDamage = true;

            agent.speed = 33;
            GameController.player.runSpeed = 25;
            GameController.player.inNotebook = false;

            phase2Started = true;

            for (int i = 0; i < 5; i++)
            {
                Transform target = phase2Spawns[UnityEngine.Random.Range(0, phase2Spawns.Length)].transform;

                Transform thing = GameObject.Instantiate<GameObject>(GameControllerScript.current.neilProjectile, target.position, target.rotation).transform;
                thing.GetComponent<NeilProjectileScript>().player = GameControllerScript.current.player.transform;
                thing.GetComponent<NeilProjectileScript>().holdPos = holdPos; 

                lastPosition = thing.position;
            }
        }

        StartCoroutine(WaitTime());
    }

    private Transform holdPos;
    
    public GameObject playerFloatyLegs;
    public GameObject shield;

    public AudioClip[] phase2Hits;
    public AudioClip[] phase2Mads;

    private bool startedAttacking = false;

    public GameObject bulletPrefab;

    private float shootDuration = 0.7f;
    private float shootAmmount = 7;

    public void Update()
    {
        if (neilHitStun <= 0)
        {
            if (neilHit)
            {
                agent.speed = targetSpeed + 5;
                    
                FindObjectOfType<PlayerScript>().walkSpeed += 3.8f;
                FindObjectOfType<PlayerScript>().runSpeed += 3.8f;
                neilHit = false;
            }
        } else {
            neilHit = true;
            neilHitStun -= Time.deltaTime;
        }
        GameController.musicBeat.BPM = 200 * (midi.Tempo);
        neilPhase2.transform.LookAt(playerTransform);
        laserOrientation.LookAt(playerTransform);

        if (!startedAttacking && phase2Phases == Phases.Attacking && !dying)
        {
            startedAttacking = true;

            IEnumerator Attack()
            {
                yield return new WaitForSeconds(1.5f);

                for (int i = 0; i < shootAmmount; i++)
                {
                    GameObject.Instantiate<GameObject>(bulletPrefab, laserOrientation.position, laserOrientation.rotation);
                    yield return new WaitForSeconds(shootDuration);
                }

                yield return new WaitForSeconds(1);

                shootAmmount += 2;
                shootDuration -= 0.1f;

                phase2Phases = Phases.Chasing;
                startedAttacking = false;
            }

            StartCoroutine(Attack());
        }

        if (phase2 && phase2Started && shieldTime > 0f)
        {
            if (phase2Phases == Phases.Chasing)
                shieldTime -= Time.deltaTime;
                
            shield.SetActive(true);
            shieldsUp = true;
        }
        else
        {
            shieldsUp = false;
            shield.SetActive(false);
        }
            
        if (GameController.debug && Input.GetKeyDown(KeyCode.K) && startedBoss)
        {
            GameController.neilhp = 2f;
        }
        
        if (targetPlayer)
            StartTarget();

        if (regenTimer > 0f && canRegen)
            regenTimer -= Time.deltaTime;

        if (regenTimer <= 0f && GameController.neilhp < 6 && canRegen && !isRegening)
        {
            isRegening = true;
            isVunerable = true;

            IEnumerator Regen()
            {
                immunetophase1damage = true;
                if (!GameControllerScript.current.ishaanMode)
                {
                    farSource.PlayOneShot(neilRegenning);
                    StartCoroutine(TempDisableSpeed(1.5f + neilRegenning.length));
                } else {
                    farSource.PlayOneShot(ishaanRegenning);
                    StartCoroutine(TempDisableSpeed(1.5f + ishaanRegenning.length));
                }
                midi.Gain = 0.01f;
                finaldrums.Gain = 0.01f;
                finaldrums.Tempo = 0.01f;
                storetempo += 0.1f;
                if (!GameControllerScript.current.ishaanMode)
                    yield return new WaitForSeconds(neilRegenning.length);
                else
                    yield return new WaitForSeconds(ishaanRegenning.length);
                print("it should play...");
                isVunerable = false;
                farSource.PlayOneShot(potDrink);
                drinkingPot = true;
                yield return new WaitForSeconds(1.5f);
                midi.Tempo = storetempo;
                midi.Gain = 0.5f;
                if (!hitPot)
                {
                    if ((GameController.neilhp + 4f) <= 28)
                        GameController.hptarget = (GameController.neilhp + 8f);
                    else
                        GameController.hptarget = 28;
                    GameController.hpgenerating = true;
                    drinkingPot = false;
                }   

                canRegen = false;
                immunetophase1damage = false;
            }

            StartCoroutine(Regen());
        }

        if (agent.speed < targetSpeed && doSpeed)
            agent.speed += 0.5f;
        else if (doSpeed)
            doSpeed = false;
    }

    IEnumerator TempDisableSpeed(float time)
    {
        float startingSpeed = agent.speed;

        agent.speed = 0;
        yield return new WaitForSeconds(time);
        agent.speed = startingSpeed;
    }

    public void PreBossAudio()
    {
        if (!GameControllerScript.current.ishaanMode)
            source.PlayOneShot(neilPreBoss);
        else
            source.PlayOneShot(ishaanPreBoss);
    }

    public BoxCollider neilPostProcessing;

    public void PostBossStart()
    {
        SongAfterBoss();

        RenderSettings.skybox = GameControllerScript.current.galaxySky;
        GameController.wall.SetFloat("_Vert", 1);
        GameController.wall.SetFloat("_VertexPrecision", 0.01f);
        source.Stop();
        source.PlayOneShot(globalHitSound);
        base.GetComponent<Animator>().SetTrigger("Hit");
        if (!GameControllerScript.current.ishaanMode)
            source.PlayOneShot(neilPostBoss);
        else
            source.PlayOneShot(ishaanPostBoss);

        startedBoss = true;

        FindObjectOfType<PlayerScript>().walkSpeed = 40;
        FindObjectOfType<PlayerScript>().runSpeed = 40;
        FindObjectOfType<PlayerScript>().infiniteStamina = true;
        FindObjectOfType<PlayerScript>().staminaByDefault = true;

        targetSpeed = agent.speed;

        IEnumerator WaitTime()
        {
            yield return new WaitForSeconds(8.2f);
            GameControllerScript.current.GenerateHP();
            yield return new WaitForSeconds(2.8f);
            targetPlayer = true;
            agent.speed = 0;
            doSpeed = true;

            neilPostProcessing.enabled = true;

            flash.gameObject.SetActive(true);
            flash.SetTrigger("Flash");

            for (int i = 0; i < 5; i++)
            {
                Transform target = projPoints[UnityEngine.Random.Range(0, projPoints.Length)];

                Transform thing = GameObject.Instantiate<GameObject>(GameControllerScript.current.neilProjectile, target.position, target.rotation).transform;
                thing.GetComponent<NeilProjectileScript>().player = GameControllerScript.current.player.transform;
                thing.GetComponent<NeilProjectileScript>().holdPos = FindObjectOfType<NeilNearTrigger>().holdPos; 

                lastPosition = thing.position;
            }

            // GameController.EnableWallBeatHit();
        }
        
        StartCoroutine(WaitTime());
    }

    public void BeatHit()
    {
        float distancefromplayer = Vector3.Distance(GameController.playerTransform.position, transform.position);
        GameController.wall.SetFloat("_VertexPrecision", distancefromplayer/200);
    }

    [HideInInspector] public bool cameFromShield = false;

    public void StartTarget()
    {
        if (phase2Phases == Phases.Chasing)
        {
            agent.SetDestination(playerTransform.position);
            agent.isStopped = false;
        }
        else
            agent.isStopped = true;
    }

    public void SongAfterBoss()
    {
        crackhouseTrouble.Stop();
        crackhouseTrouble.clip = hitSong;
        crackhouseTrouble.Play();

        IEnumerator HitStuff()
        {
            yield return new WaitForSeconds(hitSong.length);
            crackhouseTrouble.Stop();
            midi.song = crackLoop;
            midi.Play();

            GameController.musicBeat.BPM = 200;
            GameController.musicBeat.syncBPM();
            GameController.musicBeat.beatHit.AddListener(BeatHit);
        }

        StartCoroutine(HitStuff());
    }

    public AudioClip hit;
    public Transform[] projPoints;

    bool noFloors = false;

    public Animator flash;

    private bool hitPot = false;

    public Transform laserOrientation;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "NeilProjectile" && other.GetComponent<NeilProjectileScript>().shootForward && canTakeDamage && !immunetophase1damage)
        {
            if (!phase2)
            {
                GameObject.Destroy(other.gameObject);

                if (!startedBoss)
                    PostBossStart();
                else
                {
                    if (GameController.neilhp < 3f)
                    {
                        GameControllerScript.current.neilhp -= 2;
                        GameController.audioDevice.PlayOneShot(globalHitSound);
                        if (!ishaanMode)
                            ActivatePhase2();
                        else
                        {
                            foreach(NeilProjectileScript projectile in FindObjectsOfType<NeilProjectileScript>())
                            {
                                Destroy(projectile.gameObject);
                            }
                            neilPostProcessing.enabled = false;
                            flash.gameObject.SetActive(true);
                            flash.SetTrigger("Flash");
                            foreach(MeshRenderer renderer in FindObjectsOfType<MeshRenderer>())
                                {
                                    if (renderer.gameObject.name.ToLower().Contains("floor") || renderer.gameObject.name.ToLower().Contains("ceiling"))
                                    {
                                        renderer.enabled = true;
                                    }
                                }
                            Destroy(crackhouseTrouble);
                            Destroy(finaldrums);
                            Destroy(midi);
                            GameControllerScript.current.IshaanDeath();
                        }
                    }
                    else
                    {
                        if (!drinkingPot)
                        {
                            canRegen = true;
                            regenTimer = 10f;

                            GameControllerScript.current.hpgenerating = false;
                            GameControllerScript.current.neilhp -= 2;

                            if (!noFloors && GameControllerScript.current.neilhp < 15)
                            {
                                noFloors = true;
                                
                                foreach(MeshRenderer renderer in FindObjectsOfType<MeshRenderer>())
                                {
                                    if (renderer.gameObject.name.ToLower().Contains("floor") || renderer.gameObject.name.ToLower().Contains("ceiling"))
                                    {
                                        renderer.enabled = false;
                                    }
                                }

                                flash.gameObject.SetActive(true);
                                flash.SetTrigger("Flash");
                            }

                            if (agent.speed != 0)
                                targetSpeed = agent.speed;

                            agent.speed = 0;

                            IEnumerator WaitTime()
                            {
                                source.PlayOneShot(hit);
                                base.GetComponent<Animator>().SetTrigger("Hit");
                                GameController.audioDevice.PlayOneShot(globalHitSound);
                                if (GameController.neilhp < 3)
                                {
                                    print("does this even call");
                                    midi.Tempo = 0.001f;
                                    finaldrums.Tempo = storetempo + 0.1f;
                                    finaldrums.Gain = 0.5f;
                                    finaldrums.Play();
                                } else {
                                    midi.Tempo += 0.1f;
                                    GameController.chromaticEffect.intensity.value = 1f;
                                    GameController.bloomEffect.intensity.value = 100f;

                                    IEnumerator Exposure()
                                    {
                                        GameController.exposure.keyValue.value = 5f;
                                        yield return new WaitForSeconds(0.2f);
                                        GameController.exposure.keyValue.value = 1f;
                                    }
                                    
                                    StartCoroutine(Exposure());
                                    
                                    GameController.player.cameraAnimator.Play("CameraNeilHit");
                                    storetempo = midi.Tempo;
                                }
                                neilHitStun += 1;
                                return null;
                            }
                            
                            StartCoroutine(WaitTime());
                        }
                        else if (drinkingPot)
                        {
                            hitPot = true;

                            source.PlayOneShot(hitWhileDrinking);
                            source.PlayOneShot(hit);
                        }
                    }
                }
            }
            else if (phase2 && canBeHit)
            {
                if (!shieldsUp)
                {
                    GameObject.Destroy(other.gameObject);

                    GameController.audioDevice.PlayOneShot(globalHitSound);

                    GameController.neilhp -= 2;

                    shieldTime = UnityEngine.Random.Range(20,30);

                    phase2Phases = Phases.Attacking;

                    if (GameController.neilhp <= 0f)
                    {
                        canBeHit = false;
                        dying = true;

                        playerHealth.SetActive(false);
                        
                        phase2Animator.SetTrigger("Die");
                        round2Theme.GetComponent<AudioSource>().Stop();
                        killfeed.SetActive(true);
                        farSource.volume = 0.25f;
                        farSource.PlayOneShot(die);
                        tf2sounds.PlayOneShot(tf2killsound);
                        agent.isStopped = true;

                        IEnumerator Wait()
                        {
                            yield return new WaitForSeconds(3); 
                           
                            theEndAnimation.SetActive(true);
                            yield return new WaitForSeconds(2);
                            farSource.volume = 1f;
                            FindObjectOfType<UnlockedPlayerScript>().enabled = false;
                            killfeed.SetActive(false);
                            neilhealthobject.SetActive(false);
                            yield return new WaitForSeconds(3);
                            windCutscene.SetActive(true);
                            FindObjectOfType<UnlockedPlayerScript>().gameObject.SetActive(false);
                            destroyedEnviorment.SetActive(false); // OPTIMIZATION SHITS
                            finaleCutscene.SetActive(true);
                            GameControllerScript.current.player.playerVCam.gameObject.SetActive(false);
                            GameController.player.enabled = false;
                            
                            yield return new WaitForSeconds(5);
                            theEndAnimation.SetActive(false);
                            fightMercy.SetActive(true);
                        }

                        StartCoroutine(Wait());
                    }
                    else
                    {
                        IEnumerator NeilPissedAudio()
                        {
                            if (!dying)
                            {
                                print("im angry!");
                                source.PlayOneShot(phase2Hits[UnityEngine.Random.Range(0, phase2Hits.Length)]);
                                yield return new WaitForSeconds(1);
                                source.PlayOneShot(phase2Mads[UnityEngine.Random.Range(0, phase2Mads.Length)]);
                            }
                        }

                        StartCoroutine(NeilPissedAudio());
                    }
                }
                else
                {
                    GameObject.Destroy(other.gameObject);

                    shieldTime -= 5;
                    tf2sounds.PlayOneShot(ding, 0.75f);
                }
            }
        }

        else if (other.gameObject.tag == "Player" && targetPlayer && !cameFromShield)
            {
                if (phase2)
                {
                    if (GameController.neilhp > 0)
                        GameController.PlayerDamage(2);

                    if (GameController.neilPlayerHP <= 0f && !dying)
                    {   
                        regenTimer = 9999f;
                        GameController.player.gameObject.SetActive(true);  
                        GameController.player.gameOver = true;
                        playerTransform.gameObject.SetActive(false);
                        round2Theme.SetActive(false);
                        GameController.player.playerVCam.gameObject.SetActive(true);
                        GameController.currentDeathCamera = 1;

                        IEnumerator DisableSelf()
                        {
                            yield return null;
                            GameController.Destroy(gameObject);
                        }

                        StartCoroutine(DisableSelf());
                    }
                }  
                else if (!dying)
                {
                    GameController.player.gameOver = true;
                    GameController.currentDeathCamera = 1;
                }       
            }

        cameFromShield = false;
    }

    public GameObject windCutscene;
    public GameObject fightMercy;
    private bool canTakeDamage = true;
    public Transform hammerSpawnPos;
    public GameObject hammerPart;
    bool dying = false;
    private bool canBeHit = true;    
    public AudioClip theEnd;
    public GameObject finaleCutscene;
    public GameObject theEndAnimation;
    public AudioClip die;
    public AudioClip diee;
    public GameObject[] phase2Spawns;

    public AudioClip ding;
    public AudioClip tf2killsound;
    public GameObject killfeed;

    public bool phase2 = false;

    public GameControllerScript GameController;
    [SerializeField]private AudioClip globalHitSound;
}

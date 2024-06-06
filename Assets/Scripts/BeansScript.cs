using UnityEngine;
using UnityEngine.AI;
using System.Collections;
 
public class BeansScript : MonoBehaviour
{
    public float cooldown;

    private AudioSource audioDevice;

    public GameObject gumPrefab;
    private GameObject gum;

    public static Sprite spriteNPCGum;
    public Sprite npcHitGumSprite;

    public AudioClip aud_spit;
    public AudioClip reload;

    private NavMeshAgent agent;
 
    private bool chewing;
 
    public AudioClip[] aud_chewing = new AudioClip[5];
    public AudioClip[] aud_skipping = new AudioClip[5];
    public AudioClip[] aud_playerHit = new AudioClip[5];
    public AudioClip[] aud_npcHit = new AudioClip[5];
    
    private bool animationWalking = false;

    public Animator sprite;

    private void Start()
    {
        spriteNPCGum = npcHitGumSprite;
        agent = GetComponent<NavMeshAgent>();
        audioDevice = GetComponent<AudioSource>();

        Wander();
    }

    private void Update()
    {
        if (cooldown > 0f)
            cooldown -= Time.deltaTime;

        if (agent.velocity.magnitude > 0f)
        {
            if (!animationWalking)
            {
                sprite.SetTrigger("Walk");
                animationWalking = true;
            }
        }
        if (agent.velocity.magnitude <= 0f)
        {
            if (animationWalking)
            {
                sprite.SetTrigger("StopWalk");
                animationWalking = false;
            }
        }
    }
 
    private void FixedUpdate()
    {
        Vector3 direction = GameControllerScript.current.player.transform.position - base.transform.position;
        RaycastHit raycastHit;

        agent.isStopped = chewing;

        if (!chewing)
        {
            if (agent.velocity.magnitude <= 0.4f)
                Wander();
        }

        if (Physics.Raycast(base.transform.position + Vector3.up * 2f, direction, out raycastHit, float.PositiveInfinity, 769, QueryTriggerInteraction.Ignore) & raycastHit.transform.tag == "Player" && cooldown <= 0f)
        {
            cooldown = 30f;
            chewing = true;
            StartCoroutine(Spit());
        }
    }

    private void Wander()
    {
        AILocationSelectorScript wanderer = FindObjectOfType<AILocationSelectorScript>();

        wanderer.GetNewTarget();
        agent.SetDestination(wanderer.transform.position);

        if (Random.Range(0f, 99f) >= 98f & !audioDevice.isPlaying)
            audioDevice.PlayOneShot(aud_skipping[Random.Range(0, 4)]);
    }

    public Transform orientation;
 
    private IEnumerator Spit()
    {
        audioDevice.Stop();
        audioDevice.PlayOneShot(aud_chewing[Random.Range(0, 4)]);

        orientation.LookAt(new Vector3(GameControllerScript.current.player.transform.position.x, transform.position.y, GameControllerScript.current.player.transform.position.z));

        yield return new WaitForSeconds(3f);

        audioDevice.PlayOneShot(reload);

        yield return new WaitForSeconds(2f);

        Vector3 pos = transform.position;
        pos.y = gumPrefab.transform.position.y;
        orientation.eulerAngles = new Vector3(0, orientation.eulerAngles.y, orientation.eulerAngles.z);
        if (PlayerPrefs.GetFloat("sniper_beans") == 1)
            orientation.LookAt(new Vector3(GameControllerScript.current.player.transform.position.x, transform.position.y, GameControllerScript.current.player.transform.position.z));
        gum = Object.Instantiate(gumPrefab, pos, orientation.rotation);
        if (PlayerPrefs.GetFloat("sniper_beans") == 1)
            gum.GetComponent<BsodaSparyScript>().speed = 240f;
        else 
            gum.GetComponent<BsodaSparyScript>().speed = 60f;

        gum.name = "Gum";
        audioDevice.PlayOneShot(aud_spit);
        if (PlayerPrefs.GetFloat("double_shoot") == 1)
        {
            for (int i = 0; i < 1; i++)
            {
                yield return new WaitForSeconds(1f);
                orientation.LookAt(new Vector3(GameControllerScript.current.player.transform.position.x, transform.position.y, GameControllerScript.current.player.transform.position.z));
                gum = Object.Instantiate(gumPrefab, pos, orientation.rotation);

                if (PlayerPrefs.GetFloat("sniper_beans") == 1)
                    gum.GetComponent<BsodaSparyScript>().speed = 240f;
                else 
                    gum.GetComponent<BsodaSparyScript>().speed = 60f;

                gum.name = "Gum";
                audioDevice.PlayOneShot(aud_spit);
            }
        }
        GameControllerScript.current.eventDelay = 10f;
        chewing = false;
        yield break;
    }
 
    public void SorryPlayer()
    {
        audioDevice.PlayOneShot(aud_playerHit[Random.Range(0, 4)]);
    }
 
    public void SorryNPC()
    {
        audioDevice.PlayOneShot(aud_npcHit[Random.Range(0, 4)]);
    }
}
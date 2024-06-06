using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class BattleControllerScript : MonoBehaviour
{
    public static BattleControllerScript current;
    
    [Header("NPC DATA")]
    public string cname = "MrBeast";
    public string spareText = "You talk to MrBeast about the YouTube algorithm.\n You two get really invested.";
    public string checkTextCustom = "Not too happy about his credit card being stolen.";

    private bool skipDialogue = false;

    public string[] mrBeastTalks = 
    {
        "HEY, DO YOU WANT TO PARTICIPATE, IN MY YOUTUBE CHALLENGE?",
        "EH, I'LL GIVE IT A 7.",
        "FIRST ONE TO LEAVE GETS FUCKING OBLITERATED.\nSTARTING NOW.",
        "HEY, DO YOU WANT TO PARTICIPATE, IN YOUR ULTIMATE DEMISE?",
        "YOU'RE GONNA HAVE A BEAST TIME",
        "BYE BYE!",
        "DON'T MAKE ME GET OUT THE BLEACH AGAIN",
        "YOUR LIFE IS A BAD INTRO"
    };

    public string[] taunts = 
    {
        "You feel your subscriber count getting higher.",
        "MRBEAST looks you in the eyes."
    };

    public float beasthp = 250;
    public int bossDmg = 10;

    [Header("Player Data")]
    public float playerhp = 100;
    public float playermaxhp = 100;
    public float playeratk = 10;
    public float playerdef = 1;

    [Header("Internal")]
    public int fight;
    public GameControllerScript gc;
    public TextMeshProUGUI youwon;
    public GameObject results;
    public Animator battleUI;
    public float spare = 0;
    public float spareamount = 1;
    public Animator selectHud;
    public Animator damageTextanim;
    public TextMeshProUGUI damageText;

    public Slider hpslider;
    public Slider beasthpslider;
    public Slider[] mercysliders = new Slider[2];
    

    public TextMeshProUGUI hptext;
    public TextMeshProUGUI sayText;
    
    private List<Coroutine> activeSays = new List<Coroutine>();

    public GameObject mrBeastTextBox;
    public TextMeshPro mrBeastText;

    public AudioClip takeDamage;
    public AudioClip winsound;
    public AudioSource music;
    public AudioSource textbeepSoruce;
    public AudioClip mrbeastbeep;
    public AudioClip textbeep;
    public GameObject mrbeast;

    public GameObject attackUI;
    public GameObject playerweapon;
    public Sprite principalkeys;
    public Sprite vhs;
    public Sprite glock;
    public Sprite hammer;

    public void TakeDamage(int ammount, GameObject sender = null)
    {
        if (sender != null)
            GameObject.Destroy(sender);

        playerhp -= Mathf.RoundToInt(ammount / playerdef);
        GetComponent<AudioSource>().pitch = Random.Range(0.8f,1.2f);
        GetComponent<AudioSource>().PlayOneShot(takeDamage);
    }

    public IEnumerator MrBeastSay(string text, UnityEngine.Events.UnityEvent onFinish)
    {
        mrBeastTextBox.SetActive(true);
        mrBeastText.text = "";
        yield return new WaitForSeconds(0.5f);

        foreach(char chara in text.ToCharArray())
        {
            mrBeastText.text += chara;
            if (chara.ToString() != " ")
            {
                textbeepSoruce.clip = mrbeastbeep;
                textbeepSoruce.Play();
            }
            yield return new WaitForSeconds(0.02f);
        }

        yield return new WaitForSeconds(1f);

        mrBeastTextBox.SetActive(false);
        onFinish.Invoke();
    }

    void Start()
    {
        gc = GameControllerScript.current;
        selectHud.SetTrigger("Up");

        SayMessage(cname.ToUpper() + " appears.");

        Time.timeScale = 1f;
    }

    bool gameOver = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
            skipDialogue = true;
        hpslider.value = (playerhp/playermaxhp);
        beasthpslider.value = ((beasthp/250)*(1f-0.42112f))+0.42112f;
        for (int i = 0; i < 2; i++)
        {
            mercysliders[i].value = 0.4f+(spare/100)*0.6f;
        }
        hptext.text = (playerhp + " / " + playermaxhp);

        if (playerhp <= 0f && !gameOver)
        {
            SceneManager.LoadSceneAsync("GameOver");
            gameOver = true;
        }
    }

    public void SayMessage(string message, UnityEngine.Events.UnityEvent onComplete = null)
    {
        skipDialogue = false;
        foreach(Coroutine coroutine in activeSays)
        {
            StopCoroutine(coroutine);
        }
        
        activeSays.Add(StartCoroutine(Say(message, onComplete)));
    }

    public void ActSelect(int index, GameObject root)
    {
        FindObjectOfType<SelectHud>().inSubMenu = false;
        root.SetActive(false);
        if (index == 0)
        {
            UnityEvent onComplete = new UnityEvent();
            onComplete.AddListener(Attack);

            SayMessage(cname.ToUpper() + " - ATK 10 DEF 10\n* " + checkTextCustom, onComplete);
        }
        if (index == 1)
        {
            UnityEvent onComplete = new UnityEvent();
            onComplete.AddListener(Attack);
            spare += spareamount;
            if (spare < 100)
                damageText.text = "+" + spareamount + "%";
            else
                damageText.text = "MAX";
            spareamount *= 2;
            damageTextanim.SetTrigger("Spare");

            SayMessage(spareText, onComplete);
        }
    }
    public void ItemSelect(int itemid, GameObject root)
    {
        FindObjectOfType<SelectHud>().inSubMenu = false;
        root.SetActive(false);
        if (itemid == 1)
        {
            UnityEvent onComplete = new UnityEvent();
            onComplete.AddListener(Attack);
            playerhp = playermaxhp;
            SayMessage("You ate a Zesty Bar! \n* Your health was maxxed out.", onComplete);
        }
        if (itemid == 3)
        {
            UnityEvent onComplete = new UnityEvent();
            onComplete.AddListener(Attack);
            playeratk = 50;
            playerweapon.GetComponent<SpriteRenderer>().sprite = principalkeys;
            SayMessage("You equipped the keys.", onComplete);
        }
        if (itemid == 4)
        {
            UnityEvent onComplete = new UnityEvent();
            onComplete.AddListener(Attack);
            playermaxhp = 200;
            playerhp *= 2;
            SayMessage("You drank the BSODA. \n* Your max health doubled!", onComplete);
        }
        if (itemid == 6)
        {
            UnityEvent onComplete = new UnityEvent();
            onComplete.AddListener(Attack);
            playeratk = 30;
            playerweapon.GetComponent<SpriteRenderer>().sprite = vhs;
            SayMessage("You equipped the tape.", onComplete);
        }
        if (itemid == 7)
        {
            UnityEvent onComplete = new UnityEvent();
            onComplete.AddListener(Attack);
            SayMessage("You wanted to use the clock..\n* but nobody has time for that. \n* You didn't use the clock.", onComplete);
        }
        if (itemid == 8)
        {
            UnityEvent onComplete = new UnityEvent();
            onComplete.AddListener(Attack);
            playeratk += 10;
            SayMessage("You shined your weapon. Attack increased!", onComplete);
        }
        if (itemid == 9)
        {
            UnityEvent onComplete = new UnityEvent();
            onComplete.AddListener(Attack);
            playeratk = 9999;
            playerweapon.GetComponent<SpriteRenderer>().sprite = glock;
            SayMessage("You equipped the glock.", onComplete);
        }
        if (itemid == 10)
        {
            UnityEvent onComplete = new UnityEvent();
            onComplete.AddListener(Attack);
            playerdef = 2;
            SayMessage("You equipped the boots. Defense increased!", onComplete);
        }
        if (itemid == 11)
        {
            UnityEvent onComplete = new UnityEvent();
            onComplete.AddListener(Attack);
            SayMessage("You used the Poop Meter. \n* You need to poop in...\n*Oh yea, you're fighting MrBeast.", onComplete);
        }
        if (itemid == 12)
        {
            UnityEvent onComplete = new UnityEvent();
            onComplete.AddListener(Attack);
            playerhp = 1;
            SayMessage("unused item alert!!!!!!!!!\n* Your health was... minimized out?", onComplete);
        }
        if (itemid == 13)
        {
            UnityEvent onComplete = new UnityEvent();
            onComplete.AddListener(Attack);
            SayMessage("You show Mrbeast his own credit card.\n* He :Moyai:s.", onComplete);
        }
        if (itemid == 14)
        {
            UnityEvent onComplete = new UnityEvent();
            onComplete.AddListener(Attack);

            SayMessage("You called for help... \n* but this phone doesn't have a SIM Card.", onComplete);
        }
        if (itemid == 15)
        {
            UnityEvent onComplete = new UnityEvent();
            onComplete.AddListener(Attack);
            SayMessage("fart joke", onComplete);
        }
        if (itemid == 16)
        {
            UnityEvent onComplete = new UnityEvent();
            onComplete.AddListener(Attack);
            playeratk = 100;
            playerweapon.GetComponent<SpriteRenderer>().sprite = hammer;
            SayMessage("You equipped the hammer.", onComplete);
        }
        if (itemid == 17)
        {
            UnityEvent onComplete = new UnityEvent();
            onComplete.AddListener(Attack);
            playerhp += 75;
            SayMessage("You drank the 15 second energy.\n* You gained 75 HP!", onComplete);
        }
        if (itemid == 18)
        {
            UnityEvent onComplete = new UnityEvent();
            onComplete.AddListener(Attack);
            SayMessage("You tried to give MrBeast the crackpipe.\n* He declines.\n* What a shame.", onComplete);
        }
        if (itemid == 19)
        {
            UnityEvent onComplete = new UnityEvent();
            onComplete.AddListener(Attack);
            SayMessage("You used the Get Out of Jail Free Card.\n* You aren't in jail. \n* You didn't use the Get Out of Jail Free Card.", onComplete);
        }
        if (itemid == 20)
        {
            UnityEvent onComplete = new UnityEvent();
            onComplete.AddListener(Attack);
            SayMessage("no monke :sadnesscombat:", onComplete);
        }
        if (itemid == 21)
        {
            UnityEvent onComplete = new UnityEvent();
            onComplete.AddListener(Attack);
            playerhp += 5;
            SayMessage("You ate the potato.\n* You gained 5 HP!", onComplete);
        }
        if (itemid == 22)
        {
            UnityEvent onComplete = new UnityEvent();
            onComplete.AddListener(Attack);
            SayMessage("You try to use the Ender Pearl.\n* You teleported 1 meter forward.\n* Very useful.", onComplete);
        }
        if (itemid == 23)
        {
            UnityEvent onComplete = new UnityEvent();
            onComplete.AddListener(Attack);
            playerhp -= 5;
            SayMessage("You use the Lucky Block.\n* You lose 5 HP!", onComplete);
        }
        if (itemid == 24)
        {
            UnityEvent onComplete = new UnityEvent();
            onComplete.AddListener(Attack);
            SayMessage("You use the GameBoy Color.\n* It explodes in your hand.", onComplete);
        }
        if (itemid == 25)
        {
            UnityEvent onComplete = new UnityEvent();
            onComplete.AddListener(Attack);
            SayMessage("Polish cow won't be able to teleport you out of this one.\n* Nor will he be able to teleport you into this one.", onComplete);
        }
        if (itemid == 26)
        {
            UnityEvent onComplete = new UnityEvent();
            onComplete.AddListener(Attack);
            SayMessage("You use the Portal Gun.\n* There are no Balls Posters here.", onComplete);
        }
    }
    public void MercySelect(int index, GameObject root)
    {
        IEnumerator winpacifist()
        {
            music.mute = true;
            GetComponent<AudioSource>().PlayOneShot(winsound);
            youwon.text = "* You Won!\n* You got 0 EXP and $1.";
            gc.AddMoney(1f);
            results.SetActive(true);
            yield return new WaitForSeconds(winsound.length);
            gc.OnDeltaComplete(false);
            
            GameJolt.API.Trophies.TryUnlock(188675);

            FindObjectOfType<PlayerStats>().Save();
        }
        FindObjectOfType<SelectHud>().inSubMenu = false;
        root.SetActive(false);
        if (index == 0)
        {
            UnityEvent onComplete = new UnityEvent();
            onComplete.AddListener(Attack);
            if (spare < 100)
                SayMessage("You spared" + cname.ToUpper() + ", but he wasn't spared enough!", onComplete);
            else
                StartCoroutine(winpacifist());
        }
    }

    public void Attack()
    {
        FindObjectOfType<SelectHud>().inSubMenu = false;
        
        IEnumerator win()
        {
            music.mute = true;
            mrbeast.SetActive(false);
            GameJolt.API.Trophies.TryUnlock(188674);
            FindObjectOfType<PlayerStats>().Save();
            GetComponent<AudioSource>().PlayOneShot(winsound);
            youwon.text = "* You Won!\n* You got 0 EXP and $1.";
            gc.AddMoney(1f);
            results.SetActive(true);
            yield return new WaitForSeconds(winsound.length);
            gc.OnDeltaComplete(true);
            
        }
        if (beasthp > 0)
        {
            battleUI.SetTrigger("Down");
            selectHud.SetTrigger("Down");
            UnityEngine.Events.UnityEvent onFinish = new UnityEngine.Events.UnityEvent();
            onFinish.AddListener(EnableSoul);

            StartCoroutine(MrBeastSay(mrBeastTalks[UnityEngine.Random.Range(0, mrBeastTalks.Length)], onFinish));
        } else {
            StartCoroutine(win());
        }
    }

    int curAttack = 0;
    public GameObject battleStuff;
    public GameObject[] attacks;
    
    public SoulScript soul;

    public void EnableSoul()
    {
        IEnumerator Attack()
        {
            curAttack = curAttack += 1;
            if (curAttack > attacks.Length - 1)
            {
                curAttack = 0;
            }
            battleStuff.SetActive(true);
            attacks[curAttack].SetActive(true);

            soul.Init();
            if (curAttack == 0)
                attacks[curAttack].GetComponent<BeastAttack01>().Attack();
            else
                attacks[curAttack].GetComponent<BeastAttack02>().Attack();
            
            yield return new WaitForSeconds(5f);

            battleStuff.SetActive(false);
            attacks[curAttack].SetActive(false);

            selectHud.SetTrigger("Up");
            battleUI.SetTrigger("Up");
            
            attackUI.SetActive(false);
            attackUI.GetComponent<ThrowAttackScript>().canInteract = false;
            FindObjectOfType<SelectHud>().ExitSubMenu();

            if (curAttack == 0)
                attacks[curAttack].GetComponent<BeastAttack01>().Reset();
            else
                attacks[curAttack].GetComponent<BeastAttack02>().Reset();

            FindObjectOfType<BattleControllerScript>().SayMessage(taunts[UnityEngine.Random.Range(0, taunts.Length)]);
        }


        StartCoroutine(Attack());   
    }

    IEnumerator Say(string text, UnityEngine.Events.UnityEvent onComplete)
    {
        sayText.text = "* ";

        foreach(char character in text.ToCharArray())
        {
            sayText.text += character;
            if (!skipDialogue)
            {
                if (character.ToString() != " ")
                {
                    textbeepSoruce.clip = textbeep;
                    textbeepSoruce.Play();
                }
                yield return new WaitForSeconds(0.025f);
            }
        }
        
        yield return new WaitForSeconds(0.5f);

        if (onComplete != null)
        {
            skipDialogue = false;
            onComplete.Invoke();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectHud : MonoBehaviour
{
    public BattleControllerScript battleController;
    public AudioSource auSource;
    public AudioClip impact;
    public AudioClip enter;
    public AudioClip refuse;
    public bool canPress = true;
    public bool inSubMenu = false;
    public int selectedButton;
    public Image[] buttons;
    public Image[] colorbits;

    private List<Sprite> buttonNormalSprite = new List<Sprite>();
    public Sprite[] buttonSelectedSprite;

    public Sprite monohead;
    public Image head;
    public Image atkhead;

    public GameObject fightUI;
    public GameObject actUI;
    public GameObject itemUI;
    public GameObject mercyUI;
    private Color uicolor;

    private void Start()
    {
        float r = FindObjectOfType<PlayerStats>().data.playercolor_r;
        float g = FindObjectOfType<PlayerStats>().data.playercolor_g;
        float b = FindObjectOfType<PlayerStats>().data.playercolor_b;
        uicolor = new Color(r/255,g/255,b/255);
        if (PlayerPrefs.GetFloat("HUDPlayerColor") == 1)
        {
            foreach (Image image in colorbits)
            {
                image.color = uicolor;
            }
            head.sprite = monohead;
            head.color = uicolor;
            atkhead.sprite = monohead;
            atkhead.color = uicolor;
            FindObjectOfType<BattleControllerScript>().damageText.color = uicolor;
        }
        foreach(Image button in buttons)
        {
            buttonNormalSprite.Add(button.sprite);
        }

        for(int i = 0; i < buttons.Length; i++)
        {
            if (selectedButton == i)
            {
                buttons[i].sprite = buttonSelectedSprite[i];
            }
            else
            {
                buttons[i].sprite = buttonNormalSprite[i];
            }
        }
    }

    private void Update()
    {
        if (canPress)
        {
            battleController.GetComponent<AudioSource>().pitch = 1;
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                Select(-1);
            else if (Input.GetKeyDown(KeyCode.RightArrow))
                Select(1);

            if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Z)) && !inSubMenu)
                EnterSubMenu();
        }

        if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.X)) && inSubMenu)
        {
            ExitSubMenu();
        }
    }

    public void ExitSubMenu()
    {
        inSubMenu = false;
        actUI.GetComponent<ActSelectScript>().canInteract = false;
        itemUI.GetComponent<deltaitemscript>().canInteract = false;
        mercyUI.GetComponent<MercySelectScript>().canInteract = false;
        actUI.SetActive(false);
        itemUI.SetActive(false);
        mercyUI.SetActive(false);
        ToggleSelecting(true);

        if (selectedButton == 0)
        {
            fightUI.SetActive(false);
            fightUI.GetComponent<FightSelectScript>().canInteract = false;
        }
    }

    public void EnterSubMenu()
    {
        auSource.PlayOneShot(enter);

        if (selectedButton == 0)
        {
            fightUI.SetActive(true);
            fightUI.GetComponent<FightSelectScript>().canInteract = true;
        }
        else if (selectedButton == 1)
        {
            actUI.SetActive(true);
            actUI.GetComponent<ActSelectScript>().canInteract = true;
        }
        else if (selectedButton == 2)
        {
            itemUI.SetActive(true);
            itemUI.GetComponent<deltaitemscript>().canInteract = true;
        }
        else if (selectedButton == 3)
        {
            mercyUI.SetActive(true);
            mercyUI.GetComponent<MercySelectScript>().canInteract = true;
        }
        
        ToggleSelecting(false);
        inSubMenu = true;
    }

    private void Select(int change)
    {
        auSource.PlayOneShot(impact);

        selectedButton += change;

        if (selectedButton > buttons.Length - 1)
            selectedButton = 0;
        else if (selectedButton < 0)
            selectedButton = buttons.Length - 1;

        for(int i = 0; i < buttons.Length; i++)
        {
            if (selectedButton == i)
            {
                buttons[i].sprite = buttonSelectedSprite[i];
            }
            else
            {
                buttons[i].sprite = buttonNormalSprite[i];
            }
        }
    }

    public void ToggleSelecting(bool selectOn)
    {
        canPress = selectOn;

        if (canPress)
            battleController.selectHud.SetTrigger("Up");
        else
            battleController.selectHud.SetTrigger("Down");
    }
}

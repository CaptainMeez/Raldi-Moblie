using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

enum ShopState {Menu, Buy, Sell, Talk}
public class IshaanShop : MonoBehaviour
{
    private ShopState state;

    public MoneyManager moneyManager;

    public TextMeshProUGUI dialogue;
    public TextMeshProUGUI money;
    public TextMeshProUGUI inventorySpace;

    public Image bg;
    public Sprite nightSky;

    public int slotsFilled;

    public void OnEnable()
    {
        GameControllerScript.current.HideChar();
        GameControllerScript.current.StopAllEvents();
        GameControllerScript.current.canPause = false;
        GameControllerScript.current.player.inNotebook = true;

        curselected = 3;
        cursorRect.position = cursorPositions[curselected].position;
        state = ShopState.Menu;

        int index = 0;

        foreach(TextMeshProUGUI text in texts)
        {
            if (index == 0)
                text.color = Color.white;
            else
                text.color = Color.gray;

            index++;
        }

        if (PlayerPrefs.GetFloat("after_dark") == 1)
            bg.sprite = nightSky;

        Speak("Welcome to my shop.\n* Buy some potatoes I guess.");
    }
    
    bool allowMoveStuff = true;

    private void Speak(string text, bool quitAfter = false)
    {
        StopAllCoroutines();
        StartCoroutine(Say(text, quitAfter));

        if (quitAfter)
            allowMoveStuff = false;
    }
    
    private void ClickOption(int option = 0)
    {
        if (state == ShopState.Menu)
        {
            switch(option)
            {
                case 0:
                    OpenBuy();
                    break;
                case 1:
                    Speak("Look kid, i've got nothin' to give you. Im broke as shit.\n* But then again, if you wanted to give it to me for free...\n* I wouldn't complain...");
                    break;
                case 2:
                    if (FindObjectOfType<PlayerStats>().data.ishaanModeUnlocked)
                        Speak("W-what? 'Ishaan Mode'?\nWhat are you talking about?");
                    else if (!FindObjectOfType<PlayerStats>().data.theEnd)
                        Speak("H-how did you get here?\n...\nI love women.");
                    else if (GameControllerScript.current.finaleMode)
                        Speak("WHAT THE HELL ARE YOU DOING HERE?\nYOUR ABOUT TO FUCKING DIE");
                    else if (!GameControllerScript.current.spoopMode)
                        Speak("You dirty fucking hacker.");
                    else if (GameControllerScript.current.debug)
                        Speak("Come on man. Turn off debug mode.\nGo play the game like a REAL man.");
                    else
                        Speak("Been goin' across the country trying to sell my potatoes.\n* Haven't made a sale yet, but i'm confident that I can one day make my mother proud.");
                    break;
                case 3:
                    Speak("Thanks for coming.\n* Although do consider buying potatoes next time.", true);
                    break;
            }
        }
        else if (state == ShopState.Buy && curselected != 3)
        {
            if (slotsFilled < 5)
            {
                if (moneyManager.money >= buyPrice[curselected])
                {
                    Speak("W-wait... are you.. serious?\n* Am i actually making my first sale?\n* T-thank you! I never made a sale before!");
                    GameControllerScript.current.CollectItem(21);
                    GameControllerScript.current.audioDevice.PlayOneShot(GameControllerScript.current.cash);
                    GameControllerScript.current.money.money -= buyPrice[curselected];
                }
                else
                {
                    Speak("Sorry kid, you're too broke to buy this.");
                }
            } else {
                Speak("Sorry kid, you're carrying too much.");
            }
        }
    }

    private void ExitBuy()
    {
        Speak("Look kid, I get they might be a little expensive for you, but I can throw in a bonus!\n* Buy a potato, get a complimentary ISHAAN'S POTATOES sticker!!!!!!!");
        state = ShopState.Menu;

        curselected = 0;

        int index = 0;

        foreach(TextMeshProUGUI text in texts)
        {
            text.text = normOptions[index];

            index++;
        }

        buyInfoMenu.SetActive(false);
    }

    private void OpenBuy()
    {
        Speak("W-wait.. really? My first customer!?");

        state = ShopState.Buy;

        curselected = 0;

        int index = 0;

        foreach(TextMeshProUGUI text in texts)
        {
            text.text = buyOptions[index];

            index++;
        }

        buyInfoMenu.SetActive(true);
    }

    public GameObject buyInfoMenu;
    public TextMeshProUGUI buyInfoText;
    private string[] normOptions = {"Buy", "Sell", "Talk", "Exit"};
    private string[] buyOptions = {"Potato", "Potato", "Moldy Potato", ""};
    private string[] buyInfo = {"It's a potato. Nothing special.", "Yep. Definitely a potato. Nothing special.", "This one's been sitting out for a while. Again, nothing special.", ""};
    private int[] buyPrice = {5, 5, 9999, 0};
    public AudioSource dialogueSounds;
    public AudioClip ishaanTalk;

    private void Quit()
    {
        GameControllerScript.current.ShowChar();
        GameControllerScript.current.canPause = true;
        GameControllerScript.current.player.inNotebook = false;
        allowMoveStuff = true;

        base.gameObject.SetActive(false);
    }

    private void Update()
    {
        int slotsTaken = 0;
        money.text = "$" + moneyManager.money;

        foreach(int item in GameControllerScript.current.item)
        {
            if (item != 0)
                slotsTaken++;
        }
        slotsFilled = slotsTaken;
        inventorySpace.text = slotsTaken + "/5";

        if ((state == ShopState.Menu || state == ShopState.Buy) && allowMoveStuff)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
                Scroll(1);
            else if (Input.GetKeyDown(KeyCode.UpArrow))
                Scroll(-1);
            else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Z))
                ClickOption(curselected);
        }

        if (state == ShopState.Buy)
        {
            buyInfoText.text = buyInfo[curselected] + "\n$" + buyPrice[curselected];

            if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Escape))
            {
                ExitBuy();
            }
        }
    }

    public RectTransform cursorRect;
    public RectTransform[] cursorPositions;
    public TextMeshProUGUI[] texts;

    private int curselected;

    private void Scroll(int add)
    {
        curselected += add;

        if (curselected > texts.Length - 1)
            curselected = 0;
        else if (curselected < 0)
            curselected = texts.Length - 1;
        
        cursorRect.position = cursorPositions[curselected].position;

        int index = 0;

        foreach(TextMeshProUGUI text in texts)
        {
            if (curselected == index)
            {
                text.color = Color.white;
            }
            else
            {
                text.color = Color.gray;
            }

            index++;
        }
    }

    private IEnumerator Say(string text, bool quitAfter)
    {
        dialogue.text = "* ";

        foreach(char chara in text.ToCharArray())
        {
            dialogueSounds.Stop();
            dialogue.text += chara;
            dialogueSounds.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
            dialogueSounds.Play();
            yield return new WaitForSeconds(0.025f);
        }

        if (quitAfter)
        {
            yield return new WaitForSeconds(0.5f);
            Quit();
        }
    }
}

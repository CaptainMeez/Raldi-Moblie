using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class ModifierMenu : MonoBehaviour
{
    [HideInInspector]
    public static Modifier[] modifiers =
    {
        new Modifier("MrBeast's Bad Day", "MrBeast will always think you have his credit card everytime he starts a challenge."), // 0
        new Modifier("Laxitive", "Every item works like a laxitive and makes your poop timer go down."), // 1
        new Modifier("Fuck You Inparticular", "All chipfloke times get doubled, and chipfloke does not catch vanman for kidnapping."), // 2
        new Modifier("I See You", "Raldi can see you while your kidnapped."), // 3
        new Modifier("Make This Quick", "All time related items (eg: monkenator, clock) will have their time cut in half making them activate quicker."), // 4
        new Modifier("Crack Delivery", "All characters move at 2x speed."), // 5
        new Modifier("Back To The Basics", "Item slots reduced to 3"), // 6
        new Modifier("No Remorse", "Raldi does not respect privacy."), // 7
        new Modifier("Hot Headed", "Each notebook will make raldi go faster 2x as much."), // 8
        new Modifier("You Never Know", "Raldi doesnt play the \"Hello!\" sound effect when he sees you."), // 9
        new Modifier("Wheres The Bathroom", "Start the game by needing to poop."), // 10
        new Modifier("Blind", "Player values are hidden, and the indications for the ATM and bathrooms wont show."), // 11
        new Modifier("Destruction", "Breaking windows is now a crime."), // 12
        new Modifier("Passive Income", "Passing Go will grant $1, and 4 corner spots will show up, which you have to cross."), // 13
        new Modifier("After Dark", "Work the night shift at Raldi's Crackhouse."), // 14
        new Modifier("Sniper Beans", "Makes pinto beans not useless"), // 15
        new Modifier("Double Shoot", "Makes Pintobeans shoot a second time"), // 16
        new Modifier("Youtube Detention", "Y o u  c a n n o t  l e a v e .\n - MrBeast"), // 17

        new Modifier("Whos The Drug Now", "You move at 2x speed."), // 18
        new Modifier("Someones Tired", "Raldi doesnt get as angry when you collect a notebook."), // 19
        new Modifier("Item Delivery", "Items respawn after 1 minute."), // 20
        new Modifier("My Roots", "Monkenator lasts 2x as long."), // 21
        new Modifier("Bad Sleep Schedule", "Characters move 2x as slow."), // 22
        new Modifier("Wrong Mode", "Gas Canisters will replace certain items."), // 23
        new Modifier("True Hitman", "Raldi will die for longer when a hitman is hired"), // 24
        new Modifier("Radio Tape", "Tape player effect has a chance of activating at any time randomly."), // 25
        new Modifier("Minimum Wage", "'I'm not paid enough for this...'\nChipfloke will give up if you break another rule."), // 26
        new Modifier("Spilling Wallet", "Mrbeast drops twice as many quarters") // 27
    };

    public Image[] badButtons;
    public Image[] buttons;

    public Image background;
    public Sprite normalBG;
    public Sprite nightBG;

    public TextMeshProUGUI namee;
    public TextMeshProUGUI desc;

    public Image preview;

    public Sprite[] images;

    private void Start()
    {
        int index = 0;
        
        foreach(Modifier bad in modifiers)
        {
            if (!FindObjectOfType<MainMenuScript>().neilmode)
            {
                float finalToggle = PlayerPrefs.GetFloat(ConvertToInternal(bad.name));
                if (finalToggle == 0)
                    badButtons[index].gameObject.SetActive(false);
                else
                    badButtons[index].gameObject.SetActive(true);
            } 
            else 
            {
                PlayerPrefs.SetFloat(ConvertToInternal(bad.name), 0);
            }
            index++;
        }
    }

    public void OnHoverBad(int index)
    {
        namee.text = modifiers[index].name;
        desc.text = modifiers[index].description;

        preview.gameObject.SetActive(true);
        preview.sprite = buttons[index].sprite;
    }

    public void DisableAll()
    {
        int index = 0;

        foreach(Modifier bad in modifiers)
        {
            PlayerPrefs.SetFloat(ConvertToInternal(bad.name), 0);

            badButtons[index].gameObject.SetActive(false);

            index++;
        }
    }

    public void EnableAll()
    {
        int index = 0;

        foreach(Modifier bad in modifiers)
        {
            PlayerPrefs.SetFloat(ConvertToInternal(bad.name), 1);

            badButtons[index].gameObject.SetActive(true);
            
            index++;
        }
    }

    public void ToggleBadModifier(int index)
    {
        bool enabled = PlayerPrefs.GetFloat(ConvertToInternal(modifiers[index].name)) == 1;
        int finalToggle = 0;

        switch(enabled)
        {
            case true:
                finalToggle = 0;
                break;
            case false:
                finalToggle = 1;
                break;
        }

        PlayerPrefs.SetFloat(ConvertToInternal(modifiers[index].name), finalToggle);

        if (finalToggle == 0)
            badButtons[index].gameObject.SetActive(false);
        else
            badButtons[index].gameObject.SetActive(true);
    }

    public static string ConvertToInternal(string text)
    {
        string final = text;

        final = final.ToLower();
        final = final.Replace(" ", "_");
        final = final.Replace("'", "");
        final = final.Replace(",", "");
        final = final.Replace(".", "");

        return final;
    }

    private void Update()
    {
        if (PlayerPrefs.GetFloat("after_dark") == 1)
            background.sprite = nightBG;
        else
            background.sprite = normalBG;
    }
}

public class Modifier
{
    public string name;
    public string description;

    public Modifier(string name, string description)
    {
        this.name = name;
        this.description = description;
    }
}
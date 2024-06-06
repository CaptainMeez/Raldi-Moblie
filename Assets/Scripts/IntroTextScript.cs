using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

using TMPro;

public class IntroTextScript : MonoBehaviour
{
    public TextMeshProUGUI text;
    public MainMenuScript mc;

    string[] neilTexts =
    {
        "Neil Gaming",
        "...",
        "Steamed Hams.",
        "Did you know that I hate memes?",
        "It seems today",
        "That all you see",
        "Is violence and movies",
        "And ### on TV",
        "He's just an average kid, that no one understands",
        "built like a B)"
    };
    
    string[] texts =
    {
        "An awesome bit of text in the awesome game",
        "Endorsed by Baldi's Basics In Funkin' FNF Mod",
        "Games boring, go play Fortnite instead.",
        "VanMan18 on Youtube Shorts",
        "Shoutout to absolutely nobody",
        "This intro text was sponsored by Nord VPN",
        "Better than Baldi's Basics",
        "In loving memory of your mother",
        "what is grass",
        "raldi for president 2024",
        "free robux novirus 2008",
        "ur a flag with no I yk that",
        "You need to poop",
        "Get to the bathroom as soon as possible.",
        "JerrysButt44",
        "BaldyCryin.png",
        "https://discord.gg/8Wx57nYKhG",
        "join my mind craft server at aternos.pee",
        "caldis rackhouse",
        "what in the freaking crap",
        "are your serious right neow bro",
        "Null object reference: Object not set to a reference of an object",
        System.DateTime.Now.ToString(),
        "i really hope this intro text appears in a Rapparep Lol video",
        "hi matpat",
        "nothing to see here",
        "what is the dog doing",
        "there is a hidden door that leads to the crack facility go find it",
        "quirky earthbound inspired indie rpg about anxiety and depression",
        "never hold f then press escape on the you can think pad trust me bro trust me",
        "undertale",
        "Count how many references you find! Go, do it!",
        "You have no friends.",
        "doing your mom",
        "Get out of GameMaker Studio 5",
        "https://youtube.com/shorts/20P7ZXSTBCg?feature=share",
        "Does he know?",
        "mr breast",
        "Last one to leave the circle wins a free item!",
        "Variable Index [0] out of range [0]",
        "i live in a constant state of fear and misery",
        "Consider the following: piss your pants",
        "Hello, Ronezkj15",
        "Get out of Godot Engine.\nI am looking at you, Leather128",
        "Fortnite?",
        "Teris",
        "If anyone offers you drugs\nSay yes\nBecause drugs are expensive\n-shadowleague08#9850",
        "bside collection update",
        "this mill ain't so wind now",
        "I'm about to do something that I should've\ndone a long time ago\n- Ronezkj15 11/11/2022 4:21 PM",
        "THE ONE PIECE IS REAL",
        "User @Ronezkj15 is a light mode user.\nUser @Ronezkj15 is a light mode user.\nUser @Ronezkj15 is a light mode user.\nUser @Ronezkj15 is a light mode user.\n",
        "Arguement Validated\nI hope your having a great day",
        "ultimate rhythm gaming\nprobably",
        "what is HE doing here!",
        "Press escape and F at the same time when leaving the\n think pad, funny idea trust me",
        "BALLS IS DOOR",
        "Days since 3455893th FOV settings fix: 0",
        "might as well be a megaman fangame",
        "Raldi Seed Completionist when",
        "see the joke is you can't move your head up and down",
        "l + ratio \n + you fell off + trump supporter \n + biden supporter + light mode user \n + cheeseburgers in those gloves + who asked + \n among us",
        "I will go apeshit on my keyboard (faint fart on keyboard) - Cuzsie 2022",
        "i just wanted to work on monke mode.",
        "peak baldi modifications",
        "this is so retro",
        "you know who else",
        "insert cryptic message that hints to something later on",
        "lmfao",
        "shoutout to real colson",
        "YOU CAN POOP BEFORE THE INDICATOR\nFOR FUCKS SAKE",
        "does anyone actually read these",
        "we'd like to take a moment\nto announce the",
        "ALL OF OUR FOOD\nKEEPS BLOWING UP!",
        "neil was here"
    };

    void Start()
    {
        if (!mc.neilmode)
            text.text = texts[UnityEngine.Random.Range(0, texts.Length)];
        else
            text.text = neilTexts[UnityEngine.Random.Range(0, neilTexts.Length)];
        
        if (PlayerPrefs.GetFloat("PussiedOut") == 1)
        {
            PlayerPrefs.SetFloat("PussiedOut", 0);
            text.text = "Pussy.";
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class PoopText : MonoBehaviour
{
    public PlayerScript player;
    public TextMeshProUGUI text;
    public bool playerHasToPoop;

    void Start()
    {
        text = base.GetComponent<TextMeshProUGUI>();
    }

    void FixedUpdate()
    {
		text.text = player.hasToPoop && !player.diarTimeStarted ? "You need to poop!\nGet to the bathroom as soon as possible!" : "";
    }
}

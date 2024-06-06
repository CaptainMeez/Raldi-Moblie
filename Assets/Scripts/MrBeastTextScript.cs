using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class MrBeastTextScript : MonoBehaviour
{
    public MrBeastScript beast;
    public TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (beast.inChallenge)
            text.text = "Last to leave this circle wins a free item!\n" + Mathf.RoundToInt(beast.challengeTime).ToString() + " seconds remain.";
        else
            text.text = "";
    }
}

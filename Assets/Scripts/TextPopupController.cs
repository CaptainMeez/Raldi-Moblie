using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class TextPopupController : MonoBehaviour
{
    public TextMeshProUGUI jailTime;
    public TextMeshProUGUI kidnap;
    public TextMeshProUGUI poop;
    public TextMeshProUGUI mrbeast;

    public DoorScript detdoor;
    public PlaytimeScript vanman;
    public PlayerScript player;
    public MrBeastScript beast;

    private void ConditionCheck()
    {
        if (this.detdoor.lockTime > 1f)
		{
            jailTime.gameObject.SetActive(true);
			jailTime.text = "You got jail time! \n" + Calculations.GetFormattedTimeString(detdoor.lockTime) + " remain!";
		}
		else
		{
            jailTime.gameObject.SetActive(false);
			jailTime.text = string.Empty;
		}

        if (this.vanman.kidnapTime > 1f)
		{
            kidnap.gameObject.SetActive(true);
		    kidnap.text = "You have been Kidnapped! \n" + Calculations.GetFormattedTimeString(vanman.kidnapTime) + " remain!";
		}
		else
		{
            kidnap.gameObject.SetActive(false);
			kidnap.text = string.Empty;
		}

        if (!player.hasToPoop || player.diarTimeStarted)
        {
            poop.gameObject.SetActive(false);
            poop.text = "";
        }
        else if (player.hasToPoop && !player.diarTimeStarted)
        {
            poop.gameObject.SetActive(true);
            poop.text = "You need to poop!\nGet to the bathroom as soon as possible!";
        }

        if (beast.inChallenge)
        {
            mrbeast.gameObject.SetActive(true);
            mrbeast.text = "Last to leave this circle wins a free item!\n" + Calculations.GetFormattedTimeString(beast.challengeTime) + " remain!";
        }
        else
        {
            mrbeast.gameObject.SetActive(false);
            mrbeast.text = "";
        }   
    }

    void Update()
    {
        ConditionCheck();
    }
}

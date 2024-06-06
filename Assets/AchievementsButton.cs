using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GameJolt;

public class AchievementsButton : MonoBehaviour
{
    public void OpenAchievements()
    {
        GameJolt.UI.GameJoltUI.Instance.ShowTrophies();
    }
}

using UnityEngine;
using GameJolt.UI;

public class AchievementMenu : MonoBehaviour
{
    public void Open() => GameJoltUI.Instance.ShowTrophies();
}

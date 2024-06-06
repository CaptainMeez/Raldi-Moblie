using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoadTrophyGetter : MonoBehaviour
{
    public int trophy = 184398;
    public GameJolt.API.GameJoltAPI api;

    private void Awake()
    {
        if (GameJolt.API.GameJoltAPI.Instance == null)
            api.AutoLoginEvent.AddListener(Unlock);
        else
            GameJolt.API.Trophies.TryUnlock(trophy);
    }

    public void Unlock(GameJolt.API.AutoLoginResult result)
    {
        GameJolt.API.Trophies.TryUnlock(trophy);
    }
}
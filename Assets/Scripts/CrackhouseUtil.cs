using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackhouseUtil
{
    public static string GetRandomString(int ammo)
    {
        string[] randomCharacters = {"!", "@", "#", "$", "%", "^", "&", "*", "(", ")", ";", ":", "{", "}"};
        string toReturn = "";

        for(int i = 0; i < ammo; i++)
        {
            toReturn += randomCharacters[UnityEngine.Random.Range(0, randomCharacters.Length)];
        }

        return toReturn;
    }
}

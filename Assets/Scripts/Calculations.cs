
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calculations
{
    public static string GetFormattedTime(float seconds)
    {
        string final = "";

        if ((Mathf.Floor(seconds / 60) % 60) > 60f)
        {
            final += Mathf.Floor(seconds / 3600);
            final += ":";
        }

        if (seconds > 60f)
        {
            final += Mathf.Floor(seconds / 60) % 60;
            final += ":";
        }
        else
        {
            final += "0:";
        }

        if (Mathf.Floor(seconds % 60) < 10f)
            final += "0";
            
        final += Mathf.Floor(seconds % 60);
        
        return final;
    }

    public static string GetFormattedTimeString(float seconds)
    {
        string final = "";

        if ((Mathf.Floor(seconds / 60) % 60) > 60f)
        {
            final += Mathf.Floor(seconds / 3600);
            final += " hours, ";
        }

        if (seconds > 60f)
        {
            final += Mathf.Floor(seconds / 60) % 60;
            final += " minutes, ";
        }

        final += Mathf.Floor(seconds % 60);
        final += " seconds";
        
        return final;
    }

    public static float GetMinutes(float seconds)
    {
        float minutes;

        if (seconds > 60f)
            minutes = Mathf.Floor(seconds / 60) % 60;
        else
            minutes = 0;

        return minutes;
    }

    public static string ToRomanNumeral(int number)
    {
        int decrease = number + 1;
        string[] letters = new string[9]{"I","IV","V","IX","X","XL","L","XC","C"};
        int[] values = new int[9]{1,4,5,9,10,40,50,90,100};
        int tests = 0;
        string output = "";
        while (tests < 1000 || decrease == 0)
        {
            for (int i = 8; i >= 0; i--)
            {
                if (decrease > values[i]) // 23 = XXIII
                {
                    output += letters[i];
                    decrease -= values[i];
                    break;
                }
            }
            tests++;
        }
        return output;
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoulsonEngine.Text
{
    public class StringFormat
    {
        public static string SecondsToTime(float time)
        {
            float minutes = Mathf.Floor(time / 60);
     	    float seconds = time % 60;

            return (minutes == 0 ? "" : minutes + ":") + System.Math.Round(seconds);
        }

        // Correctly format Coulson strings (Replaces $KEY$ with certain varriables)
        public static string FormatString(string text)
        {
            string returnString = text;

            if (GameObject.FindObjectOfType<GameControllerScript>() != null)
                returnString = returnString.Replace("$neilKeysLeft$", (5 - GameObject.FindObjectOfType<GameControllerScript>().neilKeysCollected).ToString());

            returnString = returnString.Replace("$WindowsName$", GameObject.FindObjectOfType<PlayerStats>().data.name);

            return returnString;
        }

        public static string CapitalizeSentence(string text)
        {
            string finalString = "";
            string[] words = text.Split(' ');

            foreach (string word in words)
            {
                System.Text.StringBuilder strBuilder = new System.Text.StringBuilder(word);
                char input = strBuilder[0];
                char set = char.ToUpper(input);
                strBuilder[0] = set;
                finalString = strBuilder.ToString();
            }

            return finalString;
        }

        public static string[] GetWordsInSentence(string sentence)
        {
            return sentence.Split(' ');
        }
    }
}
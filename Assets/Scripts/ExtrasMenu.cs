using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using TMPro;

public class ExtrasMenu : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descText;
    public string curSelectedExtra;
    public GameObject loading;
    public UnityEngine.UI.Button button;

    public string[] names =
    {
        "Poster Room"
    };

    public string[] descriptions = 
    {
        "Check out community-submitted posters in the poster room!\nAll posters were submitted to Raldi's Crackhouse twitter."
    };
    
    private void OnEnable()
    {
        button.interactable = false;
        curSelectedExtra = "";
    }
    
    public void SelectExtra(string extra)
    {
        string[] extrasArray = extra.Split(':');
        curSelectedExtra = extrasArray[0];
        button.interactable = true;

        nameText.text = names[int.Parse(extrasArray[1])];
        descText.text = descriptions[int.Parse(extrasArray[1])];
    }

    public void Play()
    {
        if (curSelectedExtra != "")
        {
            loading.SetActive(true);
            switch(curSelectedExtra.ToLower())
            {
                case "poster":
                    SceneManager.LoadSceneAsync("PosterRoom");
                    break;
            } 
        }
    } 
}

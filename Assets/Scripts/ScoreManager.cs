using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public AudioSource missSFX;
    public TMPro.TextMeshPro scoreText;
    static int comboScore;
    static int misses;
    static int totalNotes;
    static int hitNotes;

    void Start()
    {
        Instance = this;
        comboScore = 0;
        misses = 0;
        totalNotes = 0;
        hitNotes = 0;
    }
    public static void Hit()
    {
        comboScore += 1;
        totalNotes += 1;
        hitNotes += 1;
        SongManager.Instance.health += 10;
    }
    public static void Miss()
    {
        misses += 1;
        totalNotes += 1;
        SongManager.Instance.health -= 10;
        Instance.missSFX.Play();    
    }
    private void Update()
    {
        scoreText.text = "Score: " + comboScore.ToString() + " | Misses: " + misses + " | Accuracy: " + CalculatePercentage() + "%";
    }

    private string CalculatePercentage()
    {
        float percent;

        if (totalNotes != 0 || hitNotes != 0)
            percent = (hitNotes / totalNotes) * 100;
        else
            percent = 100;
        
        return percent.ToString() + "% (" + CalculateRating(percent) + ")";
    }

    private string CalculateRating(float percent)
    {
        if (percent == 100)
            return "A+";
        else if (percent > 90)
            return "A";
        else if (percent > 86)
            return "B+";
        else if (percent > 80)
            return "B";
        else if (percent > 73)
            return "C+";
        else if (percent > 60)
            return "C";
        else
            return "D";
    }
}

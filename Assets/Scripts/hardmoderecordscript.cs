using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class hardmoderecordscript : MonoBehaviour
{
    // Start is called before the first frame update
    int rank = 0;
    int score = 0;
    public TextMeshProUGUI text;
	public int[] scoretargets = new int[5];
    private string[] ranks = new string[6]{"D","C","B","A","S","P"};
    void Start()
    {
        score = FindObjectOfType<PlayerStats>().data.recordScore;

        if (!FindObjectOfType<PlayerStats>().data.beatHardMode)
            gameObject.SetActive(false);
            
        CheckRank();
        text.text = "Rank: " + ranks[rank] + ", Score: " + score;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void CheckRank()
    {
        int carry = 0;
		for (int i = 0; i < 5; i++)
		{
			if (score >= scoretargets[i])
			{
				carry += 1;
			}
		}
		rank = carry;
    }
}

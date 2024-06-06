using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PTscoredisplayScript : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI scoretext;
    public TextMeshProUGUI scoretextbg;
    public Image RankEmblem;
    public GameControllerScript gc;
    
    public Sprite[] emblems = new Sprite[6];
    void Start()
    {
        gc = FindObjectOfType<GameControllerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        RankEmblem.sprite = emblems[gc.rank];
        scoretext.text = "" + gc.score;
        scoretextbg.text = "" + gc.score;
    }
}
/* 		scoretext.text = "" + score;
		ranktext.text = "" + ranks[rank].ToUpper(); */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class BonusGame : MonoBehaviour
{
    public GameControllerScript gc;

    public BonusQNA[] questions = 
    {
        new BonusQNA("Which of the following is a swear word?", new string[] {"Lasagna", "Sandwhich", "Fuck", "Badonkadonk"}, 2)
    };

    int curQuestion;
    
    public Text[] answerTexts;
    public TextMeshProUGUI inputText;
    public TextMeshProUGUI text;
    public GameObject incorrectLabel;
    public GameObject correctLabel;
    bool canInteract = true;
    void Start()
    {
        curQuestion = UnityEngine.Random.Range(0, questions.Length);

        text.text = questions[curQuestion].question;
    }

    public void AcceptAnswer(int answer)
    {
        if (inputText.text.Contains(questions[curQuestion].answers[curQuestion]) && canInteract)
        {
            IEnumerator Leave()
            {
                canInteract = false;

                incorrectLabel.SetActive(false);
                correctLabel.SetActive(true);

                yield return new WaitForSeconds(2f);

                gc.Destroy(this.gameObject);
            }

            StartCoroutine(Leave());
        }
        else if (canInteract)
        {
            incorrectLabel.SetActive(true);
        }
    }
}
public class BonusQNA
{
    public string question;
    public string[] answers;

    public BonusQNA(string question, string[] answers, int correctAnswer)
    {
        this.question = question;
        this.answers = answers;
    }
}
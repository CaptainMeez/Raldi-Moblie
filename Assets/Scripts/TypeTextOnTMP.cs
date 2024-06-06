using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class TypeTextOnTMP : MonoBehaviour
{
    [TextArea(3, 50)]
    public string textToType;

    public TextMeshPro text;

    void Awake()
    {
        text.text = "";

        IEnumerator TypeText()
        {
            foreach(char character in textToType.ToCharArray())
            {
                text.text += character;
                yield return new WaitForSeconds(0.006f);
            }
        }

        StartCoroutine(TypeText());
    }
}

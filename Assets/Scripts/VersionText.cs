using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class VersionText : MonoBehaviour
{
    public string extraMessage = "";
    // Start is called before the first frame update
    void Start()
    {
        string editorText = "";

        if (Application.isEditor)
            editorText = "(Editor)";

        TextMeshProUGUI text = base.GetComponent<TextMeshProUGUI>();
        text.text = "RaldiCrackhouse " + Application.version + "" + extraMessage + " " + editorText;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using TMPro;

public class AccurateBaldiPos : MonoBehaviour
{    
    public TextMeshProUGUI text;
    
    private void Update() => text.text = Mathf.RoundToInt(Vector3.Distance(GameControllerScript.current.player.transform.position, GameControllerScript.current.baldi.transform.position)).ToString() + " km";
}

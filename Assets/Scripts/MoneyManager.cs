using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class MoneyManager : MonoBehaviour
{
    public float money;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI bgmoneyText;

    void Update()
    {
        moneyText.text = "$" + money;
        bgmoneyText.text = "$" + money;
    }
}
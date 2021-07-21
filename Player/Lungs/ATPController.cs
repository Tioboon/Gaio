using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ATPController : MonoBehaviour
{
    public TextMeshProUGUI textBox;

    public void ActualizeAtpValue(float quantity)
    {
        int newText = (int) quantity;
        textBox.text = newText.ToString();
    }
}

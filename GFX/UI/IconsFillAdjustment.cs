using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconsFillAdjustment : MonoBehaviour
{
    private Image image;
    public VariableName variableName;
    public PlayerController playerController;
    private void OnEnable()
    {
        image = GetComponent<Image>();
        image.fillAmount = playerController.player.variables[(int) variableName].variableValue/100;
    }
}

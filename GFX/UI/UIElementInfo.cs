using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIElementInfo : MonoBehaviour
{
    public UIElement uiElementName;
    public RectTransform rTrans;
    public Image signImage;
    public SignOppacityRegToText signAlphaRegText;

    private void Start()
    {
        signImage = GetComponent<Image>();
        rTrans = GetComponent<RectTransform>();
    }

    private void OnDisable()
    {
        switch (gameObject.activeSelf)
        {
            case true:
                PlayerPrefs.SetInt(uiElementName.ToString(), 1);
                break;
            case false:
                PlayerPrefs.SetInt(uiElementName.ToString(), 0);
                break;
        }
    }
}

public enum UIElement
{
    None,
    InBreath,
    OutBreath,
    AtpSign,
    StomachSign,
    StomachIcon,
    LustSign,
    LustIcon,
    SleepSign,
    SleepIcon,
    ExcrementSign,
    ExcrementIcon,
    LungsSign,
    LungsOxygenIcon,
    LungsCarbonIcon,
    ChoicesSign,
    Master,
}
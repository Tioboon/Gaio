using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISaveSheet : MonoBehaviour
{
    public List<UIElementInfo> uiElements;

    private void OnEnable()
    {
        foreach (var elementInfo in uiElements)
        {
            var isActive = 0;
            switch (elementInfo.name == "EventController")
            {
                case true:
                    isActive = PlayerPrefs.GetInt(elementInfo.uiElementName.ToString(), 1);
                    break;
                case false:
                    isActive = PlayerPrefs.GetInt(elementInfo.uiElementName.ToString(), 0);
                    break;
            }
            switch (isActive)
            {
                case 1:
                    elementInfo.gameObject.SetActive(true);
                    break;
                case 0: 
                    elementInfo.gameObject.SetActive(false); 
                    break;
            }
        }
    }
}

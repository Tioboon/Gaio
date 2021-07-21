using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIVariablesController : MonoBehaviour
{
    public Image sleepnessImage;
    public Image loveImage;
    public Image excrementImage;
    public Image hungryImage;
    
    
    private PlayerController playerController;

    private void Awake()
    {
        playerController = GameObject.Find("GameController").GetComponent<PlayerController>();
    }

    public void AttIcons(float sleep, float love, float excrement, float hungry, float maxVariablesN)
    {
        var onePercent = 1 / maxVariablesN;
        var sleepQnt = sleep * onePercent;
        var excrementQnt = excrement * onePercent;
        var loveQnt = love * onePercent;
        var hungryQnt = hungry * onePercent;
        if(playerController.save.sleepIconActive)
        {
            sleepnessImage.fillAmount = sleepQnt;
        }
        if (playerController.save.lustIconActive)
        {
            loveImage.fillAmount = loveQnt;
        }
        if(playerController.save.stomachIconActive)
        {
            hungryImage.fillAmount = hungryQnt;
        }
        if (playerController.save.excrementIconActive)
        {
            excrementImage.fillAmount = excrementQnt;
        }
    }
}

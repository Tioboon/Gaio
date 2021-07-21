using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectController : EffectLibrary
{
    //Signs
    public List<UIElementInfo> allSigns;
    public void ReproduceEffect(DotGfxs dotGfxs)
    {
        foreach (GfxInfo gfxInfo in dotGfxs.gfxDotList)
        {
            StartCoroutine(CoroutineReproduceEffect(gfxInfo));
        }
    }

    //Reproducers
    private IEnumerator CoroutineReproduceEffect(GfxInfo gfxInfo)
    {
        yield return new WaitForSeconds(gfxInfo.timeToReproduce);
        switch (gfxInfo.gfx)
        {
            case Gfx.MakeSignVisible:
                MakeSignVisible(gfxInfo);
                break;
            case Gfx.MakeSignInvisible:
                MakeSignInvisible(gfxInfo);
                break;
            case Gfx.MakeImageRumble:
                MakeImageShake(gfxInfo);
                break;
            case Gfx.MakeImageBlink:
                MakeImageBlink(gfxInfo);
                break;
            case Gfx.MakeImageBreath:
                MakeImageBreath(gfxInfo);
                break;
            default: break;
        }
    }
    
    private void MakeSignVisible(GfxInfo gfxInfo)
    {
        UIElementInfo uiElementInfo = CheckTargetImage(gfxInfo.targetUIElement);
        uiElementInfo.gameObject.SetActive(true);
        if (gfxInfo.targetUIElement == UIElement.AtpSign)
        {
            uiElementInfo.signAlphaRegText.StartAdjusting(gfxInfo.reproductionFPS);
        }
        FadeInImage(uiElementInfo.signImage, gfxInfo.timeOfReproduction, gfxInfo.reproductionFPS);
    }

    private void MakeSignInvisible(GfxInfo gfxInfo)
    {
        UIElementInfo uiElementInfo = CheckTargetImage(gfxInfo.targetUIElement);
        uiElementInfo.gameObject.SetActive(false);
        if (gfxInfo.targetUIElement == UIElement.AtpSign)
        {
            uiElementInfo.signAlphaRegText.StartAdjusting(gfxInfo.reproductionFPS);
        }
        FadeOutImage(uiElementInfo.signImage, gfxInfo.timeOfReproduction, gfxInfo.reproductionFPS);
    }

    private void MakeImageShake(GfxInfo gfxInfo)
    {
        UIElementInfo uiElementInfo = CheckTargetImage(gfxInfo.targetUIElement);
        ShakeImage(uiElementInfo.rTrans, gfxInfo.intensity, gfxInfo.reproductionFPS, gfxInfo.timeOfReproduction);
    }

    private void MakeImageBlink(GfxInfo gfxInfo)
    {
        UIElementInfo uiElementInfo = CheckTargetImage(gfxInfo.targetUIElement);
        var initialColor = uiElementInfo.signImage.color;
        BlinkImage(uiElementInfo.signImage, gfxInfo.color, gfxInfo.reproductionFPS, gfxInfo.timeOfReproduction, initialColor);
    }

    private void MakeImageBreath(GfxInfo gfxInfo)
    {
        UIElementInfo uiElementInfo = CheckTargetImage(gfxInfo.targetUIElement);
        BreathImage(uiElementInfo.rTrans, gfxInfo.intensity, gfxInfo.timesThatRepeat, gfxInfo.reproductionFPS, gfxInfo.timeOfReproduction);
    }

    //Tools
    private UIElementInfo CheckTargetImage(UIElement targetUIElement)
    {
        UIElementInfo targetImage = null;
        foreach (var sign in allSigns)
        {
            if (sign.uiElementName == targetUIElement)
            {
                targetImage = sign;
                break;
            }
        }

        return targetImage;
    }
}

[Serializable]
public enum Gfx
{
    None,
    MakeSignVisible,
    MakeSignInvisible,
    MakeImageRumble,
    MakeImageBlink,
    MakeImageBreath,
}

[Serializable]
public class GfxInfo
{
    public Gfx gfx;
    public UIElement targetUIElement;
    public float intensity;
    public Color color;
    public float timeToReproduce;
    public float timeOfReproduction;
    public int reproductionFPS;
    public int timesThatRepeat;
}

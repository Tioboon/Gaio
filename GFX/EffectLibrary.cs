using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectLibrary : MonoBehaviour
{
    //Fade In & Out
    public void FadeInImage(Image image, float timeToFullOpacity, int fps)
    {
        Color newColor = new Color(image.color.r, image.color.g, image.color.b, 0);
        image.color = newColor;
        StartCoroutine(FadeImageCoroutine(image, timeToFullOpacity, fps, true));
    }
    
    public void FadeOutImage(Image image, float timeToFullOpacity, int fps)
    {
        Color newColor = new Color(image.color.r, image.color.g, image.color.b, 255);
        image.color = newColor;
        StartCoroutine(FadeImageCoroutine(image, timeToFullOpacity, fps, false));
    }

    private IEnumerator FadeImageCoroutine(Image image, float timeToFullOpacity, int fps, bool inIsTrue)
    {
        yield return new WaitForSeconds(timeToFullOpacity / fps);
        Color newImageAlpha = image.color;
        float alphaVariation = 1 / (float)fps;
        float newAlpha = 0;
        if (inIsTrue)
        {
            newAlpha = newImageAlpha.a + alphaVariation;
        }
        else
        {
            newAlpha = newImageAlpha.a - alphaVariation;
        }
        newImageAlpha =
            new Color(image.color.r, image.color.g, image.color.b, newAlpha);
        image.color = newImageAlpha;
        if (newAlpha < 1 && newAlpha > 0)
        {
            StartCoroutine(FadeImageCoroutine(image, timeToFullOpacity, fps, inIsTrue));
        }
    }

    //Image shake
    public void ShakeImage(RectTransform imageTransform, float intensity, int fps, float timeOfReproduction)
    {
        List<Vector2> minMaxList = new List<Vector2>();
        minMaxList.Add(imageTransform.anchorMin);
        minMaxList.Add(imageTransform.anchorMax);
        StartCoroutine(ShakeImageCoroutine(imageTransform, intensity, fps, timeOfReproduction, 0,
            0, minMaxList));
    }

    private IEnumerator ShakeImageCoroutine(RectTransform imageTransform, float intensity, int fps, float timeOfReproduction, int sideThatIsGoing, float timeCounter, List<Vector2> minMaxInitialAnchors)
    {
        switch (sideThatIsGoing)
        {
            case 0:
                imageTransform.anchorMax += new Vector2(+intensity, 0);
                imageTransform.anchorMin += new Vector2(+intensity, 0);
                imageTransform.offsetMin = Vector2.zero;
                imageTransform.offsetMax = Vector2.zero;
                sideThatIsGoing += 1;
                break;
            case 1:
                imageTransform.anchorMax += new Vector2(-intensity, -intensity);
                imageTransform.anchorMin += new Vector2(-intensity, -intensity);
                imageTransform.offsetMin = Vector2.zero;
                imageTransform.offsetMax = Vector2.zero;
                sideThatIsGoing += 1;
                break;
            case 2:
                imageTransform.anchorMax += new Vector2(+intensity, 0);
                imageTransform.anchorMin += new Vector2(+intensity, 0);
                imageTransform.offsetMin = Vector2.zero;
                imageTransform.offsetMax = Vector2.zero;
                sideThatIsGoing += 1;
                break;
            case 3:
                imageTransform.anchorMax += new Vector2(-intensity, +intensity);
                imageTransform.anchorMin += new Vector2(-intensity, +intensity);
                imageTransform.offsetMin = Vector2.zero;
                imageTransform.offsetMax = Vector2.zero;
                sideThatIsGoing = 0;
                break;
        }
        var secondsToWait = timeOfReproduction / fps;
        yield return new WaitForSeconds(secondsToWait);
        timeCounter += secondsToWait;
        if (timeCounter < timeOfReproduction)
        {
            StartCoroutine(ShakeImageCoroutine(imageTransform, intensity, fps, timeOfReproduction, sideThatIsGoing,
                timeCounter, minMaxInitialAnchors));
        }
        else
        {
            imageTransform.anchorMin = minMaxInitialAnchors[0];
            imageTransform.anchorMax = minMaxInitialAnchors[1];
            imageTransform.offsetMin = Vector2.zero;
            imageTransform.offsetMax = Vector2.zero;
        }
    }

    
    //Blink image with color
    public void BlinkImage(Image image, Color color, int fps, float timeOfReproduction, Color initialColor)
    {
        StartCoroutine(BlinkImageCoroutine(image, color, fps, timeOfReproduction, initialColor, 0, false));
    }

    private IEnumerator BlinkImageCoroutine(Image image, Color color, int fps, float timeOfReproduction, 
        Color initialColor, float progress, bool invert)
    {
        float progressPerCorout = 1f / fps;
        if(!invert)
        {
            progress += progressPerCorout;
            image.color = Color.Lerp(initialColor, color, progress);
        }
        else
        {
            progress += progressPerCorout;
            image.color = Color.Lerp(color, initialColor, progress);
        }
        float timeToWait = timeOfReproduction / fps;
        yield return new WaitForSeconds(timeToWait);
        if (!invert)
        {
            if (progress < 1)
            {
                StartCoroutine(BlinkImageCoroutine(image, color, fps, timeOfReproduction, initialColor, progress, invert));
            }
            else
            {
                StartCoroutine(BlinkImageCoroutine(image, color, fps, timeOfReproduction, initialColor, 0, true));
            }
        }
        else
        {
            if (progress > 0)
            {
                StartCoroutine(BlinkImageCoroutine(image, color, fps, timeOfReproduction, initialColor, progress, invert));
            }
        }
    }
    
    //Breath image
    public void BreathImage(RectTransform rectTransform, float intensity, int timesThatExpands, int fps, float timeOfReproduction)
    {
        StartCoroutine(BreathImageCoroutine(rectTransform, intensity, timesThatExpands, fps, timeOfReproduction, 0,
            false, rectTransform.anchorMax, rectTransform.anchorMin));
    }

    private IEnumerator BreathImageCoroutine(RectTransform rectTransform, float intensity, int times, int fps, float timeOfReproduction, float timeCounter, bool shrinking, Vector2 initialMaxAnc, Vector2 initialMinAnc)
    {
        var intensityForFrame = intensity * times / fps;
        switch (shrinking)
        {
            case true:
                rectTransform.anchorMax -= new Vector2(intensityForFrame, intensityForFrame);
                rectTransform.anchorMin += new Vector2(intensityForFrame, intensityForFrame);
                break;
            case false:
                rectTransform.anchorMax += new Vector2(intensityForFrame, intensityForFrame);
                rectTransform.anchorMin -= new Vector2(intensityForFrame, intensityForFrame);
                break;
        }
        //Recursividade
        float secondsToWait = timeOfReproduction / fps;
        yield return new WaitForSeconds(secondsToWait);
        timeCounter += secondsToWait;
        if (timeOfReproduction > timeCounter)
        {
            var maxDistortion = initialMaxAnc + new Vector2(intensity, intensity);
            var minDistortion = initialMaxAnc - new Vector2(intensity, intensity);
            switch (shrinking)
            {
                case true:
                    StartCoroutine(rectTransform.anchorMax.x <= minDistortion.x
                        ? BreathImageCoroutine(rectTransform, intensity, times, fps, timeOfReproduction, timeCounter,
                            false, initialMaxAnc, initialMinAnc)
                        : BreathImageCoroutine(rectTransform, intensity, times, fps, timeOfReproduction, timeCounter,
                            shrinking, initialMaxAnc, initialMinAnc));
                    break;
                case false:
                    StartCoroutine(rectTransform.anchorMax.x >= maxDistortion.x
                        ? BreathImageCoroutine(rectTransform, intensity, times, fps, timeOfReproduction, timeCounter,
                            true, initialMaxAnc, initialMinAnc)
                        : BreathImageCoroutine(rectTransform, intensity, times, fps, timeOfReproduction, timeCounter,
                            shrinking, initialMaxAnc, initialMinAnc));
                    break;
            }
        }
        else
        {
            rectTransform.anchorMax = initialMaxAnc;
            rectTransform.anchorMin = initialMinAnc;
        }
    }
}

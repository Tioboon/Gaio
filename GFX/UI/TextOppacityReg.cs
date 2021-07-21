using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TextOppacityReg : MonoBehaviour
{
    private TextMeshProUGUI textMesh;
    private float actualAlpha;
    private Image fatherImage;

    private void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        actualAlpha = textMesh.alpha;
        fatherImage = GetComponent<Image>();
    }

    public void StartAdjust(float fps)
    {
        StartCoroutine(AdjustAlpha(fps));
    }

    public float ActualAlpha
    {
        get {return fatherImage.color.a;}
        set
        {
            if(actualAlpha == value) return;
            actualAlpha = value;
        }
    }

    private IEnumerator AdjustAlpha(float fps)
    {
        yield return new WaitForSeconds(fps);
        textMesh.alpha = ActualAlpha;
        StartCoroutine(AdjustAlpha(fps));
    }
}

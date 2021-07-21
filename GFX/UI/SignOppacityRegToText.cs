using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignOppacityRegToText : MonoBehaviour
{
    public List<TextOppacityReg> texts;

    public void StartAdjusting(float fps)
    {
        foreach (TextOppacityReg textOppacityReg in texts)
        {
            textOppacityReg.StartAdjust(fps);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LungsController : MonoBehaviour
{
    public Image gas;
    public Image oxygen;

    public void AttGasStats(float oxygenQnt, float gasQnt, float gasMax)
    {
        var onePercent = 1 / gasMax;
        var oxFill = onePercent * oxygenQnt;
        var gasFill = (oxygenQnt + gasQnt) * onePercent;
        gas.fillAmount = gasFill;
        oxygen.fillAmount = oxFill;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "new Player", menuName = "Player")]
public class Player : ScriptableObject
{
    public List<DotVariables> variables;
    public bool inBand;
    public bool sleeping;
}
public enum VariableName
{
    atp = 0,
    oxygen = 1,
    gases = 2,
    excrement = 3,
    hungry = 4,
    slepiness = 5,
    lust = 6,
    hp = 7,
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetSaveVars : MonoBehaviour
{
    public bool resetVars;

    private void Awake()
    {
        switch (resetVars)
        {
            case true:
                PlayerPrefs.DeleteAll();
                break;
        }
    }
}

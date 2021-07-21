using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "New Event", menuName = "Event")]
public class Event : ScriptableObject
{
    //Name
    public string eventName;
    //ATP Cost
    public int eventCost;
    //Time Cost
    public int timeCost;
    //Final text that will show on log.
    public string finalText;
    //Font size.
    public int textSize;
    //True if the player can't choose which event will came after.
    public bool randomEvent;
    //Choices that appears after text
    public List<Event> choices;
    //Clips that play in "."
    public List<DotClipsList> dotClipsInfo;
    //Variable changes that happens in "."
    public List<DotVarChange> dotEventVarChange;
    //Image that appears in "."
    public List<DotImage> dotImages;
    //Game Feel Effects in "."
    public List<DotGfxs> dotGfxList;
    //Reset things that player did in that event
    public List<Event> eventsToReset;
    //Change text color
    public DollarColor dollarColor;

    //if is idle event
    public bool idle;
    //Time after the event finish to write the text to change to idle
    public float timeToChangeEvent;
    
    //Stock the actions that player make in this scene
    public List<Event> eventsDoneInThisScene;
}

//The changes in player variables of one "."
[Serializable]
public class DotVarChange
{
    public ChangeBandVar changeBandVar;
    public ChangeSleepingVar changeSleepingVar;
    public List<DotVariables> increaseVariables;
    
    [Serializable]
    public struct ChangeBandVar
    {
        public bool boolChange;
        public bool value;
    }
    
    [Serializable]
    public struct ChangeSleepingVar
    {
        public bool boolChange;
        public bool value;
    }
}

//The image that shows in some "."
[Serializable]
public class DotImage
{
    public bool changesImage;
    public TimedImages backGround;
    public Sprite layerOne;
    public Sprite layerTwo;
    public Sprite layerThree;
}

[Serializable]
public class DotVariables
{
    public VariableName variableName;
    public float variableValue;
    public float timeToChangeValue;
}

[Serializable] 
public class DotGfxs
{
    public List<GfxInfo> gfxDotList;
}

[Serializable]
public class DollarColor
{
    public List<Color32> colorsPerDollarSign;
}

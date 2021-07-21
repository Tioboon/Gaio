using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class EventController : MonoBehaviour
{
    //Event that are reproducing
    public Event actualEvent;
    //Text that are showing now
    private string actualText = "";
    //If is writing, is the char index
    private int index = 0;

    //Time to draw chars on screen
    public float letterTimeToDraw;
    public float spaceTimeToDraw;
    public float commaPauseTime;
    public float dotPauseTime;

    //Where the text is wrote
    private TextMeshProUGUI logTextBox;
    //Controls the sounds
    private SoundController soundController;
    //Controls the effects
    private EffectController effectController;
    //Controls the choices
    private ChoicesController choicesController;
    //Controls player
    public PlayerController playerController;
    //Controls Images
    private GraphController graphController;

    //Count how many dots have passed
    private int dotCount;
    private int dollarCount;
    private bool dollarActive;
    private List<int> listCharsColor = new List<int>();
    
    //Idle event infos
    public IdleInfos idleInfos;
    //Idle Choices
    public List<IdleChoicesPackage> idleEventsList;

    private void Start()
    {
        GetReferences();
        ChangeEvent(actualEvent);
    }

    private void GetReferences()
    {
        logTextBox = transform.Find("Log").GetComponent<TextMeshProUGUI>();
        soundController = transform.Find("AudioController").GetComponent<SoundController>();
        choicesController = transform.Find("ChoicesController").GetComponent<ChoicesController>();
        playerController = GameObject.Find("GameController").GetComponent<PlayerController>();
        graphController = transform.Find("GraphController").GetComponent<GraphController>();
        effectController = GetComponent<EffectController>();
    }

    public void ChangeEvent(Event newEvent)
    {
        actualEvent = newEvent;
        soundController.StopSoundsEventChange(newEvent.dotClipsInfo[0].clips);
        choicesController.StopDisplaying();
        ResetVars();
        logTextBox.fontSize = newEvent.textSize;
        StartCoroutine(ReproduceText());
    }

    private void ResetVars()
    {
        actualText = "";
        index = 0;
        dotCount = 0;
    }
    
    // ReSharper disable Unity.PerformanceAnalysis
    private IEnumerator ReproduceText()
    {
        //if not readied all letters
        if (index < actualEvent.finalText.Length)
        {
            //get one letter
            char letter = actualEvent.finalText[index];
            //Add the letter in the text
            //All text has to init with a "." that doesn't are showed.
            if(dotCount != 0 && letter != '$')
            {
                actualText += letter;
            }
            //Actualize on screen
            logTextBox.text = actualText;

            switch (dollarActive)
            {
                case true:
                    listCharsColor.Add(actualText.Length);
                    break;
            }
            //set to go to the next
            index += 1;
            switch (letter)
            {
                case '.':
                    //Execute sounds and effects
                    ReproduceGameFeel(actualEvent.dotClipsInfo[dotCount].clips, actualEvent.dotGfxList[dotCount],
                        actualEvent.dotEventVarChange[dotCount], actualEvent.dotImages[dotCount], actualEvent.eventsToReset, actualEvent.timeCost);
                    //Says that have passed one more dot
                    dotCount += 1;
                    //Pause for some time
                    yield return new WaitForSeconds(dotPauseTime);
                    //Check next letter
                    StartCoroutine(ReproduceText());
                    yield break;
                case '$':
                    switch (dollarActive)
                    {
                        case false:
                            dollarActive = true;
                            StartCoroutine(ReproduceText());
                            yield break;
                        case true:
                            ChangeTextColor(actualEvent.dollarColor.colorsPerDollarSign[dollarCount]);
                            dollarCount += 1;
                            dollarActive = false;
                            StartCoroutine(ReproduceText());
                            listCharsColor = new List<int>();
                            yield break;
                    }
                case ',':
                    yield return new WaitForSeconds(commaPauseTime);
                    StartCoroutine(ReproduceText());
                    yield break;
                case ' ':
                    yield return new WaitForSeconds(spaceTimeToDraw);
                    StartCoroutine(ReproduceText());
                    yield break;
                default:
                    yield return new WaitForSeconds(letterTimeToDraw);
                    StartCoroutine(ReproduceText());
                    yield break;
            }
        }
        switch (actualEvent.choices[0].idle)
        {
            //if is idle event, will display text based on stats.
            case true:
                StartCoroutine(DisplayIdle(actualEvent.choices[0]));
                break;
            //case not idle
            default:
                switch (actualEvent.randomEvent)
                {
                    //case player can't choose event
                    case true:
                        //Set event randomly
                        StartCoroutine(GoToRandomEvent(actualEvent.choices));
                        break;
                    case false:
                        //Display choices.
                        choicesController.StartDisplaying(actualEvent.choices);
                        break;
                }
                break;
        }
    }

    private void ChangeTextColor(Color color)
    {
        for (int i = listCharsColor[0]; i < listCharsColor[listCharsColor.Count-1]; i++)
        {
            var meshIndex = logTextBox.textInfo.characterInfo[i].materialReferenceIndex;
            var vertexIndex = logTextBox.textInfo.characterInfo[i].vertexIndex;
            var vertexColors = logTextBox.textInfo.meshInfo[meshIndex].colors32;
            vertexColors[vertexIndex] = color;
        }
    }

    private void ReproduceGameFeel(List<ClipInfo> clipsInfoList, DotGfxs gfxInfo, DotVarChange eventVarChange, DotImage background, List<Event> eventsToReset, int timeCost)
    {
        foreach (var clipInfo in clipsInfoList)
        {
            //Request that the sound is played after X time
            soundController.ReproduceSound(clipInfo);
        }
        playerController.AttPlayerVars(eventVarChange);
        graphController.ChangeSprite(background);
        effectController.ReproduceEffect(gfxInfo);
        TimeController.AddTime(timeCost);
        ResetEvents(eventsToReset);
    }


    private void ResetEvents(List<Event> eventsToReset)
    {
        foreach (var eEvent in eventsToReset)
        {
            eEvent.eventsDoneInThisScene = new List<Event>();
        }
    }


    public DotImage emptyDotImages;
    public DotClipsList emptyDotClipsInfo;
    public DotGfxs emptyDotGfxList;
    public DotVarChange emptyDotVarChange;
    private IEnumerator DisplayIdle(Event idleEvent)
    {
        idleEvent.choices = idleEventsList[0].Choices;
        //first dot
        idleEvent.finalText = ".";

        //Band
        switch (playerController.player.inBand)
        {
            case true:
            //apply band image on the background
            break;
        }
        
        //Variables
        List<DotVariables> playerVars = playerController.player.variables;
        
        //Atp
        float atp = playerVars[(int) VariableName.atp].variableValue;
        if (atp > 80)
        {
            idleEvent.finalText += idleInfos.highAtp;
        }
        else if (atp > 40)
        {
            idleEvent.finalText += idleInfos.normalAtp;
        }
        else if (atp > 20)
        {
            idleEvent.finalText += idleInfos.lowAtp;
        }
        else
        {
            idleEvent.finalText += idleInfos.noneAtp;
        }
        
        //Stomach
        float hunger = playerVars[(int) VariableName.hungry].variableValue;
        if (hunger > 80)
        {
            idleEvent.finalText += idleInfos.highStomach;
        }
        else if (hunger > 40)
        {
            idleEvent.finalText += idleInfos.normalStomach;
        }
        else if (hunger > 20)
        {
            idleEvent.finalText += idleInfos.lowStomach;
        }
        else
        {
            idleEvent.finalText += idleInfos.noneStomach;
        }
        
        //Lust
        float lust = playerVars[(int) VariableName.lust].variableValue;
        if (lust > 80)
        {
            idleEvent.finalText += idleInfos.highLust;
        }
        else if (lust > 40)
        {
            idleEvent.finalText += idleInfos.normalLust;
        }
        else if (lust > 20)
        {
            idleEvent.finalText += idleInfos.lowLust;
        }
        else
        {
            idleEvent.finalText += idleInfos.noneLust;
        }
        
        //Excrement
        float excrement = playerVars[(int) VariableName.excrement].variableValue;
        if (excrement > 80)
        {
            idleEvent.finalText += idleInfos.highExcrement;
        }
        else if (excrement > 40)
        {
            idleEvent.finalText += idleInfos.normalExcrement;
        }
        else if (excrement > 20)
        {
            idleEvent.finalText += idleInfos.lowExcrement;
        }
        else
        {
            idleEvent.finalText += idleInfos.noneExcrement;
        }
        
        //Sleepness
        float sleepiness = playerVars[(int) VariableName.slepiness].variableValue;
        if (sleepiness > 80)
        {
            idleEvent.finalText += idleInfos.highSleepiness;
        }
        else if (sleepiness > 40)
        {
            idleEvent.finalText += idleInfos.normalSleepiness;
        }
        else if (sleepiness > 20)
        {
            idleEvent.finalText += idleInfos.lowSleepiness;
        }
        else
        {
            idleEvent.finalText += idleInfos.noneSleepiness;
        }

        int dotN = CountDots(idleEvent.finalText);
        AddDotsThings(dotN, idleEvent);

        //Wait some time, than cuts the scene to idle.
        yield return new WaitForSeconds(idleEvent.timeToChangeEvent);
        ChangeEvent(idleEvent);
    }
    
    private IEnumerator GoToRandomEvent(List<Event> choices)
    {
        yield return new WaitForSeconds(actualEvent.timeToChangeEvent);
        var intChoice = Random.Range(0, choices.Count);
        ChangeEvent(choices[intChoice]);
    }

    private int CountDots(string str)
    {
        int count = 0;
        foreach (char character in str)
        {
            if (character == '.')
            {
                count += 1;
            }
        }

        return count;
    }

    private void AddDotsThings(int howMany, Event @event)
    {
        for (int i = 0; i < howMany; i++)
        {
            @event.dotImages.Add(emptyDotImages);
        
            @event.dotClipsInfo.Add(emptyDotClipsInfo);
        
            @event.dotGfxList.Add(emptyDotGfxList);
        
            @event.dotEventVarChange.Add(emptyDotVarChange);
        }
    }
}

[Serializable]
public class IdleInfos
{
    public string noneAtp;
    public string lowAtp;
    public string normalAtp;
    public string highAtp;
    
    public string noneStomach;
    public string lowStomach;
    public string normalStomach;
    public string highStomach;

    public string noneLust;
    public string lowLust;
    public string normalLust;
    public string highLust;

    public string noneExcrement;
    public string lowExcrement;
    public string normalExcrement;
    public string highExcrement;

    public string noneSleepiness;
    public string lowSleepiness;
    public string normalSleepiness;
    public string highSleepiness;
}


[Serializable]
public class IdleChoicesPackage
{
    public int historyIntensity;
    public List<Event> Choices;
}



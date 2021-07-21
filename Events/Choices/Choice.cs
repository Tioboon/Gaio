using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Choice : MonoBehaviour, IPointerClickHandler
{
    public float timeToDestroy;
    
    public GameObject canvas;
    public GameObject textPopUpGO;
    public String textInsufficientAtp;
    
    //EventController
    private EventController eventController;
    //The event that the choice executes
    public Event choiceEvent;
    //Nome
    private TextMeshProUGUI nameTXT;
    //Custo
    private TextMeshProUGUI cost;

    private void GetReferences()
    {
        eventController = GameObject.Find("EventController").GetComponent<EventController>();
        nameTXT = transform.Find("Name").GetComponent<TextMeshProUGUI>();
        cost = transform.Find("Cost").GetComponent<TextMeshProUGUI>();
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void SetNameAndCost()
    {
        GetReferences();
        nameTXT.text = choiceEvent.eventName;
        cost.text = choiceEvent.eventCost.ToString();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //Check ATP values
        if (eventController.playerController.player.variables[(int) VariableName.atp].variableValue >=
            choiceEvent.eventCost)
        {
            //Change the event
            eventController.playerController.player.variables[(int) VariableName.atp].variableValue -= choiceEvent.eventCost;
            eventController.actualEvent.eventsDoneInThisScene.Add(choiceEvent);
            eventController.ChangeEvent(choiceEvent);
        }
        else
        {
            //Creates a text saying the player don't have enough ATP to realize that action
            var newTextGO = Instantiate(textPopUpGO);
            var textMeshPro = newTextGO.GetComponent<TextMeshProUGUI>();
            var rectTrans = textMeshPro.GetComponent<RectTransform>();
            textMeshPro.text = textInsufficientAtp;
            textMeshPro.transform.SetParent(canvas.transform);
            textMeshPro.gameObject.SetActive(true);
            rectTrans.anchorMin = Vector2.zero;
            rectTrans.anchorMax = Vector2.one;
            rectTrans.offsetMin = Vector2.zero;
            rectTrans.offsetMax = Vector2.zero;
            rectTrans.localScale = Vector3.one;
            var newFade = newTextGO.AddComponent<FadeToDestroy>();
            newFade.DestroyAfterXSec(timeToDestroy);
        }
    }
}

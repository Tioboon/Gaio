using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventList : MonoBehaviour
{
    public List<Event> eventList;
    public EventController eventController;


    private void OnEnable()
    {
        eventController.actualEvent = eventList[PlayerPrefs.GetInt("actualevent", 1)];
    }

    private void OnDisable()
    {
        for (int i = 0; i < eventList.Count; i++)
        {
            switch (eventList[i] == eventController.actualEvent)
            {
                case false:
                    break;
                case true:
                    PlayerPrefs.SetInt("actualevent", i);
                    break;
            }
        }
    }
}

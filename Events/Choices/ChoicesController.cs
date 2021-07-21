using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoicesController : MonoBehaviour
{
    public GameObject choiceGO;
    public float timeToShowChoice;
    public List<float> xAnchor;
    public float initialDistY;
    public float distanceBetweenY;
    public float ySize;
    private List<Event> actualChoices = new List<Event>();
    private List<GameObject> actualChoicesGO = new List<GameObject>();
    private bool initialized;
    public GameObject textPopUpGo;
    public float timeToPopUpDestroy;

    public void StartDisplaying(List<Event> choices)
    {
        StartCoroutine(DisplayChoices(choices));
    }
    // ReSharper disable Unity.PerformanceAnalysis
    private IEnumerator DisplayChoices(List<Event> choices)
    {
        foreach (var choice in choices)
        {
            var newChoice = Instantiate(choiceGO, transform);
            RectTransform choiceRT = newChoice.GetComponent<RectTransform>();
            var yMax = 1 - (initialDistY + ySize * actualChoicesGO.Count + distanceBetweenY * actualChoicesGO.Count);
            var yMin = yMax - ySize;
            choiceRT.anchorMin = new Vector2(xAnchor[0], yMin);
            choiceRT.anchorMax = new Vector2(xAnchor[1], yMax);
            Choice newChoiceInfo = newChoice.AddComponent<Choice>();
            newChoiceInfo.textPopUpGO = textPopUpGo;
            newChoiceInfo.textInsufficientAtp = "Não tem ATP suficiente";
            newChoiceInfo.choiceEvent = choice;
            newChoiceInfo.canvas = transform.parent.gameObject;
            newChoiceInfo.timeToDestroy = timeToPopUpDestroy;
            newChoiceInfo.SetNameAndCost();
            actualChoices.Add(choice);
            actualChoicesGO.Add(newChoice);
            newChoice.SetActive(true);
            yield return new WaitForSeconds(timeToShowChoice);
        }
    }

    public void StopDisplaying()
    {
        foreach (var GO in actualChoicesGO)
        {
            Destroy(GO);
        }
        actualChoices = new List<Event>();
        actualChoicesGO = new List<GameObject>();
    }
}

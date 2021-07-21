using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphController : MonoBehaviour
{
    private Image backGround;
    private Image imageOne;
    private Image imageTwo;
    private Image imageThree;

    private void Start()
    {
        backGround = transform.Find("BackGround").GetComponent<Image>();
        imageOne = transform.Find("LayerOne").GetComponent<Image>();
        imageTwo = transform.Find("LayerTwo").GetComponent<Image>();
        imageThree = transform.Find("LayerThree").GetComponent<Image>();
    }

    public void ChangeSprite(DotImage sprite)
    {
        if (sprite.changesImage)
        {
            backGround.gameObject.SetActive(true);
            switch (TimeController.time)
            {
                case TimeController.TimeInfo.evening:
                    backGround.sprite = sprite.backGround.evening;
                    break;
                case TimeController.TimeInfo.morning:
                    backGround.sprite = sprite.backGround.morning;
                    break;
                case TimeController.TimeInfo.night:
                    backGround.sprite = sprite.backGround.night;
                    break;
                case TimeController.TimeInfo.raining:
                    backGround.sprite = sprite.backGround.raining;
                    break;
            }

            switch (sprite.layerOne != null)
            {
                case true:
                    imageOne.gameObject.SetActive(true);
                    imageOne.sprite = sprite.layerOne;
                    break;
                case false:
                    imageOne.gameObject.SetActive(false);
                    break;
            }

            switch (sprite.layerTwo != null)
            {
                case true:
                    imageTwo.gameObject.SetActive(true);
                    imageTwo.sprite = sprite.layerTwo;
                    break;
                case false:
                    imageTwo.gameObject.SetActive(false);
                    break;
            }

            switch (sprite.layerThree != null)
            {
                case true:
                    imageThree.gameObject.SetActive(true);
                    imageThree.sprite = sprite.layerThree;
                    break;
                case false:
                    imageThree.gameObject.SetActive(false);
                    break;
            }
        }
    }
}

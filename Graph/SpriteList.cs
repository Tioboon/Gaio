using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteList : MonoBehaviour
{
    public List<Sprite> spriteList;
    public List<Image> images;

    private void OnDisable()
    {
        for (int i = 0; i < images.Count; i++)
        {
            for (int j = 0; j < spriteList.Count; j++)
            {
                if(images[i].gameObject.activeSelf)
                {
                    if (spriteList[j].name == images[i].sprite.name)
                    {
                        PlayerPrefs.SetInt(images[i].name, j);
                    }
                }
                else
                {
                    PlayerPrefs.SetInt(images[i].name, -1);
                }
            }
        }
    }

    private void OnEnable()
    {
        foreach (var image in images)
        {
            try
            {
                if(PlayerPrefs.GetInt(image.name) == -1) return;
                image.sprite = spriteList[PlayerPrefs.GetInt(image.name)];
                image.gameObject.SetActive(true);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
    }
}

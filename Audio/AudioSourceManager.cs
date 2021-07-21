using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceManager : MonoBehaviour
{
    //Audio source
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    //Play the sound, and if isn't in loop, destroy after playing
    public void PlaySound()
    {
        audioSource.Play();
        if(!audioSource.loop)
        {
            StartCoroutine(DestroyThisSpeaker());
        }
    }

    //Destroy after playing
    private IEnumerator DestroyThisSpeaker()
    {
        yield return new WaitForSeconds(audioSource.clip.length);
        Destroy(gameObject);
    }
}

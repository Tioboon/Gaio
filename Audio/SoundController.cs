using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    //0 R, 1 L, 2 F
    public List<Transform> containers;

    public GameObject speaker;
    public List<ClipInfo> playingSoundsList;
    public List<GameObject> audioSourcesPlayingSoundList;
    
    
    public void PlaySound(ClipInfo clips)
    {
        var alreadyPlaying = false;
        foreach (var clip in playingSoundsList)
        {
            if(clip == clips) alreadyPlaying = true;
        }
        if(alreadyPlaying) return;
        var newSpeaker = Instantiate(speaker);
        var speakerSource = newSpeaker.GetComponent<AudioSource>();
        speakerSource.clip = clips.audioClip;
        speakerSource.volume = clips.volume;
        speakerSource.pitch = clips.pitch;
        speakerSource.panStereo = clips.stereoPan;
        speakerSource.loop = clips.loop;
        var newSourceManager = newSpeaker.GetComponent<AudioSourceManager>();
        switch (clips.speaker)
        {
            case Speaker.rightSpeaker:
                newSpeaker.transform.parent = containers[0]; 
                playingSoundsList.Add(clips);
                audioSourcesPlayingSoundList.Add(newSpeaker);
                newSourceManager.PlaySound(); 
                break;
            case Speaker.leftSpeaker:
                newSpeaker.transform.parent = containers[1]; 
                playingSoundsList.Add(clips);
                audioSourcesPlayingSoundList.Add(newSpeaker);
                newSourceManager.PlaySound(); break;
            case Speaker.frontSpeaker:
                newSpeaker.transform.parent = containers[2]; 
                playingSoundsList.Add(clips);
                audioSourcesPlayingSoundList.Add(newSpeaker);
                newSourceManager.PlaySound(); break;
        }
    }

    public void ReproduceSound(ClipInfo clipInfo)
    {
        StartCoroutine(ReproduceSoundCoroutine(clipInfo));
    }
    
    private IEnumerator ReproduceSoundCoroutine(ClipInfo clipInfo)
    {
        //Wait the right amount of seconds, than play the sounds;
        yield return new WaitForSeconds(clipInfo.timeToReproduceAfterDot);
        PlaySound(clipInfo);
    }
    
    
    //Não para de tocar o som entre eventos se os dois tiverem o mesmo som em loop
    public void StopSoundsEventChange(List<ClipInfo> exceptions)
    {
        var listIntSoundsToStop = new List<int>();
        //For each sound that is playing in this event
        for (int i = 0; i < playingSoundsList.Count; i++)
        {
            var isException = false;
            //If the sound is in loop
            if (playingSoundsList[i].loop)
            {
                //for each sound that isn't stopped;
                foreach (var clipsInfo in exceptions)
                {
                    //If is the sound that the first for each is checking
                    if (playingSoundsList[i].audioClip == clipsInfo.audioClip)
                    {
                        //Don't destroy speaker
                        isException = true;
                        //Deriva as informações do ClipInfo que já estava tocando.
                        playingSoundsList[i] = clipsInfo;
                    }
                }
                if (isException) continue;
                listIntSoundsToStop.Add(i);
            }
        }
        DestroySpeakers(listIntSoundsToStop);
    }

    private void DestroySpeakers(List<int> soundIntToDestroy)
    {
        foreach (var intN in soundIntToDestroy)
        {
            playingSoundsList.Remove(playingSoundsList[intN]);
            Destroy(audioSourcesPlayingSoundList[intN]);
        }
    }
}

[Serializable]
public class ClipInfo
{
    public AudioClip audioClip;
    public float timeToReproduceAfterDot;
    public Speaker speaker;
    
    public bool loop;
    public float volume;
    public float pitch;
    public float stereoPan;
}

[Serializable]
public enum Speaker
{
    rightSpeaker,
    leftSpeaker,
    frontSpeaker,
}

[Serializable]
public class DotClipsList
{
    public List<ClipInfo> clips;
}
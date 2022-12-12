using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Data;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

public class AudioManager : SerializedMonoBehaviour
{
    [BoxGroup("Player")]
    [OdinSerialize] Dictionary<string, AudioSource> m_audioSources = new Dictionary<string, AudioSource>();
    
    public void Play(string _audioName)
    {
        m_audioSources[_audioName].Play();
    }

    public void SetToBeDestroyed()
    {
        foreach (KeyValuePair<string, AudioSource> kvp in m_audioSources)
        {
            if (kvp.Value.isPlaying)
            {
                //If there's audio still playing, delay it's destroy event
                StartCoroutine(DelayDestroy());
                return;
            }
        }

        Destroy(gameObject);
    }

    IEnumerator DelayDestroy()
    {
        yield return new WaitForSecondsRealtime(5);
        SetToBeDestroyed();
    }
}
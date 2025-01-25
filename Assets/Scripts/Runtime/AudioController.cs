using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioController : MonoBehaviour
{
    public AudioClip[] MusicClips;
    private AudioSource m_AudioSource;

    private void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
        GameManager.Instance.OnPlayerAged += OnPlayerAged;
    }

    private void OnPlayerAged(int age)
    {
        float currentTime = m_AudioSource.time;

        if (age < 15)
        {
            return;
        }
        else if (age < 30)
        {
            m_AudioSource.clip = MusicClips[1];
        }
        else if (age < 55)
        {
            m_AudioSource.clip = MusicClips[2];
        }
        else if (age < 80)
        {
            m_AudioSource.clip = MusicClips[3];
        }
        else if (age < 100)
        {
            m_AudioSource.clip = MusicClips[4];
        }

        m_AudioSource.time = currentTime;
        m_AudioSource.Play();
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;
using static BubbleCollection;

public class GameManager : Singleton<GameManager>
{
    public delegate void AgeEvent(int age);
    public Action BeginPlay;
    public AgeEvent OnPlayerAged;
    public BubbleCollection BubbleCollection;
    public Quotes AgeQuotes;
    public TextAnimation TopText;
    public TextAnimation BottomText;
    public AudioController AudioController;
    private List<BubbleComponent> m_AllBubbles;
    [SerializeField]
    private GameObject m_BubblePrefab;
    private AudioSource m_AudioSource;

    private void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
        m_AllBubbles = new List<BubbleComponent>();
        m_AllBubbles.Add(FindFirstObjectByType<BubbleComponent>());
        TopText.BeginText("Blow to begin...");
    }

    public void TryAdvance()
    {
        m_AudioSource.Play();
    }

    public void Advance()
    {
        foreach (BubbleComponent bubble in m_AllBubbles)
        {
            if (bubble == null)
            {
                continue;
            }

            bubble.GetOlder();
        }

        BottomText.BeginText(m_AllBubbles[0].CurrentAge.ToString());
        SpawnNewBubbles();
    }

    public void BubbleDied(BubbleComponent bubble)
    {
        m_AllBubbles.Remove(bubble);
    }

    private void SpawnNewBubbles()
    {
        int currentAge = m_AllBubbles[0].CurrentAge;
        Vector3 spawPosition = m_AllBubbles[0].transform.position + Vector3.up * 5f;
        List<BubbleEntry> currentBubbles = new List<BubbleEntry>();
        foreach (BubbleEntry entry in BubbleCollection.Bubbles)
        {
            if (entry.SpawnAge == currentAge)
            {
                currentBubbles.Add(entry);
            }
        }

        spawPosition.x -= (currentBubbles.Count - 1) / 2f;
        foreach (BubbleEntry entry in currentBubbles)
        {
            GameObject newBubble = Instantiate(m_BubblePrefab, spawPosition, Quaternion.identity);
            newBubble.name = entry.Data.name;
            newBubble.GetComponent<NPCController>().Setup(m_AllBubbles[0].transform, entry.Data);
            m_AllBubbles.Add(newBubble.GetComponent<BubbleComponent>());
            spawPosition.x += 1;
        }
    }

    public void PlayerAged(int age)
    {
        if (age == 15)
        {
            TopText.BeginText(AgeQuotes.MainQuotes[0]);
        }
        else if (age == 30)
        {
            TopText.BeginText(AgeQuotes.MainQuotes[1]);
        }
        else if (age == 55)
        {
            TopText.BeginText(AgeQuotes.MainQuotes[2]);
        }
        else if (age == 80)
        {
            TopText.BeginText(AgeQuotes.MainQuotes[3]);
        }
        else if (age == 100)
        {
            TopText.BeginText(AgeQuotes.MainQuotes[4]);
        }

        OnPlayerAged?.Invoke(age);
    }

    public void IntroReady()
    {
        BeginPlay?.Invoke();
    }
}

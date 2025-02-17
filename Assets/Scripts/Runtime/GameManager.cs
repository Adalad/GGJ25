using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static BubbleCollection;

public class GameManager : Singleton<GameManager>
{
    public string[] StartingMessages;
    public string[] Credits;
    public int PlayerAge
    {
        get
        {
            return m_AllBubbles[0].CurrentAge;
        }
    }
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
    }

    public void TryAdvance()
    {
        m_AudioSource.Play();
    }

    public void Advance()
    {
        List<BubbleComponent> bubblesToRemove = new List<BubbleComponent>();
        bubblesToRemove.AddRange(m_AllBubbles);
        foreach (BubbleComponent bubble in bubblesToRemove)
        {
            if (bubble == null)
            {
                continue;
            }

            bubble.GetOlder();
        }

        m_AllBubbles = bubblesToRemove;
        BottomText.BeginText(m_AllBubbles[0].CurrentAge.ToString());
        SpawnNewBubbles();
    }

    public void BubbleDied(BubbleComponent bubble)
    {
        if (bubble == m_AllBubbles[0])
        {
            foreach (BubbleComponent bob in m_AllBubbles)
            {
                bubble.EndGame();
            }
        }

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
        switch (age)
        {
            case 1:
                TopText.BeginText(StartingMessages[0]);
                break;
            case 2:
                TopText.BeginText(StartingMessages[1]);
                break;
            case 3:
                TopText.BeginText(StartingMessages[2]);
                break;
            case 4:
                TopText.BeginText(StartingMessages[3]);
                break;
            case 5:
                TopText.BeginText(StartingMessages[4]);
                break;
            case 15:
                TopText.BeginText(AgeQuotes.MainQuotes[0]);
                break;
            case 30:
                TopText.BeginText(AgeQuotes.MainQuotes[1]);
                break;
            case 55:
                TopText.BeginText(AgeQuotes.MainQuotes[2]);
                break;
            case 80:
                TopText.BeginText(AgeQuotes.MainQuotes[3]);
                break;
            case 100:
                TopText.BeginText(AgeQuotes.MainQuotes[4]);
                break;
        }

        OnPlayerAged?.Invoke(age);
    }

    public void IntroReady()
    {
        TopText.BeginText("Blow to begin...");
        BeginPlay?.Invoke();
    }

    public void EndGame(int age)
    {
        if (age == 100)
        {
            StartCoroutine(CreditsRoutine());
            return;
        }

        StartCoroutine(EndGameRoutine());
    }

    private IEnumerator EndGameRoutine()
    {
        TopText.BeginText("You blew your chance!");
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private IEnumerator CreditsRoutine()
    {
        for (int i = 0; i < Credits.Length; i++)
        {
            TopText.BeginText(Credits[i]);
            yield return new WaitForSeconds(5f);
        }

        Application.Quit();
    }
}

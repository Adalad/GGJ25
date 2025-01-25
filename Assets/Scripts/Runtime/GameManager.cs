using System.Collections.Generic;
using UnityEngine;
using static BubbleCollection;

public class GameManager : Singleton<GameManager>
{
    public BubbleCollection BubbleCollection;
    public TextAnimation TopText;
    public TextAnimation BottomText;
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

    public void Advance()
    {
        m_AudioSource.Play();
        foreach (BubbleComponent bubble in m_AllBubbles)
        {
            bubble.GetOlder();
        }

        BottomText.BeginText(m_AllBubbles[0].CurrentAge.ToString());
        SpawnNewBubbles();
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
}

using System.Collections.Generic;
using UnityEngine;
using static BubbleCollection;

public class GameManager : Singleton<GameManager>
{
    public BubbleCollection BubbleCollection;
    private List<BubbleComponent> m_AllBubbles;
    [SerializeField]
    private GameObject m_BubblePrefab;

    private void Start()
    {
        m_AllBubbles = new List<BubbleComponent>();
        m_AllBubbles.Add(FindFirstObjectByType<BubbleComponent>());
        SpawnNewBubbles();
    }

    public void Advance()
    {
        foreach (BubbleComponent bubble in m_AllBubbles)
        {
            bubble.GetOlder();
        }

        SpawnNewBubbles();
    }

    private void SpawnNewBubbles()
    {
        int currentAge = m_AllBubbles[0].CurrentAge;
        Vector3 spawPosition = m_AllBubbles[0].transform.position + Vector3.down * 5f;
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

using System.Collections.Generic;

public class GameManager : Singleton<GameManager>
{
    public BubbleCollection BubbleCollection;
    private List<BubbleComponent> m_AllBubbles;

    private void Start()
    {
        m_AllBubbles = new List<BubbleComponent>();
        m_AllBubbles.Add(FindFirstObjectByType<BubbleComponent>());
    }

    public void Advance()
    {
        foreach (BubbleComponent bubble in m_AllBubbles)
        {
            bubble.GetOlder();
        }
    }
}

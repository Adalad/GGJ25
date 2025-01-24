using UnityEngine;

public class NPCController : BubbleComponent
{
    public BubbleData Data;

    private void Start()
    {

    }

    private void Update()
    {

    }

    protected override void Initialize()
    {
        m_CurrentAge = Data.StartingAge;
    }
}

using UnityEngine;

[CreateAssetMenu(fileName = "BubbleData", menuName = "Scriptable Objects/BubbleData")]
public class BubbleData : ScriptableObject
{
    public AnimationCurve Affinity;
    public int StartingAge;
    public int MaxAge;
}

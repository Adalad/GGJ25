using UnityEngine;

[CreateAssetMenu(fileName = "BubbleAnimation", menuName = "Scriptable Objects/BubbleAnimation")]
public class BubbleAnimation : ScriptableObject
{
    public float FrameTime = 0.2f;
    public Sprite[] ChildSprites;
    public Sprite[] YoungSprites;
    public Sprite[] AdultSprites;
    public Sprite[] OldSprites;
    public Sprite[] AncientSprites;
    public Sprite[] PopSprites;
}

using System;
using UnityEngine;

[CreateAssetMenu(fileName = "BubbleCollection", menuName = "Scriptable Objects/BubbleCollection")]
public class BubbleCollection : ScriptableObject
{
    [Serializable]
    public class BubbleEntry
    {
        [SerializeField]
        public int SpawnAge;
        [SerializeReference]
        public BubbleData Data;
    }

    public BubbleEntry[] Bubbles;
}

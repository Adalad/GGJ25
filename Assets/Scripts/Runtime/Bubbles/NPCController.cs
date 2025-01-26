using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class NPCController : BubbleComponent
{
    public BubbleData Data;
    private Transform m_Target;
    private Rigidbody2D m_Rigidbody;
    private float m_CurrentAffinity;
    [SerializeField]
    private float m_ForceMultiplier = 1f;
    private const float MaxAffinityDistance = 4f;
    private const float MinAffinityDistance = 1f;
    private Vector3 m_LastPosition = Vector3.zero;

    private void Awake() => m_Rigidbody = GetComponent<Rigidbody2D>();

    protected override void Initialize()
    {
        m_CurrentAge = Data.StartingAge;
    }

    public override void GetOlder()
    {
        base.GetOlder();
        if (CurrentAge > Data.MaxAge)
        {
            OnDie();
        }

        m_CurrentAffinity = Map(Data.Affinity.Evaluate(GameManager.Instance.PlayerAge / 100f), 0, 1, MaxAffinityDistance, MinAffinityDistance);
    }

    public void Setup(Transform target, BubbleData data)
    {
        m_Target = target;
        Data = data;
        Initialize();
        m_CurrentAffinity = Map(Data.Affinity.Evaluate(GameManager.Instance.PlayerAge / 100f), 0, 1, MaxAffinityDistance, MinAffinityDistance);
        StartCoroutine(MovementRoutine());
    }

    private IEnumerator MovementRoutine()
    {
        if (m_Target != null)
        {
            m_LastPosition = m_Target.position;
        }

        float currentDistance;
        Vector3 direction;
        while (true)
        {
            currentDistance = Vector3.Distance(m_LastPosition, transform.position);
            direction = (m_LastPosition - transform.position) * m_ForceMultiplier;
            if (currentDistance < m_CurrentAffinity)
            {
                direction *= -1;
            }

            m_Rigidbody.AddForce(direction);

            yield return null;
        }
    }

    private float Map(float x, float in_min, float in_max, float out_min, float out_max)
    {
        return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
    }

    public override void EndGame()
    {
        StopAllCoroutines();
    }
}

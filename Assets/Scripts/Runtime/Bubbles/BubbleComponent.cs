using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public abstract class BubbleComponent : MonoBehaviour
{
    public BubbleAnimation Animation;
    protected int m_CurrentAge;
    private SpriteRenderer m_SpriteRenderer;
    private Sprite[] m_CurrentSprites;
    private Coroutine m_AnimationRoutine;

    private void Awake() => m_SpriteRenderer = GetComponent<SpriteRenderer>();

    private void Start()
    {
        Initialize();
        RefreshSprites();
        // TODO remove when anims are done
        //m_AnimationRoutine = StartCoroutine(AnimationRoutine());
    }

    protected abstract void Initialize();

    public void GetOlder()
    {
        m_CurrentAge++;
        RefreshSprites();
    }

    private void RefreshSprites()
    {
        if (m_CurrentAge < 15)
        {
            m_CurrentSprites = Animation.ChildSprites;
            return;
        }
        else if (m_CurrentAge < 30)
        {
            m_CurrentSprites = Animation.YoungSprites;
        }
        else if (m_CurrentAge < 55)
        {
            m_CurrentSprites = Animation.AdultSprites;
        }
        else if (m_CurrentAge < 80)
        {
            m_CurrentSprites = Animation.OldSprites;
        }
        else if (m_CurrentAge < 100)
        {
            m_CurrentSprites = Animation.AncientSprites;
        }
        else
        {
            StartCoroutine(Die());
        }
    }

    private IEnumerator Die()
    {
        if (m_AnimationRoutine != null)
        {
            StopCoroutine(m_AnimationRoutine);
        }

        int currentFrame = 0;
        m_CurrentSprites = Animation.PopSprites;
        while (currentFrame < m_CurrentSprites.Length)
        {
            m_SpriteRenderer.sprite = m_CurrentSprites[currentFrame];
            yield return new WaitForSeconds(Animation.FrameTime);
            currentFrame++;
        }

        Destroy(gameObject);
    }

    private IEnumerator AnimationRoutine()
    {
        int currentFrame = 0;
        while (true)
        {
            m_SpriteRenderer.sprite = m_CurrentSprites[currentFrame];
            yield return new WaitForSeconds(Animation.FrameTime);
            currentFrame = (currentFrame + 1) % m_CurrentSprites.Length;
        }
    }
}

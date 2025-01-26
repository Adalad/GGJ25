using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(CircleCollider2D), typeof(AudioSource))]
public abstract class BubbleComponent : MonoBehaviour
{
    public BubbleAnimation Animation;
    public int CurrentAge
    {
        get => m_CurrentAge;
    }
    protected int m_CurrentAge;
    private SpriteRenderer m_SpriteRenderer;
    private CircleCollider2D m_Collider;
    private AudioSource m_AudioSource;
    private Sprite[] m_CurrentSprites;
    private Coroutine m_AnimationRoutine;

    private void Start()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_Collider = GetComponent<CircleCollider2D>();
        m_AudioSource = GetComponent<AudioSource>();
        RefreshSprites();
        m_AnimationRoutine = StartCoroutine(AnimationRoutine());
    }

    protected abstract void Initialize();

    public virtual void GetOlder()
    {
        m_CurrentAge++;
        RefreshSprites();
        OnAge();
    }

    private void RefreshSprites()
    {
        if (m_CurrentAge < 15)
        {
            transform.localScale = Vector3.one * 0.8f;
            m_CurrentSprites = Animation.ChildSprites;
            return;
        }
        else if (m_CurrentAge < 30)
        {
            transform.localScale = Vector3.one * 0.9f;
            m_CurrentSprites = Animation.YoungSprites;
        }
        else if (m_CurrentAge < 55)
        {
            transform.localScale = Vector3.one;
            m_CurrentSprites = Animation.AdultSprites;
        }
        else if (m_CurrentAge < 80)
        {
            transform.localScale = Vector3.one;
            m_CurrentSprites = Animation.OldSprites;
        }
        else if (m_CurrentAge < 100)
        {
            transform.localScale = Vector3.one;
            m_CurrentSprites = Animation.AncientSprites;
        }
        else
        {
            OnDie();
        }
    }

    protected IEnumerator Die()
    {
        GameManager.Instance.BubbleDied(this);
        m_AudioSource.Play();
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

    protected virtual void OnDie()
    {
        StartCoroutine(Die());
    }

    protected virtual void OnAge()
    {
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

    public virtual void EndGame()
    {
    }
}

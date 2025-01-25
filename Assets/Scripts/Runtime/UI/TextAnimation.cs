using System.Collections;
using TMPro;
using UnityEngine;

public class TextAnimation : MonoBehaviour
{
    public float WaitTime = 1f;
    public float MoveSpeed = 1f;
    public float TopPosition = 200f;
    public float BottomPosition = -100f;
    private TMP_Text m_TextMeshPro;
    private RectTransform m_RectTransform;
    private Coroutine m_MoveRoutine;

    private void Start()
    {
        m_TextMeshPro = GetComponent<TMP_Text>();
        m_RectTransform = GetComponent<RectTransform>();
    }

    public void BeginText(string newText)
    {
        if (m_MoveRoutine != null)
        {
            StopCoroutine(m_MoveRoutine);
        }

        m_TextMeshPro.text = newText;
        m_MoveRoutine = StartCoroutine(MoveRoutine());
    }

    private IEnumerator MoveRoutine()
    {
        Vector3 start = m_RectTransform.anchoredPosition;
        Vector3 target = Vector3.up * TopPosition;
        float ratio = 0f;
        while (Vector3.Distance(m_RectTransform.anchoredPosition, target) > 1f)
        {
            m_RectTransform.anchoredPosition = Vector3.Lerp(start, target, ratio);
            ratio += MoveSpeed * Time.deltaTime;

            yield return null;
        }

        yield return new WaitForSeconds(WaitTime);
        m_RectTransform.anchoredPosition = target;
        start = target;
        target = Vector3.up * BottomPosition;
        ratio = 0f;
        while (Vector3.Distance(m_RectTransform.anchoredPosition, target) > 1f)
        {
            m_RectTransform.anchoredPosition = Vector3.Lerp(start, target, ratio);
            ratio += MoveSpeed * Time.deltaTime;

            yield return null;
        }

        m_RectTransform.anchoredPosition = target;
    }
}

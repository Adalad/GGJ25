using System.Collections;
using TMPro;
using UnityEngine;

public class TextAnimation : MonoBehaviour
{
    public float MoveSpeed = 1f;
    public float TopPosition = 200f;
    public float BottomPosition = -100f;
    private TMP_Text m_TextMeshPro;
    private RectTransform m_RectTransform;

    private void Start()
    {
        m_TextMeshPro = GetComponent<TMP_Text>();
        m_RectTransform = GetComponent<RectTransform>();
    }

    public void BeginText(string newText)
    {
        m_TextMeshPro.text = newText;
        StartCoroutine(MoveRoutine());
    }

    private IEnumerator MoveRoutine()
    {
        Vector3 target = Vector3.up * TopPosition;
        while (Vector3.Distance(m_RectTransform.anchoredPosition, target) > 1f)
        {
            m_RectTransform.anchoredPosition = Vector3.Lerp(m_RectTransform.anchoredPosition, target, MoveSpeed * Time.deltaTime);

            yield return null;
        }

        m_RectTransform.anchoredPosition = target;
        target = Vector3.up * BottomPosition;
        while (Vector3.Distance(m_RectTransform.anchoredPosition, target) > 1f)
        {
            m_RectTransform.anchoredPosition = Vector3.Lerp(m_RectTransform.anchoredPosition, target, MoveSpeed * Time.deltaTime);

            yield return null;
        }

        m_RectTransform.anchoredPosition = target;
    }
}
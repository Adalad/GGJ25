using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class LogoAnimation : MonoBehaviour
{
    [SerializeField]
    private float m_FadeSpeed = 1f;
    private Image m_Image;

    private void Start()
    {
        m_Image = GetComponent<Image>();
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        float alpha = 1f;
        while (alpha > 0f)
        {
            alpha -= m_FadeSpeed * Time.deltaTime;
            m_Image.color = new Color(1f, 1f, 1f, alpha);
            yield return null;
        }

        GameManager.Instance.IntroReady();
    }
}

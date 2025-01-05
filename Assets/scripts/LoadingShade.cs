using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingShade : MonoBehaviour
{
    [SerializeField] private bool startOpaque = false;

    [SerializeField] private GameObject parentObject;
    [SerializeField] private TextMeshProUGUI loadingText;
    [SerializeField] private Image loadingImage;

    public delegate void FadeEvent();
    public event FadeEvent OnFadeCompleted;
    private float currentAlpha;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        float initialAlpha = startOpaque ? 1f : 0f;
        if (startOpaque) parentObject.SetActive(true);
        SetAlpha(initialAlpha);
    }

    public void FadeIn(float duration)
    {
        StartCoroutine(LoadingFade(0, 1, duration));
    }

    public void FadeOut(float duration)
    {
        StartCoroutine(LoadingFade(1, 0, duration));
    }

    private IEnumerator LoadingFade(float start, float end, float duration)
    {
        parentObject.SetActive(true);
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float currentValue = Mathf.Lerp(start, end, t);
            SetAlpha(currentValue);
            yield return null;
        }
        currentAlpha = end;
        OnFadeCompleted?.Invoke();
        SetAlpha(end);
    }

    private void SetAlpha(float alpha)
    {
        loadingText.color = new Color(loadingText.color.r, loadingText.color.g,loadingText.color.b, alpha);
        loadingImage.color = new Color(loadingImage.color.r, loadingImage.color.g,loadingImage.color.g, alpha);
        if (alpha == 0)
        {
            parentObject.SetActive(false);
        }
    }
}

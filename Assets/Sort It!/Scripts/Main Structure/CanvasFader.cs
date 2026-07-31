using UnityEngine;
using System;

[RequireComponent(typeof(CanvasGroup))]
public class CanvasFader : MonoBehaviour
{
    public static CanvasFader Instance;
    private CanvasGroup canvasGroup;

    [Header(" Fade Settings ")]
    [SerializeField] private float fadeDuration = 0.5f;  

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    private void Start()
    {
        // Ekran simsiyah başlar, açılışta hemen FadeIn ile eriyip kaybolur
        FadeIn();
    }

    public void FadeIn(Action onComplete = null)
    {
        if (canvasGroup == null) return;

        // Varsa eski animasyonu iptal et
        LeanTween.cancel(gameObject);

        canvasGroup.alpha = 1f;  
        canvasGroup.blocksRaycasts = true;  

        // BUG-FREE YÖNTEM: alphaCanvas yerine doğrudan Value kullanıyoruz!
        LeanTween.value(gameObject, 1f, 0f, fadeDuration)
            .setEase(LeanTweenType.linear)
            .setIgnoreTimeScale(true) // Zaman dursa bile bu animasyon çalışır!
            .setOnUpdate((float val) => 
            {
                // LeanTween'in saydığı değeri, her karede CanvasGroup'a zorla eşitliyoruz
                canvasGroup.alpha = val;
            })
            .setOnComplete(() =>
            {
                canvasGroup.alpha = 0f;
                canvasGroup.blocksRaycasts = false;
                onComplete?.Invoke();
            });
    }

    public void FadeOut(Action onComplete = null)
    {
        if (canvasGroup == null) return;

        LeanTween.cancel(gameObject);

        canvasGroup.alpha = 0f; 
        canvasGroup.blocksRaycasts = true; 

        // BUG-FREE YÖNTEM
        LeanTween.value(gameObject, 0f, 1f, fadeDuration)
            .setEase(LeanTweenType.linear)
            .setIgnoreTimeScale(true)
            .setOnUpdate((float val) => 
            {
                canvasGroup.alpha = val;
            })
            .setOnComplete(() =>
            {
                canvasGroup.alpha = 1f;
                onComplete?.Invoke();
            });
    }
}
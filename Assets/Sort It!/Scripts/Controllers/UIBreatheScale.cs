using UnityEngine;
using System.Collections;

public class UIBreatheScale : MonoBehaviour
{
    [Header(" Scale Settings ")]
    [Tooltip("Buton orijinal boyutunun yüzde kaçına kadar şişecek? (1.05 = %5 büyüme)")]
    [SerializeField] private float scaleMultiplier = 1.05f;
    
    [Tooltip("Şişme ve sönme işleminin süresi")]
    [SerializeField] private float duration = 0.8f;

    private Vector3 originalScale;

    private void Awake()
    {
        originalScale = transform.localScale;
    }

    private void OnEnable()
    {
        // Unity'nin yükleme krizi geçirmemesi için animasyonu bir Coroutine'e devrediyoruz!
        StartCoroutine(StartAnimationDelay());
    }

    private IEnumerator StartAnimationDelay()
    {
        yield return null; // SİHİR BURADA: Tam olarak 1 frame bekle!

        // Eğer o 1 frame içinde obje yanlışlıkla tekrar kapanırsa diye güvenlik kilidi
        if (!gameObject.activeInHierarchy) yield break; 

        transform.localScale = originalScale;
        LeanTween.scale(gameObject, originalScale * scaleMultiplier, duration)
            .setLoopPingPong()
            .setEaseInOutSine();
    }

    private void OnDisable()
    {
        LeanTween.cancel(gameObject);
        transform.localScale = originalScale;
    }
}
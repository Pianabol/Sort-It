using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CanvasGroup))]
public class UIHeartbeatFade : MonoBehaviour
{
    [Header(" Fade Settings ")]
    [Tooltip("Buton ne kadar görünmez olacak? (0 = Tam kaybolur, 0.2 = Hafif silikleşir)")]
    [SerializeField] private float minAlpha = 0.2f;
    
    [Tooltip("Kaybolma ve geri gelme süresi")]
    [SerializeField] private float duration = 0.6f;

    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        StartCoroutine(StartAnimationDelay());
    }

    private IEnumerator StartAnimationDelay()
    {
        yield return null; // 1 frame bekle!

        if (!gameObject.activeInHierarchy) yield break;

        canvasGroup.alpha = 1f;
        LeanTween.alphaCanvas(canvasGroup, minAlpha, duration)
            .setLoopPingPong()
            .setEaseInOutSine();
    }

    private void OnDisable()
    {
        LeanTween.cancel(gameObject);
        canvasGroup.alpha = 1f;
    }
}
/*
using System.Collections;
using System.Collections.Generic;   
using UnityEngine;
public class BottleController : MonoBehaviour
{
    public Color[] bottleColors;
    public SpriteRenderer bottleMaskSR;

    public AnimationCurve ScaleAndRotationMultiplierCurve;
    public AnimationCurve FillAmountCurve;
    public AnimationCurve RotationSpeedMultiplierCurve;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    /* void Start()
    {
        UpdateColorsOnShader();
    } */

    // Update is called once per frame
    /* void Update()
    {
        if(Input.GetKeyUp(KeyCode.Space))
        {
            StartCoroutine(RotateBottle());
        } 
    } */

   /* void UpdateColorsOnShader()
    {
        bottleMaskSR.material.SetColor("_C1", bottleColors[0]);
        bottleMaskSR.material.SetColor("_C2", bottleColors[1]);
        bottleMaskSR.material.SetColor("_C3", bottleColors[2]);
        bottleMaskSR.material.SetColor("_C4", bottleColors[3]);
    }
    public float timeToRotate = 1.0f;
        
    IEnumerator RotateBottle()
    {
    float t = 0f;

    float startFill = 1f;
    float endFill = 0f; // test için tamamen boşalsın

        while (t < timeToRotate)
        {
            float lerpValue = t / timeToRotate;
            float angleValue = Mathf.Lerp(0f, 90f, lerpValue);

            transform.eulerAngles = new Vector3(0f, 0f, angleValue);

            float sarm = ScaleAndRotationMultiplierCurve.Evaluate(angleValue);
            bottleMaskSR.material.SetFloat("_SARM", sarm);

            float fillProgress = FillAmountCurve.Evaluate(lerpValue);
            float fillAmount = Mathf.Lerp(startFill, endFill, fillProgress);
            bottleMaskSR.material.SetFloat("_FillAmount", Mathf.Clamp01(fillAmount));

            t += Time.deltaTime*RotationSpeedMultiplierCurve.Evaluate(angleValue);
            yield return null;
        }

        transform.eulerAngles = new Vector3(0f, 0f, 90f);
        bottleMaskSR.material.SetFloat("_SARM", ScaleAndRotationMultiplierCurve.Evaluate(90f));
        bottleMaskSR.material.SetFloat("_FillAmount", endFill);

        StartCoroutine(RotateBottleBack());
    }

    IEnumerator RotateBottleBack()
    {
        float t = 0f;

        while (t < timeToRotate)
        {
            float lerpValue = t / timeToRotate;
            float angleValue = Mathf.Lerp(90f, 0f, lerpValue);

            transform.eulerAngles = new Vector3(0f, 0f, angleValue);

            float sarm = ScaleAndRotationMultiplierCurve.Evaluate(angleValue);
            bottleMaskSR.material.SetFloat("_SARM", sarm);

          /*float fillProgress = FillAmountCurve.Evaluate(lerpValue);
            float fillAmount = Mathf.Lerp(startFill, endFill, fillProgress);
            bottleMaskSR.material.SetFloat("_FillAmount", Mathf.Clamp01(fillAmount));
            

            t += Time.deltaTime;
            yield return null;
        }

        transform.eulerAngles = new Vector3(0f, 0f, 0f);
        bottleMaskSR.material.SetFloat("_SARM", ScaleAndRotationMultiplierCurve.Evaluate(0f));
    }
}
*/
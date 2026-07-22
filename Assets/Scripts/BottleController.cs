using System.Collections;
using System.Collections.Generic;   
using UnityEngine;

public class BottleController : MonoBehaviour
{
    public SpriteRenderer bottleMaskSR;
    public Color[] bottleColors;

    public AnimationCurve ScaleAndRotationMultiplierCurve;
    public AnimationCurve FillAmountCurve;
    public AnimationCurve RotationSpeedMultiplier;

    public float timeToRotate = 1.0f;


    void Start()
    {
        UpdateColorsOnShader();
    }

    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Space))
        {
            StartCoroutine(RotateBottle());
        } 
    }

    void UpdateColorsOnShader()
    {
        bottleMaskSR.material.SetColor("_C1", bottleColors[0]);
        bottleMaskSR.material.SetColor("_C2", bottleColors[1]);
        bottleMaskSR.material.SetColor("_C3", bottleColors[2]);
        bottleMaskSR.material.SetColor("_C4", bottleColors[3]);
    }

    IEnumerator RotateBottle()
    {
        float t=0;
        float lerpValue;
        float angleValue;

        while(t<timeToRotate)
        {
            lerpValue = t/timeToRotate;
            angleValue = Mathf.Lerp(0f, 90f, lerpValue);

            transform.eulerAngles = new Vector3(0f, 0f, angleValue);
            bottleMaskSR.material.SetFloat("_SARM", ScaleAndRotationMultiplierCurve.Evaluate(angleValue));
            bottleMaskSR.material.SetFloat("_FillAmount", FillAmountCurve.Evaluate(angleValue));

            t+=Time.deltaTime*RotationSpeedMultiplier.Evaluate(angleValue);

            yield return new WaitForEndOfFrame();
        }

        angleValue = 90f;
        transform.eulerAngles = new Vector3(0f, 0f, angleValue);
        bottleMaskSR.material.SetFloat("_SARM", ScaleAndRotationMultiplierCurve.Evaluate(angleValue));
        bottleMaskSR.material.SetFloat("_FillAmount", FillAmountCurve.Evaluate(angleValue));

        StartCoroutine(RotateBottleBack());

    }

    IEnumerator RotateBottleBack()
    {
        float t=0;
        float lerpValue;
        float angleValue;

        while(t<timeToRotate)
        {
            lerpValue = t/timeToRotate;
            angleValue = Mathf.Lerp(90f, 0f, lerpValue);

            transform.eulerAngles = new Vector3(0f, 0f, angleValue);
            bottleMaskSR.material.SetFloat("_SARM", ScaleAndRotationMultiplierCurve.Evaluate(angleValue));

            t+=Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        angleValue = 0f;
        transform.eulerAngles = new Vector3(0f, 0f, angleValue);
        bottleMaskSR.material.SetFloat("_SARM", ScaleAndRotationMultiplierCurve.Evaluate(angleValue));
    }
}
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

    public float[] fillAmounts;
    public float[] rotationValues;

    private int rotationIndex = 0;
    [Range(0,4)]
    public int numberOfColorsInBottle=4;

    public Color topColor;
    public int numberOfTopColorLayers=1;

    public BottleController bottleControllerRef;
    public bool justThisBottle = false;

    private int numberOfColorsToTransfer = 0;




    public float timeToRotate = 1.0f;


    void Start()
    {
        bottleMaskSR.material.SetFloat("_FillAmount", fillAmounts[numberOfColorsInBottle]);

        UpdateColorsOnShader();
        UpdateTopColorValues();
    }

    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Space) && justThisBottle==true)
        {
            UpdateTopColorValues();

            if(bottleControllerRef.FillBottleCheck(topColor))
            {
                numberOfColorsToTransfer=Mathf.Min(numberOfTopColorLayers, 4-bottleControllerRef.numberOfColorsInBottle);    

                for(int i=0; i<numberOfColorsToTransfer; i++)
                {
                    bottleControllerRef.bottleColors[bottleControllerRef.numberOfColorsInBottle+i] = topColor;
                }
                bottleControllerRef.UpdateColorsOnShader();

            }

            CalculateRotationIndex(4 - bottleControllerRef.numberOfColorsInBottle);

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

        float lastAngleValue = 0;


        while(t<timeToRotate)
        {
            lerpValue = t/timeToRotate;
            angleValue = Mathf.Lerp(0f, rotationValues[rotationIndex], lerpValue);

            transform.eulerAngles = new Vector3(0f, 0f, angleValue);
            bottleMaskSR.material.SetFloat("_SARM", ScaleAndRotationMultiplierCurve.Evaluate(angleValue));
            
            if(fillAmounts[numberOfColorsInBottle] > FillAmountCurve.Evaluate(angleValue))
            {
                bottleMaskSR.material.SetFloat("_FillAmount", FillAmountCurve.Evaluate(angleValue));

                bottleControllerRef.FillUp(FillAmountCurve.Evaluate(lastAngleValue) - FillAmountCurve.Evaluate(angleValue));
            }

            t+=Time.deltaTime*RotationSpeedMultiplier.Evaluate(angleValue);
            
            lastAngleValue = angleValue;

            yield return new WaitForEndOfFrame();
        }

        angleValue = rotationValues[rotationIndex];
        transform.eulerAngles = new Vector3(0f, 0f, angleValue);
        bottleMaskSR.material.SetFloat("_SARM", ScaleAndRotationMultiplierCurve.Evaluate(angleValue));
        bottleMaskSR.material.SetFloat("_FillAmount", FillAmountCurve.Evaluate(angleValue));

        numberOfColorsInBottle -= numberOfColorsToTransfer;
        bottleControllerRef.numberOfColorsInBottle += numberOfColorsToTransfer;

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
            angleValue = Mathf.Lerp(rotationValues[rotationIndex], 0f, lerpValue);

            transform.eulerAngles = new Vector3(0f, 0f, angleValue);
            bottleMaskSR.material.SetFloat("_SARM", ScaleAndRotationMultiplierCurve.Evaluate(angleValue));

            t+=Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }
        UpdateTopColorValues();

        angleValue = 0f;
        transform.eulerAngles = new Vector3(0f, 0f, angleValue);
        bottleMaskSR.material.SetFloat("_SARM", ScaleAndRotationMultiplierCurve.Evaluate(angleValue));
    }

    void UpdateTopColorValues()
    {
        if(numberOfColorsInBottle!=0)
        {
            numberOfTopColorLayers = 1;
            topColor = bottleColors[numberOfColorsInBottle - 1];

            if(numberOfColorsInBottle == 4)
            {
                if(bottleColors[3].Equals(bottleColors[2]))
                {
                    numberOfTopColorLayers = 2;
                    if(bottleColors[2].Equals(bottleColors[1]))
                    {
                        numberOfTopColorLayers = 3;
                        if(bottleColors[1].Equals(bottleColors[0]))
                        {
                            numberOfTopColorLayers = 4;
                        }
                    }
                }
            }

            else if(numberOfColorsInBottle == 3)
            {
                if(bottleColors[2].Equals(bottleColors[1]))
                {
                    numberOfTopColorLayers = 2;
                    if(bottleColors[1].Equals(bottleColors[0]))
                    {
                        numberOfTopColorLayers = 3;
                    }
                }
            }
            
            else if(numberOfColorsInBottle == 2)
            {
                if(bottleColors[1].Equals(bottleColors[0]))
                {
                    numberOfTopColorLayers = 2;
                }
            }

            rotationIndex = 3 - (numberOfColorsInBottle - numberOfTopColorLayers);

        }
    }

    private bool FillBottleCheck(Color colorToCheck)
    {
        if(numberOfColorsInBottle == 0)
        {
            return true;
        }
        else
        {
            if(numberOfColorsInBottle==4)
            {
                return false;
            }
            else
            {
                if(topColor.Equals(colorToCheck))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }

    private void CalculateRotationIndex(int numberOfEmptySpacesInSecondBottle)
    {
        rotationIndex = 3 - (numberOfColorsInBottle - Mathf.Min(numberOfEmptySpacesInSecondBottle, numberOfTopColorLayers));
    }

    private void FillUp(float fillAmountToAdd)
    {
        bottleMaskSR.material.SetFloat("_FillAmount", bottleMaskSR.material.GetFloat("_FillAmount") + fillAmountToAdd);
    }
}
using UnityEngine;
using System;

[Serializable] public class LinearProportionConverter
{
    [SerializeField] public float dimension1Min;
    [SerializeField] public float dimension1Max;
    [SerializeField] public float dimension2Min;
    [SerializeField] public float dimension2Max;

    public LinearProportionConverter(float dimension1Min, float dimension1Max, float dimension2Min, float dimension2Max)
    {
        this.dimension1Min = dimension1Min;
        this.dimension1Max = dimension1Max;
        this.dimension2Min = dimension2Min;
        this.dimension2Max = dimension2Max;
    }

    public float CalculateDimension1Value(float dimension2Value)
    {
        var a = dimension2Min;
        var b = dimension2Max;
        var c = dimension1Min;
        var d = dimension1Max;
        var e = dimension2Value;

        var result = c + (((e - a) * (d - c)) / (b - a));

        return result;
    }

    public float CalculateDimension2Value(float dimension1Value)
    {
        var a = dimension1Min;
        var b = dimension1Max;
        var c = dimension2Min;
        var d = dimension2Max;
        var e = dimension1Value;

        // Debug.Log("dimension1Min: " + dimension1Min);
        // Debug.Log("dimension1Max: " + dimension1Max);
        // Debug.Log("dimension2Min: " + dimension2Min);
        // Debug.Log("dimension2Max: " + dimension2Max);
        // Debug.Log("dimension1Value: " + dimension1Value);

        var result = c + (((e - a) * (d - c)) / (b - a));

        return result;
    }
}
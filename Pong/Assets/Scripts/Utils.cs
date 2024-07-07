using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Tags
{
    public const string SCORE = "Score";
    public const string RACKET = "Racket";
}

public static class Utils
{
    public static Vector2 CustomRandomInsideUnitCircle(float maxSlope = 0.5f)
    {
        Vector2 result = Random.insideUnitCircle;

        while (Mathf.Abs(Vector2.Dot(result, Vector2.right)) < maxSlope) result = Random.insideUnitCircle;

        return result;
    }
}

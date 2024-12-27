using System.Collections.Generic;
using UnityEngine;

public class BezierCurve
{
    private readonly Vector2 start;
    private readonly Vector2 control;
    private readonly Vector2 end;

    public BezierCurve(Vector2 start, Vector2 end)
    {
        this.start = start;
        this.end = end;

        // Контрольная точка, определяющая кривизну
        control = (start + end) / 2 + new Vector2(0, 50); // Увеличиваем по оси Y для примера
    }

    public Vector2 GetPoint(float t)
    {
        // Формула для Bezier кривой
        var u = 1 - t;
        var tt = t * t;
        var uu = u * u;

        var p = uu * start; // (1 - t)^2 * P0
        p += 2 * u * t * control; // 2(1 - t)t * P1
        p += tt * end; // t^2 * P2

        return p;
    }

    public List<Vector2> GetCurvePoints(int segments = 10)
    {
        var points = new List<Vector2>();
        for (var i = 0; i <= segments; i++) points.Add(GetPoint(i / (float)segments));
        return points;
    }
}
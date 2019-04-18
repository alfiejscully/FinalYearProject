using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bezier : MonoBehaviour
{

    //vectors that store the control points
    public Transform point0, point1, point2;

    //stores the line
    public LineRenderer line;

    private int numPoints = 50;
    private Vector3[] pos = new Vector3[50]; 

    void Start()
    {
        line.positionCount = numPoints;

        DrawQuadCurve();
    }

    void Update()
    {
        DrawQuadCurve();
    }

    public void DrawQuadCurve()
    {
        for (int i = 1; i < numPoints + 1; i++)
        {
            float t = i / (float)numPoints;
            pos[i - 1] = CalculateQuadBezierPoint(t, point0.position, point1.position, point2.position);
        }
        line.SetPositions(pos);
    }

    //points are plotted by this function 
    public Vector3 CalculateQuadBezierPoint(float t, Vector3 _p0, Vector3 _p1, Vector3 _p2)
    {
        //quadratic function equation
        //B(t) = (1-t)2P0 + 2(1-t)tP1 + t2P2  
        //         u           u        tt
        //        uu * p0 + 2 * u * t * p1 + tt * p2  

        float u = 1f - t;
        float tt = t * t;
        float uu = u * u;

        Vector3 p = uu * _p0;
        p += 2 * u * t * _p1;
        p += tt * _p2;

        return p;
    }

}

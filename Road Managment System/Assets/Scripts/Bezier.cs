using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bezier : MonoBehaviour
{

    //vectors that store the control points
    public Vector3 point0;
    public Vector3 point1;
    public Vector3 point2;

    public float length = 0;
    public Vector3[] points;

    void Start()
    {

    }

    void Update()
    {

    }

    //Initalise bezier, 
    //vec0 is start point
    //vec1 is handler  
    //vec2 is end point 
    public Bezier(Vector3 vec0, Vector3 vec1, Vector3 vec2)
    {
        this.point0 = vec0;
        this.point1 = vec1;
        this.point2 = vec2;
    }

    //points are plotted by this function (uses the quad equation)
    public Vector3 CalculateQuadBezierPoint(float t)
    {
        //B(t) = (1-t)2P0 + 2(1-t)tP1 + t2P2  
        //         u           u        tt
        //        uu * p0 + 2 * u * t * p1 + tt * p2  

        float u = 1f - t;
        float tt = t * t;
        float uu = u * u;

        Vector3 p = uu * point0;
        p += 2 * u * t * point1;
        p += tt * point2;

        return p;
    }

    //number is the amount of points and precision is the matching  
    public void CalculatePoints(int number, int precision = 100)
    {
        //checks if the number is less than the precision 
        if (number > precision)
        {
            Debug.LogError("number is lower than the precision");
        }


        //calculates the length with precision to a rough estimation, saves length in array 
        length = 0;

        //store the lengths between CalculateQuadBezierPoint in an array 
        float[] arcLengths = new float[precision];

        Vector3 oldPoint = CalculateQuadBezierPoint(0);

        for (int i = 1; i < arcLengths.Length; i++)
        {
            Vector3 newPoint = CalculateQuadBezierPoint((float)i / precision); //will get the next point
            arcLengths[i] = Vector3.Distance(oldPoint, newPoint); //distance to old point from new point
            length += arcLengths[i]; //adds to the bezier's length
            oldPoint = newPoint; //new point will be old point in next loop 
        }

        //points array created
        points = new Vector3[number];
        //spaces out the length
        float sectionLength = length / number;

        //index of the arc
        int arcIdx = 0;

        float walkLength = 0; // how far along the path that we've walk
        oldPoint = CalculateQuadBezierPoint(0);

        //will iterate through the points then set them
        for (int i = 0; i < points.Length; i++)
        {
            float iSecLength = i * sectionLength; //total lenth for walkLength to be equal and vaild

            //run through the arcLengths untill passed
            if (walkLength < iSecLength)
            {
                walkLength += arcLengths[arcIdx]; //adds next arcLength to walk
                arcIdx++; //next arcLength
            }

            points[i] = CalculateQuadBezierPoint((float)arcIdx / arcLengths.Length);
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour, IHeapItem<Node>
{
	public bool traversable;
	public Vector3 worldPos;

	public float gCost;
	public float hCost;

	public Node parent;

    int heapIdx;

	public List<Node> connections;

	void Start()
	{
		traversable = true;
	}

	public Vector3 GetPosition()
	{
		return transform.position;
	}		

	//creates fCost by adding gCost to hCost
	public float fCost
	{
		get
		{
			return gCost + hCost;
		}
	}

	public int HeapIdx
	{
		get
		{
			return heapIdx;
		}
		set
		{
			heapIdx = value;
		}
	}

	public int CompareTo(Node nodeCompare)
	{
		int compare = fCost.CompareTo(nodeCompare.fCost);
		if (compare == 0)
		{
			compare = hCost.CompareTo(nodeCompare.hCost);
		}
		return -compare;
	}

    public float GetDistance(Node other)
    {
        Vector3 direction = new Vector3
            (
                GetPosition().x - other.GetPosition().x,
                GetPosition().y - other.GetPosition().y,
                GetPosition().z - other.GetPosition().z
            );

        float distance = direction.magnitude;

        return distance;
    }

    public List<Node> GetSubnodes(Node other)
    {
        List<Node> subnodes = new List<Node>();

        // Total amount of divisions 
        int divisions = 10;

        // Find distance between this node and the other
        float distance = GetDistance(other);

        // Divide distance by the divisions to get sub distance
        float subdistance = (distance / divisions);

        // Add sub node at position of this node to the subnodes
        subnodes.Add(this);

        Vector3 difference = other.transform.position - transform.position;

        // Calculate direction of vector between this node and other node
        Vector3 direction = transform.position + (difference).normalized;

        // Find position at next node using the 'sub distance' & Multiply the direction by the 'sub distance'
        Vector3 translationAmount = direction * subdistance;

        Vector3 h = (difference / 2.0f);

        //Vector3 y = new Vector3(0.0f, 1.0f, 0.0f);
        Vector3 y = transform.forward;

        Vector3 cross = Vector3.Cross(h, y);

        Vector3[] positions = DrawQuadCurve(divisions, transform, cross, other.transform);

        for (int i = 0; i < divisions; i++)
        {
            GameObject node = new GameObject();

            node.AddComponent<Node>();

            //Vector3 translation = transform.position + (translationAmount * (i + 1));
            
            Vector3 translation = positions[i];

            node.transform.position = translation;

            subnodes.Add(node.GetComponent<Node>());
        }

        return subnodes;
    }

    public Vector3[] DrawQuadCurve(int numPoints, Transform point0, Vector3 point1, Transform point2)
    {
        Vector3[] positions = new Vector3[numPoints];

        for (int i = 1; i < numPoints + 1; i++)
        {
            float t = i / (float)numPoints;
            positions[i - 1] = CalculateQuadBezierPoint(t, point0.position, point1, point2.position);
        }

        return positions;
    }

  
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




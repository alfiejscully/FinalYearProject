using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour, IHeapItem<Node>
{
	public bool traversable;

	public float gCost;
	public float hCost;

	public Node parent;

    int heapIdx;

	public List<Node> connections;

    void Awake()
    {
        traversable = true;
    }

    void Update()
    {
       

    }
	void Start()
	{
        
    }
    
    public Vector3 GetPosition()
	{
		return transform.position;
	}		

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
        //list to store the subnodes in
        List<Node> subnodes = new List<Node>();

        // Total amount of divisions 
        int divisions = 10;

        // Find distance between this node and the other
        float distance = GetDistance(other);

        // Add sub node at position of this node to the subnodes
        subnodes.Add(this);

        //find the distance between the nodes
        Vector3 difference = other.transform.position - transform.position;

        //
        Vector3 cross = transform.position + (difference / 2);

        //
        Vector3 sumfing = (transform.forward - other.transform.forward) * (distance / 2);

        //
        cross += sumfing;

        //
        Vector3[] positions = DrawQuadCurve(divisions, transform.position, cross, other.transform.position);

        for (int i = 0; i < divisions; i++)
        {
            GameObject node = new GameObject();

            node.AddComponent<Node>();

            Vector3 translation = positions[i];

            node.transform.position = translation;

            subnodes.Add(node.GetComponent<Node>());
                        
        }
                
        return subnodes;
    }

    public Vector3[] DrawQuadCurve(int numPoints, Vector3 point0, Vector3 point1, Vector3 point2)
    {
        Vector3[] positions = new Vector3[numPoints + 1];

        for (int i = 0; i < numPoints + 1; i++)
        {
            float t = i / (float)numPoints;
            positions[i] = CalculateQuadBezierPoint(t, point0, point1, point2);
        }

        return positions;
    }

  
    public Vector3 CalculateQuadBezierPoint(float t, Vector3 _p0, Vector3 _p1, Vector3 _p2)
    {
        //quadratic function equation
        //B(t) = (1-t)2P0 + 2(1-t)tP1 + t2P2  

        //quadratic function equation
        Vector3 pos = (Mathf.Pow(1.0f - t, 2) * _p0) + (2.0f * (1.0f - t) * t * _p1) + (Mathf.Pow(t, 2) * _p2);

        return pos;
    }

}




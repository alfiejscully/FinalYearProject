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

    public Pathfinder pathfinder;

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

   

    public List<Node> GetSubnodes(Node other)
    {
        List<Node> subnodes = new List<Node>();

        int divisions = 3;

        // Find distance between this node and the other
        float d = pathfinder.GetDistance(this, other);

        // Divide distance by the divisions to get sub distance
        float sd = (d / divisions);

        // Add sub node at position of this node to the subnodes
        


        // Find position at next node using the 'sub distance'

        // etc...

        // Add the end node to the nodes

        return subnodes;
    }

}




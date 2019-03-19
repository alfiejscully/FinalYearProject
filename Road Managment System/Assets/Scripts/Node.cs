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

}




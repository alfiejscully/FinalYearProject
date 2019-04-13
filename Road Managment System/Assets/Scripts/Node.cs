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
        int divisions = 3;

        // Find distance between this node and the other
        float distance = GetDistance(other);

        // Divide distance by the divisions to get sub distance
        float subdistance = (distance / divisions);

        // Add sub node at position of this node to the subnodes
        subnodes.Add(this);

        // Calculate direction of vector between this node and other node
        Vector3 direction = (other.transform.position - transform.position).normalized;

        // Find position at next node using the 'sub distance' & Multiply the direction by the 'sub distance'
        Vector3 translationAmount = direction * subdistance;

        for (int i = 0; i < divisions; i++)
        {
            GameObject node = new GameObject();

            node.AddComponent<Node>();

            Vector3 translation = transform.position + (translationAmount * (i + 1));

            node.transform.position = translation;

            subnodes.Add(node.GetComponent<Node>());
        }

        return subnodes;
    }

}




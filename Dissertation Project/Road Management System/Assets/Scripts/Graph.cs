using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour 
{
	public List<Node> nodes;

	void Awake()
	{
        //graph is created before program starts
		CreateGraph();
	}

	void CreateGraph()
	{
        //list of nodes created on the heap
		nodes = new List<Node>();

		List<GameObject> Children = new List<GameObject>();

		foreach (Transform child in transform)
		{
			Children.Add(child.gameObject);
		}

		foreach (var child in Children)
		{
			Node node = child.GetComponent<Node>();

			nodes.Add(node);
		}
	}

}
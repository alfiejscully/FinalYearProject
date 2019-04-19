using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

public class Pathfinder : MonoBehaviour
{
    //transforms for the start and end position 
	public Transform seeker, target;

    //allows the pathfinder to add-on a graph
	public Graph graph;

    //list of the nodes in path
	public List<Node> Path;

    //gizmos 
	public bool onlyPathGizmos;

	//used for the gizmos start point
	Vector3 snp;
	Vector3 sp;

	//used for the gizmos end point
	Vector3 enp;
	Vector3 ep;

	void Start()
	{

	}

	void Update()
	{
		FindPath(seeker.position, target.position);
	}

	void FindPath(Vector3 startPos, Vector3 endPos)
	{
		Node startNode = GetClosestNode(startPos);
		Node endNode = GetClosestNode(endPos);

		snp = startNode.GetPosition();
		sp = startPos;

		enp = endNode.GetPosition();
		ep = endPos;

		Heap<Node> openSet = new Heap<Node>(graph.nodes.Count);
		HashSet<Node> closedSet = new HashSet<Node>();

		openSet.Add(startNode);

		while (openSet.Count > 0)
		{
			Node currentNode = openSet.RemoveFirst();

			closedSet.Add(currentNode);

			if (currentNode == endNode)
			{
				RetracePath(startNode, endNode);

				onlyPathGizmos = true;

				seeker.GetComponent<Movement>().Pathfind = true;

				return;
			}

			foreach (Node neighbour in currentNode.connections)
			{
				if (!neighbour.traversable || closedSet.Contains(neighbour))
				{
					continue;
				}

				float newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);

				if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
				{
					neighbour.gCost = newMovementCostToNeighbour;
					neighbour.hCost = GetDistance(neighbour, endNode);
					neighbour.parent = currentNode;

					if (!openSet.Contains(neighbour))
					{
						openSet.Add(neighbour);
					}
				}
			}
		}
	}

	public Node GetClosestNode(Vector3 worldPos)
	{
		Node closest = graph.nodes[0];

		foreach (var node in graph.nodes)
		{
			if (GetDistance(node, worldPos) <= GetDistance(closest, worldPos))
			{
				closest = node;
			}
		}

		return closest;
	}

	void RetracePath(Node startNode, Node endNode)
	{
		//UnityEngine.Debug.Log("Retracing Path...");

		List<Node> path = new List<Node>();
		Node currentNode = endNode;

		while (currentNode != startNode)
		{
			path.Add(currentNode);
			currentNode = currentNode.parent;
		}

		path.Add(startNode);

		path.Reverse();

		Path = path;
	}

    public float GetDistance(Node nodeA, Node nodeB)
	{
		Vector3 direction = new Vector3
			(
				nodeA.GetPosition().x - nodeB.GetPosition().x,
				nodeA.GetPosition().y - nodeB.GetPosition().y,
				nodeA.GetPosition().z - nodeB.GetPosition().z
			);

		float distance = direction.magnitude;

		return distance;
	}

	public float GetDistance(Node node, Vector3 pos)
	{
		Vector3 direction = new Vector3
			(
				node.GetPosition().x - pos.x,
				node.GetPosition().y - pos.y,
				node.GetPosition().z - pos.z
			);

		float distance = direction.magnitude;

		return distance;
	}

    //fuction visually represnts what the algorithm is doing 
	void OnDrawGizmos()
	{
		if (onlyPathGizmos)
		{
			Gizmos.color = Color.cyan;

			foreach (var node in graph.nodes)
			{
                List<Transform> targets = new List<Transform>();

                for (int u = 0; u < node.connections.Count; u++)
                {
                    if (u == (node.connections.Count - 1))
                    {
                        //targets.Add(transform);
                        Gizmos.DrawLine(node.GetPosition(), node.connections[0].GetPosition());
                    }
                    else
                    {
                        List<Node> subnodes = node.connections[u].GetSubnodes(node.connections[u + 1]);

                        for (int y = 1; y < subnodes.Count; y++)
                        {
                            Gizmos.DrawLine(subnodes[y - 1].GetPosition(), subnodes[y].GetPosition());
                        }

                        //foreach (var subnode in subnodes)
                        //{
                        //    //targets.Add(subnode.transform);
                        //    Gizmos.DrawLine(node.GetPosition(), node.connections[0].GetPosition());
                        //}
                    }
                }

                //foreach (var target in targets)
                //{
                //    Gizmos.DrawLine(node.GetPosition(), connection.GetPosition());
                //}

    //            foreach (var connection in node.connections)
				//{
    //                List<Node> subnodes = node.GetSubnodes(connection);







    //                foreach (var subnode in subnodes)
    //                {
    //                    Gizmos.DrawLine(node.GetPosition(), connection.GetPosition());
    //                }

    //                //Gizmos.DrawLine(node.GetPosition(), connection.GetPosition());
				//}
			}

			Gizmos.color = Color.red;

			int i = 0;

			foreach (var node in Path)
			{
				if (i < Path.Count - 1)
				{
					Gizmos.DrawLine(node.GetPosition(), Path [i + 1].GetPosition());	
				}

				i++;
			}

			Gizmos.color = Color.green;

			Gizmos.DrawLine(sp, snp);

			Gizmos.color = Color.magenta;

			Gizmos.DrawLine(ep, enp);
		}
	}

}
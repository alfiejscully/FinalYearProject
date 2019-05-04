using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

public class Pathfinder : MonoBehaviour
{
    Node move;
    //transforms for the start and end position 
    public Transform seeker;

    //allows the pathfinder to add-on a graph
    public Graph graph;

    //stores the nodes in path
    public List<Node> Path;

    //gizmos 
    private bool onlyPathGizmos;
    //used for the gizmos start point
    Vector3 snp;
    Vector3 sp;
    //used for the gizmos end point
    Vector3 enp;
    Vector3 ep;

    Transform target;

    void Start()
	{
        target = FindNewTarget();

        //find the path from start to finish 
        FindPath(seeker.position, target.position);
    }

	void FixedUpdate()
	{
        //floating points to find the range of nearest node for the seeker
        float x = Mathf.Abs((seeker.position.x - target.position.x));
        float y = Mathf.Abs((seeker.position.y - target.position.y));
        float z = Mathf.Abs((seeker.position.z - target.position.z));

        //stores the speed value from movement script
        float speed = seeker.GetComponent<Movement>().speed;

        if ((x < 5.0f) &&
            (y < 50.0f) && //works on the scale range for the mesh 
            (z < 5.0f))
        {
            //stores the new target
            target = FindNewTarget();

            UnityEngine.Debug.Log("Working");

            //stores new start and end nodes position
            Node newStartNode = GetClosestNode(seeker.position);
            Node newEndNode = GetClosestNode(target.position);

            //finds the path between the seeker and target positions 
            FindPath(seeker.position, target.position);

            //retraces the path between the start and end node
            RetracePath(newStartNode, newEndNode);

            //set to true
            onlyPathGizmos = true;
            seeker.GetComponent<Movement>().Pathfind = true;
        }
    }

	public void FindPath(Vector3 startPos, Vector3 endPos)
	{
        //Instantiates a new stopwatch 
        Stopwatch testing = new Stopwatch();
        //Begin timing
        testing.Start();

        //stores start and end nodes position
        Node startNode = GetClosestNode(startPos);
		Node endNode = GetClosestNode(endPos);

        //For gizmos use 
		snp = startNode.GetPosition();
		sp = startPos;
		enp = endNode.GetPosition();
		ep = endPos;

		Heap<Node> openSet = new Heap<Node>(graph.nodes.Count);
		HashSet<Node> closedSet = new HashSet<Node>();

        //adds the start node to open set
		openSet.Add(startNode);

        //will loop while the openset amount has a node in it 
		while (openSet.Count > 0)
		{
			Node currentNode = openSet.RemoveFirst();

			closedSet.Add(currentNode);

            //Path
			if (currentNode == endNode)
			{
                //Stop timing.
                testing.Stop();

                //Prints result of how fast the path is generated 
                print("Path Found: " + testing.ElapsedMilliseconds + "ms");

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

    public Transform FindNewTarget()
    {
        //min index is set to 0
        int min = 0;
        //max stores the amount of nodes in the list
        int max = graph.nodes.Count;

        //rand stores unity's randomizer range system
        float rand = UnityEngine.Random.Range(min, max);

        //rand is converted to an int
        int index = (int)rand;

        //targets the node in the index
        Node targetNode = graph.nodes[index];

        //return the node transform 
        return targetNode.GetComponent<Transform>();
    }


    public Node GetClosestNode(Vector3 worldPos)
	{
        //stores the first node in the graphs index that is closest 
		Node closest = graph.nodes[0];
        
        //Looks at the nodes stores in the list
		foreach (var node in graph.nodes)
		{
            //gets the distance between the closest node if its less or equal to the world position 
            if (GetDistance(node, worldPos) <= GetDistance(closest, worldPos))
			{
                //stores node as closest 
				closest = node;
			}
		}

        //returns the closest node
		return closest;
	}

	void RetracePath(Node startNode, Node endNode)
	{
        //stores path in list
		List<Node> path = new List<Node>();
        //end node is stores in current 
		Node currentNode = endNode;

        //loops when current is not equal to the start node
		while (currentNode != startNode)
		{
            //adds current to the list
			path.Add(currentNode);
            //current becomes the parent 
			currentNode = currentNode.parent;
		}

        //adds the start node to the list
		path.Add(startNode);

        //Reverses through the list 
		path.Reverse();

        //path is stores in the pathfinder
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

    //fuction visually represents what the algorithm is doing 
	void OnDrawGizmos()
	{
		if (onlyPathGizmos)
		{
            Gizmos.color = Color.cyan;

            foreach (var node in graph.nodes)
            {
                //List<Transform> targets = new List<Transform>();

                //for (int u = 0; u < node.connections.Count; u++)
                //{
                //    if (u == (node.connections.Count - 1))
                //    {
                //        targets.Add(transform);
                //        Gizmos.DrawLine(node.GetPosition(), node.connections[0].GetPosition());
                //    }
                //    else
                //    {
                //            List<Node> nodez = node.connections[u].GetSubnodes(node.connections[u + 1]);

                //            for (int y = 1; y < nodez.Count; y++)
                //            {
                //                Gizmos.DrawLine(nodez[y - 1].GetPosition(), nodez[y].GetPosition());
                //            }

                //    }                                           
                //}
                foreach (var connection in node.connections)
                {
                    Gizmos.DrawLine(node.GetPosition(), connection.GetPosition());
                }
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

            Gizmos.color = Color.blue;

            List<Node> subnodes = new List<Node>();

            for (int y = 0; y < Path.Count; y++)
            {
                if (y < Path.Count - 1)
                {
                    List<Node> currentSubNodes = Path[y].GetSubnodes(Path[y + 1]);

                    subnodes.AddRange(currentSubNodes);
                }
            }

            for (int t = 0; t < subnodes.Count; t++)
            {
                if (t < subnodes.Count - 1)
                {
                    Gizmos.DrawLine(subnodes[t].GetPosition(), subnodes[t + 1].GetPosition());
                }
            }

            Gizmos.color = Color.yellow;

            Vector3[] positions = Path[0].DrawQuadCurve(10, new Vector3(0.0f, 0.0f, 0.0f), new Vector3(1.0f, 0.0f, 0.0f), new Vector3(2.0f, 0.0f, 0.0f));

            for (int g = 0; g < positions.Length; g++)
            {
                if (g < positions.Length - 1)
                {
                    Gizmos.DrawLine(positions[g], positions[g + 1]);
                }
            }

            //foreach (var subnode in subnodes)
            //{
            //    Destroy(subnode);
            //}

            Gizmos.color = Color.green;

			Gizmos.DrawLine(sp, snp);

			Gizmos.color = Color.magenta;

			Gizmos.DrawLine(ep, enp);
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
	public List<Transform> targets;
	public float speed = 5.0f;

	public Pathfinder Pathfinder;

	private int current = 0;

	public bool Pathfind = false;

	bool GotPath = false;

	void Update ()
	{
		//targets.Clear();

		if (Pathfind)
		{
			if (!GotPath)
			{
				List<Node> path = Pathfinder.Path;

                int current = 0;

				foreach (var node in path)
				{
                    List<Node> subnodes = node.GetSubnodes(path[current + 1]);

                    foreach (var subnode in subnodes)
                    {
                        targets.Add(subnode.transform);
                    }

                    //targets.Add(node.transform);

                    ++current;
				}

				GotPath = true;
			}

			float x = Mathf.Abs((transform.position.x - targets[current].position.x));
			float y = Mathf.Abs((transform.position.y - targets[current].position.y));
			float z = Mathf.Abs((transform.position.z - targets[current].position.z));

			if ((x < 0.1f) &&
				(y < 1.0f) && //works on the scale of the mesh for example if player is scle of 1 then set to 1, need to improve this
				(z < 0.1f))
			{
				current = ++current;
			}
			else
			{
				Vector3 pos = Vector3.MoveTowards(transform.position, targets[current].position, speed * Time.deltaTime);
				GetComponent<Rigidbody>().MovePosition(pos);
			}
		}
	}
}

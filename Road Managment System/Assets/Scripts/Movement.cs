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

    private List<Node> path;


    void Update ()
	{
		//targets.Clear();

		if (Pathfind)
		{
			if (!GotPath)
			{
				path = Pathfinder.Path;

                for (int i = 0; i < path.Count; i++)
                {
                    if (i == (path.Count - 1))
                    {
                        targets.Add(transform);
                    }
                    else
                    {
                        List<Node> subnodes = path[i].GetSubnodes(path[i + 1]);

                        foreach (var subnode in subnodes)
                        {
                            targets.Add(subnode.transform);
                        }
                    }
                }

				GotPath = true;
			}

           // if (path.Count > 0 && current < path.Count - 1)
           // {
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
           // }
		}
	}
}

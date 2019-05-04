using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

	public List<Transform> targets;

	public float speed = 25.0f;

	public Pathfinder Pathfinder;

	private int current = 0;

	public bool Pathfind = false;

	bool GotPath = false;

    private List<Node> path;

    void Update()
    {
        
    }
    void FixedUpdate ()
	{
		
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

            if (current < targets.Count - 1)
            {
                //finding the differences of the transform and target posistions for X,Y,Z
                float x = Mathf.Abs((transform.position.x - targets[current].position.x));
                float y = Mathf.Abs((transform.position.y - targets[current].position.y));
                float z = Mathf.Abs((transform.position.z - targets[current].position.z));

                if ((x < speed * Time.fixedDeltaTime) &&
                    (y < 50.0f) && //works on the scale range for the mesh 
                    (z < speed * Time.fixedDeltaTime))
                {
                    current = ++current;

                    Vector3 direction = targets[current].position - transform.position;
                    Quaternion rotation = Quaternion.LookRotation(direction);
                    transform.rotation = rotation;

                    //Quaternion targetRotate = Quaternion.LookRotation(targets[current].position - transform.position);
                    //transform.rotation = Quaternion.Lerp(transform.rotation, targetRotate, Time.fixedDeltaTime * speed);
                }
                else
                {
                    Vector3 pos = Vector3.MoveTowards(transform.position, targets[current].position, speed * Time.fixedDeltaTime);
                    GetComponent<Rigidbody>().MovePosition(pos);

                }
            }
            else
            {
                GotPath = false;
            }
		}
	}
}

// LookAt to keep the player moving in the forward direction

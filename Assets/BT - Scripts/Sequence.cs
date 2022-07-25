using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : Node
{
    // Start is called before the first frame update
    public Sequence(string n)
    {
        name = n;
    }

    public override Status Process()
    {
        Status childstatus = children[currentChildren].Process();

        if (childstatus == Status.RUNNING)
            return Status.RUNNING;
        if (childstatus == Status.FAILURE)
        {
            currentChildren = 0;
            foreach (Node n in children)
            {
                n.Reset();
            }
            return Status.FAILURE;

        }
            
        currentChildren++;
        if(currentChildren >= children.Count)
        {
            currentChildren = 0;
            return Status.SUCESS;
        }
        return Status.RUNNING;
    }
}

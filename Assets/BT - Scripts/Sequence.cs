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
            return childstatus;

        currentChildren++;
        if(currentChildren >= children.Count)
        {
            currentChildren = 0;
            return Status.SUCESS;
        }
        return Status.RUNNING;
    }
}

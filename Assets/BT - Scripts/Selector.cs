using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : Node
{
    public Selector(string n)
    {
        name = n;
    }

    public override Status Process()
    {
        Status childstatus = children[currentChildren].Process();

        if (childstatus == Status.RUNNING)
            return Status.RUNNING;
        if (childstatus == Status.SUCESS)
        {
            currentChildren = 0;
            return Status.SUCESS;
        }

        currentChildren++;
        if (currentChildren >= children.Count)
        {
            currentChildren = 0;
            return Status.FAILURE;
        }
        return Status.RUNNING;
    }
}

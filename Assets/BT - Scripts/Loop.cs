using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loop : Node
{
    BehaviourTree dependancy;
    // Start is called before the first frame update
    public Loop(string n, BehaviourTree d)
    {
        name = n;
        dependancy = d;
    }

    public override Status Process()
    {
        if(dependancy.Process() == Status.FAILURE)
        {
            return Status.SUCESS;
        }
        
        Status childstatus = children[currentChildren].Process();

        if (childstatus == Status.RUNNING)
            return Status.RUNNING;
        if (childstatus == Status.FAILURE)
        {

            return childstatus;

        }

        currentChildren++;
        if (currentChildren >= children.Count)
        {
            currentChildren = 0;
            
        }
        return Status.RUNNING;
    }
}

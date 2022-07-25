using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inverter : Node
{
    // Start is called before the first frame update
    public Inverter(string n)
    {
        name = n;
    }

    public override Status Process()
    {
        Status childstatus = children[0].Process();

        if (childstatus == Status.RUNNING)
            return Status.RUNNING;
        if (childstatus == Status.FAILURE)
            return Status.SUCESS;
        else
            return Status.FAILURE;
        
    }
}

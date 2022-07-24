using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public enum Status { SUCESS, RUNNING, FAILURE};
    public Status status;
    public List<Node> children = new List<Node>();
    public int currentChildren = 0;
    public string name;

    public Node() { }

    public Node(string n)
    {
        name = n;
    }

    public virtual Status Process()
    {
        return children[currentChildren].Process();
    }

    public void AddChild(Node n)
    {
        children.Add(n);
    }
}
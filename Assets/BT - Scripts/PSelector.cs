using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Priorizer
public class PSelector : Node
{
    Node[] nodeArray;
    bool order = false;
    public PSelector(string n)
    {
        name = n;
    }

    void OrderNodes()
    {
        nodeArray = children.ToArray();
        Sort(nodeArray, 0, children.Count - 1);
        children = new List<Node>(nodeArray);
    }

    public override Status Process()
    {
        if (!order)
        {
            OrderNodes();
            order = true;
        }
        
        Status childstatus = children[currentChildren].Process();
        if (childstatus == Status.RUNNING)
            return Status.RUNNING;
        if (childstatus == Status.SUCESS)
        {
            //children[currentChildren].sortOrder = 1;
            currentChildren = 0;
            order = false;
            return Status.SUCESS;
        }
        //else
        //{
            //children[currentChildren].sortOrder = 10;
        //}
        currentChildren++;
        if (currentChildren >= children.Count)
        {
            currentChildren = 0;
            order = false;
            return Status.FAILURE;
        }
        return Status.RUNNING;
    }
    int Partition(Node[] array, int low, int high)
    {
        Node pivot = array[high];
        int lowIndex = (low - 1);

        for (int j = low; j < high; j++)
        {
            if (array[j].sortOrder <= pivot.sortOrder)
            {
                lowIndex++;
                Node temp = array[lowIndex];
                array[lowIndex] = array[j];
                array[j] = temp;
            }
        }
        Node temp1 = array[lowIndex + 1];
        array[lowIndex + 1] = array[high];
        array[high] = temp1;
        return lowIndex + 1;
    }
    void Sort(Node[] array, int low, int high)
    {
        if (low < high)
        {
            int partitionIndex = Partition(array, low, high);
            Sort(array, low, partitionIndex - 1);
            Sort(array, partitionIndex + 1, high);
        }
    }
}

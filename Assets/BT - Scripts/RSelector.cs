using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Randomizer
public class RSelector : Node
{
    Node[] nodeArray;
    bool shuffle = false;
    public RSelector(string n)
    {
        name = n;
    }

    public override Status Process()
    {
        if (!shuffle) {
            children.Shuffle();
            shuffle = true;
        }
        

        Status childstatus = children[currentChildren].Process();

        if (childstatus == Status.RUNNING)
            return Status.RUNNING;
        if (childstatus == Status.SUCESS)
        {
            //children[currentChildren].sortOrder = 1;
            currentChildren = 0;
            shuffle = false;
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
            shuffle = false;
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

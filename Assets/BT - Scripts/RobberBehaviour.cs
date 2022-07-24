using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobberBehaviour : MonoBehaviour
{
    BehaviourTree tree;
    public GameObject diamond;
    public GameObject backdoor;
    public GameObject frontdoor;
    public GameObject van;
    NavMeshAgent agent;

    public enum ActionState { IDLE, WORKING};
    ActionState state = ActionState.IDLE;

    Node.Status treeStatus = Node.Status.RUNNING;

    [Range(0, 1000)]
    public int money = 400;

    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        
        tree = new BehaviourTree();
        Sequence steal = new Sequence("Steal Somenthing");
        Leaf goToDiamond = new Leaf("Go To Diamond", GoToDiamond);
        Leaf hasGotMoney = new Leaf("Has Got Money", HasMoney);
        Leaf goToBackDoor = new Leaf("Go To BackDoor", GoToBackDoor);
        Leaf goToFrontDoor = new Leaf("Go To FrontDoor", GoToFrontDoor);
        Leaf goToVan = new Leaf("Go To Van", GoToVan);
        Selector OpenDoors = new Selector("Open Door");

        OpenDoors.AddChild(goToFrontDoor);
        OpenDoors.AddChild(goToBackDoor);

        steal.AddChild(hasGotMoney);
        steal.AddChild(OpenDoors);
        steal.AddChild(goToDiamond);
        //steal.AddChild(goToFrontDoor);
        steal.AddChild(goToVan);
        tree.AddChild(steal);

        tree.PrintTree();
    }
    public Node.Status HasMoney()
    {
        if (money >= 500)
            return Node.Status.FAILURE;
        return Node.Status.SUCESS;
    }
    public Node.Status GoToDiamond()
    {
        Node.Status s =  GoToLocation(diamond.transform.position);
        if (s == Node.Status.SUCESS)
        {
            diamond.transform.parent = this.gameObject.transform;
        }
        return s;
    }
    public Node.Status GoToBackDoor()
    {
        return GoToDoor(backdoor);
    }
    public Node.Status GoToFrontDoor()
    {
        return GoToDoor(frontdoor);
    }
    public Node.Status GoToVan()
    {
        Node.Status s = GoToLocation(van.transform.position);
        if (s == Node.Status.SUCESS)
        {
            money += 300;
            diamond.SetActive(false);
        }
        return s;
    }

    public Node.Status GoToDoor(GameObject door)
    {
        Node.Status s = GoToLocation(door.transform.position);
        if (s == Node.Status.SUCESS)
        {
            if (!door.GetComponent<Lock>().isLocked)
            {
                door.SetActive(false);
                return Node.Status.SUCESS;
            }
            return Node.Status.FAILURE;
        }
        else
            return s;
    }
    
    Node.Status GoToLocation(Vector3 destination)
    {
        float distanceToTarget = Vector3.Distance(destination, this.transform.position);
        if(state == ActionState.IDLE)
        {
            agent.SetDestination(destination);
            state = ActionState.WORKING;
        }
        else if(Vector3.Distance(agent.pathEndPosition, destination) >= 2)
        {
            state = ActionState.IDLE;
            return Node.Status.FAILURE;
        }
        else if (distanceToTarget < 2)
        {
            state = ActionState.IDLE;
            return Node.Status.SUCESS;
        }
        return Node.Status.RUNNING;
    }

    void Update()
    {
        if(treeStatus != Node.Status.SUCESS)
            treeStatus = tree.Process();
    }
}

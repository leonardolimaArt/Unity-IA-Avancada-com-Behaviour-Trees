using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobberBehaviour : BTAgente
{
    
    public GameObject diamond;
    public GameObject[] painting;
    public GameObject backdoor;
    public GameObject frontdoor;
    public GameObject van;
    GameObject pickup;
    int r;

    Leaf goToBackDoor;
    Leaf goToFrontDoor;

       [Range(0, 1000)]
    public int money = 400;

    new void Start()
    {
        base.Start();

        painting = GameObject.FindGameObjectsWithTag("Painting");
        r = Random.Range(0, painting.Length);

        Sequence steal = new Sequence("Steal Somenthing");
        
        Leaf goToDiamond = new Leaf("Go To Diamond", GoToDiamond, 1);
        Leaf goToPainting = new Leaf("Go To Painting", GoToPainting, 2);
        Leaf hasGotMoney = new Leaf("Has Got Money", HasMoney);
        goToBackDoor = new Leaf("Go To BackDoor", GoToBackDoor, 2);
        goToFrontDoor = new Leaf("Go To FrontDoor", GoToFrontDoor, 1);
        Leaf goToVan = new Leaf("Go To Van", GoToVan);
        
        PSelector OpenDoors = new PSelector("Open Door");
        PSelector selectObject = new PSelector("Select Object to Steal");

        Inverter invertMoney = new Inverter("Invert Money");

        

        invertMoney.AddChild(hasGotMoney);

        OpenDoors.AddChild(goToFrontDoor);
        OpenDoors.AddChild(goToBackDoor);

        steal.AddChild(invertMoney);
        steal.AddChild(OpenDoors);

        selectObject.AddChild(goToDiamond);
        selectObject.AddChild(goToPainting);

        steal.AddChild(selectObject);
        //steal.AddChild(goToFrontDoor);
        steal.AddChild(goToVan);
        tree.AddChild(steal);

        tree.PrintTree();
    }
    public Node.Status HasMoney()
    {
        if (money < 500)
            return Node.Status.FAILURE;
        return Node.Status.SUCESS;
    }
    public Node.Status GoToDiamond()
    {
        if (!diamond.activeSelf) return Node.Status.FAILURE;
        Node.Status s =  GoToLocation(diamond.transform.position);
        if (s == Node.Status.SUCESS)
        {
            diamond.transform.parent = this.gameObject.transform;
            pickup = diamond;
        }
        return s;
    }
    public Node.Status GoToPainting()
    {
        if (!painting[r].activeSelf) return Node.Status.FAILURE;
        Node.Status s = GoToLocation(painting[r].transform.position);
        if (s == Node.Status.SUCESS)
        {
            painting[r].transform.parent = this.gameObject.transform;
            pickup = painting[r];
        }
        return s;
    }

    public Node.Status GoToArt(int i)
    {
        if (!painting[i].activeSelf) return Node.Status.FAILURE;
        Node.Status s = GoToLocation(painting[i].transform.position);
        if (s == Node.Status.SUCESS)
        {
            painting[i].transform.parent = this.gameObject.transform;
            pickup = painting[i];
        }
        return s;
    }
    public Node.Status GoToBackDoor()
    {
        Node.Status s = GoToDoor(backdoor);
        if (s == Node.Status.FAILURE)
            goToBackDoor.sortOrder = 10;
        else
            goToBackDoor.sortOrder = 1;
        return s;
    }
    public Node.Status GoToFrontDoor()
    {
        Node.Status s = GoToDoor(frontdoor);
        if (s == Node.Status.FAILURE)
            goToFrontDoor.sortOrder = 10;
        else
            goToFrontDoor.sortOrder = 1;
        return s;
    }
    public Node.Status GoToVan()
    {
        Node.Status s = GoToLocation(van.transform.position);
        if (s == Node.Status.SUCESS)
        {
            money += 300;
            pickup.SetActive(false);
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
                door.GetComponent<NavMeshObstacle>().enabled = false;
                return Node.Status.SUCESS;
            }
            return Node.Status.FAILURE;
        }
        else
            return s;
    }
    
    new Node.Status GoToLocation(Vector3 destination)
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
}

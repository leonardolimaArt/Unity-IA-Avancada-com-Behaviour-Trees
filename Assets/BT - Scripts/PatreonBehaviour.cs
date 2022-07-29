using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatreonBehaviour : BTAgent
{

    public GameObject[] art;
    public GameObject frontdoor;
    public GameObject home;


    [Range(0, 1000)]
    public int boredom = 1000;

    new void Start()
    {
        base.Start();

        art = GameObject.FindGameObjectsWithTag("Painting");

        RSelector selectObject = new RSelector("Select Art to View");
        for (int i = 0; i < art.Length; i++)
        {
            Leaf gta = new Leaf("Go to " + art[i].name, i, GoToArt);
            selectObject.AddChild(gta);
        }
        Leaf goToFrontDoor = new Leaf("Go To Front Door", GoToFrontDoor);
        Leaf goHome = new Leaf("Go Home", GoHome);
        Leaf isBored = new Leaf("Is Bored?", IsBored);

        Sequence viewArt = new Sequence("View Art");

        viewArt.AddChild(isBored);
        viewArt.AddChild(goToFrontDoor);


        BehaviourTree whileBored = new BehaviourTree();
        whileBored.AddChild(isBored);

        Loop lookAtPainting = new Loop("Look", whileBored);
        lookAtPainting.AddChild(selectObject);

        viewArt.AddChild(lookAtPainting);

        viewArt.AddChild(goHome);

        Selector bePatron = new Selector("Be An Art Patron");
        bePatron.AddChild(viewArt);

        tree.AddChild(bePatron);

        StartCoroutine("IncreaseBoredom");

    }

    IEnumerator IncreaseBoredom()
    {
        while (true)
        {
            boredom = Mathf.Clamp(boredom + 20, 0, 1000);
            yield return new WaitForSeconds(Random.Range(1, 5));
        }
    }


    public Node.Status GoToFrontDoor()
    {
        Node.Status s = GoToDoor(frontdoor);
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

    public Node.Status GoToArt(int i)
    {
        if (!art[i].activeSelf) return Node.Status.FAILURE;
        Node.Status s = GoToLocation(art[i].transform.position);

        if(s == Node.Status.SUCESS)
        {
            boredom = Mathf.Clamp(boredom - 50, 0, 1000);
        }
        return s;
    }

    public Node.Status GoHome()
    {
        Node.Status s = GoToLocation(home.transform.position);
        return s;
    }

    public Node.Status IsBored()
    {
        if(boredom < 100)
        {
            return Node.Status.FAILURE;
        }
        else
        {
            return Node.Status.SUCESS;
        }
    }
}

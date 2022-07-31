using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Blackboard : MonoBehaviour {

    public float timeOfDay;
    public Text clock;
    public Text dis;
    public Stack<GameObject> patrons = new Stack<GameObject>();
    public int openTime = 6;
    public int closeTime = 20;
    public GameObject robber;
    public GameObject frontDoor;
    public GameObject backDoor;
    public GameObject objDoor1;
    public GameObject objDoor2;
    public GameObject objDoor3;
    public GameObject sunLight;
    public GameObject nightLight;

    static Blackboard instance;
    public static Blackboard Instance {

        get {

            if (!instance) {

                Blackboard[] blackboards = GameObject.FindObjectsOfType<Blackboard>();
                if (blackboards != null) {

                    if (blackboards.Length == 1) {

                        instance = blackboards[0];
                        return instance;
                    }
                }
                GameObject go = new GameObject("Blackboard", typeof(Blackboard));
                instance = go.GetComponent<Blackboard>();
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
        set {

            instance = value as Blackboard;
        }
    }

    void Start() {

        StartCoroutine("UpdateClock");
        
    }

    IEnumerator UpdateClock() {

        while (true) {

            timeOfDay++;
            if (timeOfDay > 23) timeOfDay = 0;
            clock.text = timeOfDay.ToString("00") + ":00";

            if (timeOfDay == closeTime) {

                patrons.Clear();
            }

            yield return new WaitForSeconds(5.0f);
        }
    }

    public bool RegisterPatron(GameObject p) {


        patrons.Push(p);
        return true;
    }

    public void DeristerPatron() {

        // patron = null;
    }

    
    public void Update()
    {
        if (timeOfDay > closeTime || timeOfDay < openTime)
        {
            frontDoor.GetComponent<Lock>().isLocked = true;
            objDoor1.transform.localRotation = Quaternion.Lerp(objDoor1.transform.localRotation, Quaternion.Euler(0, 0, 0), 2f * Time.deltaTime);
            objDoor2.transform.localRotation = Quaternion.Lerp(objDoor2.transform.localRotation, Quaternion.Euler(0, 180, 0), 2f * Time.deltaTime);
            sunLight.SetActive(false);
            nightLight.SetActive(true);
            frontDoor.GetComponent<NavMeshObstacle>().enabled = true;
        }
        else
        {
            frontDoor.GetComponent<Lock>().isLocked = false;
            objDoor1.transform.localRotation = Quaternion.Lerp(objDoor1.transform.localRotation, Quaternion.Euler(0, 0, 90f), 2f * Time.deltaTime);
            objDoor2.transform.localRotation = Quaternion.Lerp(objDoor2.transform.localRotation, Quaternion.Euler(0, 180, 90f), 2f * Time.deltaTime); ;
            sunLight.SetActive(true);
            nightLight.SetActive(false);
            backDoor.GetComponent<NavMeshObstacle>().enabled = true;
        }
        float dist = Vector3.Distance(robber.transform.position, objDoor3.transform.position);
        dis.text = dist.ToString();
        if (dist <= 10)
        {
            objDoor3.transform.localRotation = Quaternion.Lerp(objDoor3.transform.localRotation, Quaternion.Euler(0, 0, 0), 2f * Time.deltaTime); ;
        }
        else
        {
            objDoor3.transform.localRotation = Quaternion.Lerp(objDoor3.transform.localRotation, Quaternion.Euler(0, 0, 90f), 2f * Time.deltaTime); ;
        }
    }
}

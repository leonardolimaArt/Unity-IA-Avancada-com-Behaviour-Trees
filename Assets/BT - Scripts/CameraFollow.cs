using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform Target;
    public float distance;
    

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(Target.transform.position.x - distance, Target.transform.position.y + distance, Target.transform.position.z - distance);
        transform.LookAt(Target);
    }
}

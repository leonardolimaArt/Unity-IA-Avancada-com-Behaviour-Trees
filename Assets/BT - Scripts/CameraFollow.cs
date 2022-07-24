using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform Target;
    
    [Range(0,10)]
    public int distance;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(Target.position.x - distance, Target.position.y + distance, Target.position.z - distance);
        transform.LookAt(Target);

    }
}

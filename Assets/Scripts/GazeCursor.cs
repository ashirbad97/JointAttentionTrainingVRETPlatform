using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fove.Unity;
public class GazeCursor : MonoBehaviour
{
    [SerializeField]
    FoveInterface foveInterface;
    public static string currentGazedObject;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //print("combined ray "+ foveInterface.GetCombinedGazeRay().value.direction);
        //Depth Value is not accurate
        //print("deptjh " + foveInterface.GetCombinedGazeDepth().value);
        //transform.position = foveInterface.GetCombinedGazeRay().value.GetPoint(1);

        RaycastHit hit;
        
        if (Physics.Raycast(foveInterface.GetCombinedGazeRay().value, out hit, 100))
        {
           //print("GazeCursor Hit Collider " + hit.transform.name);
           currentGazedObject = hit.transform.name;
           transform.position = hit.point;
        }
    }
}

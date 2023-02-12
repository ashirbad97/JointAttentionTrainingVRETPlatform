using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fove.Unity;
// Add a layer to the GazeCursor such that it is being ignored in the raycasting layer
public class GazeCursor : MonoBehaviour
{
    [SerializeField]
    FoveInterface foveInterface;
    public static string currentGazedObject;
    LayerMask layerMask;
    // Start is called before the first frame update
    void Start()
    {
        currentGazedObject = null;
        layerMask = ~(1 << 3);//LayerMask for ignoring the realtime ball
    }

    // Update is called once per frame
    void Update()
    {
        //print("combined ray "+ foveInterface.GetCombinedGazeRay().value.direction);
        //Depth Value is not accurate
        //print("deptjh " + foveInterface.GetCombinedGazeDepth().value);
        //transform.position = foveInterface.GetCombinedGazeRay().value.GetPoint(1);

        RaycastHit hit;
        // Debug.Log("Fove Gaze Cursor value: " + foveInterface.GetCombinedGazeRay().value);
        // Condition if the ray being casted using the combined Fove Ray which is being thrown at a distant of z=100.
        //N.B: Condition is true only if there is a ray hit with a collider
        if (Physics.Raycast(foveInterface.GetCombinedGazeRay().value, out hit, 100, layerMask))// throws the ray at a distance of 100 mts and pass it to the hit, 
        // N.B: Ignore the layermask of the realtime cursor
        {
            // print("GazeCursor Hit Collider " + hit.transform.name);
            currentGazedObject = hit.transform.name;
            transform.position = hit.point;
        }
        //N.B: If ray is not hitting on any collider
        else
        {
            currentGazedObject = null;
        }
    }
}

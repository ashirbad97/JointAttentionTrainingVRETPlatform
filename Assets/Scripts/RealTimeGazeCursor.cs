using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fove.Unity;
public class RealTimeGazeCursor : MonoBehaviour
{
    [SerializeField]
    FoveInterface foveInterface;
    [SerializeField] GameObject realTimeBallCursor;
    [SerializeField]
    int assumeZDepthVal;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        realTimeBallCursor.transform.position = foveInterface.GetCombinedGazeRay().value.GetPoint(assumeZDepthVal);
    }
}

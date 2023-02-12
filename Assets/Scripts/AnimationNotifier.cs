using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationNotifier : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LeftFingerStayEnded()
    {
        Debug.Log("left finger stay");
    }
    public void RightFingerStayEnded()
    {
        Debug.Log("right finger stay");
    }
    public void LeftFingerEnded()
    {
        Debug.Log("right finger");
    }
    public void RightFingerEnded()
    {
        Debug.Log("right finger ");
    }
}

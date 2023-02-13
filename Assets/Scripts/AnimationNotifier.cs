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
        DataDumper.DumpHandCueEndTime("LeftFingerStay");
    }
    public void RightFingerStayEnded()
    {
        DataDumper.DumpHandCueEndTime("RightFingerStay");
    }
    public void LeftFingerEnded()
    {
        DataDumper.DumpHandCueEndTime("LeftFinger");
    }
    public void RightFingerEnded()
    {
        DataDumper.DumpHandCueEndTime("RightFinger");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ArmTargetPlacement : MonoBehaviour
{
    [SerializeField]
    GameObject targetBall;
    [SerializeField]
    GameObject targetHand;
    [SerializeField]
    Animator characterAnimator;

    Vector3 startPoint;
    Vector3 targetBallPoint;
    float oneHandDist;
    Vector3 handTargetPos;
    
    // Start is called before the first frame update
    void Start()
    {
        //Subscription of ExperimentController Events with the respective functions
        ExperimentController.OnObjectTrackedInLeftDir += PlaceTargetHandToLeft;
        ExperimentController.OnObjectTrackedInRightDir += PlaceTargetHandToRight;
        oneHandDist = 0.7f;
        startPoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaceTargetHandToRight(){
        targetBallPoint = targetBall.transform.position;
        Vector3 dir = targetBallPoint - transform.position;
        float dirLength = dir.magnitude;
        Vector3 normalizedDir = dir/dirLength;
        handTargetPos = transform.position + normalizedDir*oneHandDist;
       // print("handTargetPos "+handTargetPos);
        targetHand.transform.position = handTargetPos; 
        characterAnimator.SetTrigger("PointRight");
    }

    public void PlaceTargetHandToLeft(){
        targetBallPoint = targetBall.transform.position;
        Vector3 dir = targetBallPoint - transform.position;
        float dirLength = dir.magnitude;
        Vector3 normalizedDir = dir/dirLength;
        handTargetPos = transform.position + normalizedDir*oneHandDist;
        //print("handTargetPos "+handTargetPos);
        targetHand.transform.position = handTargetPos; 
        characterAnimator.SetTrigger("PointLeft");
    }

}

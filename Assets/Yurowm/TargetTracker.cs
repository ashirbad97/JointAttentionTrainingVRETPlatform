using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TargetTracker : MonoBehaviour
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
        //characterAnimator = character.transform.GetComponent<Animator>();
        oneHandDist = 0.7f;
        startPoint = transform.position;
        EventManagement.OnObjectTrackedInLeftDir += PlaceTargetHandToLeft;
        EventManagement.OnObjectTrackedInRightDir += PlaceTargetHandToRight;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // public void CalculateTargetPlace(){
    //     print("targetBallPoint "+targetBallPoint);
    //     print("hand pos "+transform.position);
    //     float d = (float)Math.Sqrt( Math.Pow( (targetBallPoint.x-startPoint.x), 2) + Math.Pow( (targetBallPoint.y-startPoint.y), 2) );
    //     print("d "+d);
    //     float t = oneHandDist/d;
    //     handTargetPos.z = startPoint.z - oneHandDist;
    //     handTargetPos.x = (1-t)*startPoint.x + t*targetBallPoint.x;
    //     handTargetPos.y = (1-t)*startPoint.y + t*targetBallPoint.y;
        
    //     targetHand.transform.position = rightArm.transform.InverseTransformPoint(handTargetPos);// handTargetPos;
    //     print("handTargetPos "+handTargetPos);
    //    // characterAnimator.SetTrigger("PointRight");
    // }

    public void PlaceTargetHandToRight(){
        targetBallPoint = targetBall.transform.position;
        Vector3 dir = targetBallPoint - transform.position;
        float dirLength = dir.magnitude;
        Vector3 normalizedDir = dir/dirLength;
        handTargetPos = transform.position + normalizedDir*oneHandDist;
        print("handTargetPos "+handTargetPos);
        targetHand.transform.position = handTargetPos; 
        characterAnimator.SetTrigger("PointRight");
    }

    public void PlaceTargetHandToLeft(){
        targetBallPoint = targetBall.transform.position;
        Vector3 dir = targetBallPoint - transform.position;
        float dirLength = dir.magnitude;
        Vector3 normalizedDir = dir/dirLength;
        handTargetPos = transform.position + normalizedDir*oneHandDist;
        print("handTargetPos "+handTargetPos);
        targetHand.transform.position = handTargetPos; 
        characterAnimator.SetTrigger("PointLeft");
    }

}

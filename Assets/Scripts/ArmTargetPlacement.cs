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
    [SerializeField]
    AnimationClip leftHandAnimation;
    [SerializeField]
    AnimationClip rightHandAnimation;

    Vector3 startPoint;
    Vector3 targetBallPoint;
    float oneHandDist;
    Vector3 handTargetPos;

    void OnEnable()
    {
        //Subscription of ExperimentController Events with the respective functions
        if (gameObject.CompareTag("RightArm"))
        {
            ExperimentController.OnObjectTrackedInRightDir += PlaceTargetHandToRight;
        }
        else if (gameObject.CompareTag("LeftArm"))
        {
            ExperimentController.OnObjectTrackedInLeftDir += PlaceTargetHandToLeft;
        }
        
        
    }
    // Start is called before the first frame update
    void Start()
    {
        oneHandDist = 0.7f;
        startPoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlaceTargetHandToRight(float handAnimDuration, string animName)
    {
        targetBallPoint = targetBall.transform.position;
        Vector3 dir = targetBallPoint - transform.position;
        float dirLength = dir.magnitude;
        Vector3 normalizedDir = dir / dirLength;
        handTargetPos = transform.position + normalizedDir * oneHandDist;
        targetHand.transform.position = handTargetPos;
        characterAnimator.speed = rightHandAnimation.length / handAnimDuration;//Computes the playback speed based on the time
        characterAnimator.SetTrigger(animName);//Triggers to play the animation in the animator (refer the animator window)
        StartCoroutine(MoveHandToTarget(handAnimDuration));
    }

    public void PlaceTargetHandToLeft(float handAnimDuration, string animName)
    {
        targetBallPoint = targetBall.transform.position;
        Vector3 dir = targetBallPoint - transform.position;
        float dirLength = dir.magnitude;
        Vector3 normalizedDir = dir / dirLength;
        handTargetPos = transform.position + normalizedDir * oneHandDist;
        targetHand.transform.position = handTargetPos;
        characterAnimator.speed = leftHandAnimation.length / handAnimDuration;//Computes the playback speed based on the time
        characterAnimator.SetTrigger(animName);//Triggers to play the animation in the animator (refer the animator window)
        StartCoroutine(MoveHandToTarget(handAnimDuration));
    }
    //Allows the response of the user to register the target object after the animation has been played
    //In context allows the head anim to run followed by the hand pointing and then only allow the target Object registration
    IEnumerator MoveHandToTarget(float handAnimDuration)
    {
        yield return new WaitForSeconds(handAnimDuration+1f);//will pause the coroutine unless the condition has beensatisfied
        //Currently Animation is asynchronous, execute below only after animation is complete
        //Turn on the flag for waiting for response after al the cues are delivered
        ExperimentController.waitForResponse = true;
    }

    void OnDisable()
    {
        //UnSubscription of ExperimentController Events with the respective functions
        if (gameObject.CompareTag("RightArm"))
        {
            ExperimentController.OnObjectTrackedInRightDir -= PlaceTargetHandToRight;
        }
        else if (gameObject.CompareTag("LeftArm"))
        {
            ExperimentController.OnObjectTrackedInLeftDir -= PlaceTargetHandToLeft;
        }

    }

}

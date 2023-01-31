using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fove.Unity;
using System;

public class ExperimentController : MonoBehaviour
{
    //Reference to the headTarget object which has to move
    [SerializeField] private GameObject headTarget;
    [SerializeField] private GameObject targetHeadEndPoint;
    [SerializeField] private float timeToMoveHeadTarget;
    [SerializeField] private GameObject initialHeadTargetPosition;
    [SerializeField] private float subjectResponseWaitTime = 3f;
    //Declaring Events for Arm Animation of the Avatar
    public static event Action OnObjectTrackedInRightDir;
    public static event Action OnObjectTrackedInLeftDir;
    public bool waitForResponse = false;
    string currentGazedObject;
    string prevGazedObject;
    float countDownTimer;
    // Start is called before the first frame update
    void Start()
    {
        //Find all the endTarget tagged objects into an array and select one randomly
        GameObject[] endTargets;
        endTargets = GameObject.FindGameObjectsWithTag("endTarget");
        //Randomly select a endTarget
        int randomTargetIndex = UnityEngine.Random.Range(0, endTargets.Length);
        targetHeadEndPoint = endTargets[randomTargetIndex];
        //Find the direction component attached from the script
        string endPointDirection = targetHeadEndPoint.GetComponent<TargetEndPointProperty>().direction.ToString();
        //Fixates the headTarget position to the reference GameObject of initialHeadTargetPosition
        headTarget.transform.position = initialHeadTargetPosition.transform.position;
        //Coroutine to move the headTarget so as to move the avatar head and the Arm Animation
        StartCoroutine(MoveTargetBall(endPointDirection));
    }

    // Update is called once per frame
    void Update()
    {
        if (waitForResponse)
        {
            //Deterimines if any GameObject is being gazed upon, if yes then prints the name of the GameObject
            if (GazeCursor.currentGazedObject != "")
            {
                currentGazedObject = GazeCursor.currentGazedObject;
                //Debug.Log("Current GameObject: " + currentGazedObject);
                //Debug.Log("Previous GameObject: " + prevGazedObject);               
                if ((currentGazedObject == prevGazedObject || prevGazedObject == null) && subjectResponseWaitTime > 0)
                {
                    subjectResponseWaitTime -= Time.deltaTime;
                }
                else if (subjectResponseWaitTime <=0)
                {
                    Debug.Log("User Response is: "+ currentGazedObject);
                    waitForResponse = false;
                    //Debug.Break();
                }
                else
                {
                   subjectResponseWaitTime = subjectResponseWaitTime; //Figure thisout
                }
                prevGazedObject = currentGazedObject;
            }
        }        
    }

    private void FixedUpdate()
    {

    }
    IEnumerator MoveTargetBall(string endPointDirection)
    {
        //Since updates type functions are called each frame, the transform position change per frame, since we are doing it using a Coroutine and based on time rather than frames we manually update the position based on a loop. 
        float elapsedTime = 0;
        while (elapsedTime < timeToMoveHeadTarget)
        {
            headTarget.transform.position = Vector3.Lerp(initialHeadTargetPosition.transform.position, targetHeadEndPoint.transform.position, (elapsedTime / timeToMoveHeadTarget));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        //Bringing manually to the end position coz with Lerp u never reach the end position
        headTarget.transform.position = targetHeadEndPoint.transform.position;
        //At this point the ball has moved to it's targetEndPoint
        //At this point we should start checking if the user is able to Gaze at the correct object or not
        //If we need to mve only the Arm before running this we can set the 
        //Manually hard-coding for the Arm Movement animation to run after the Head Movement
        if (endPointDirection == "Right") OnObjectTrackedInRightDir();
        else OnObjectTrackedInLeftDir();
        //Currently Animation is asynchronous, execute below only after animation is complete
        //Turn on the flag for waiting for response after al the cues are delivered
        waitForResponse = true;
        yield return null;
    }
}

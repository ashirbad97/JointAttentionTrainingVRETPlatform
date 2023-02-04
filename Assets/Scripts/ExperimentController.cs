using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fove.Unity;
using System;
using UnityEngine.SceneManagement;

public class ExperimentController : MonoBehaviour
{
    //Reference to the headTarget object which has to move
    [SerializeField] private GameObject headTarget;
    [SerializeField] private GameObject targetHeadEndPoint;
    [SerializeField] private float timeToMoveHeadTarget;
    [SerializeField] private GameObject initialHeadTargetPosition;
    [SerializeField] private float subjectResponseWaitTime;
    [SerializeField] private float faceFixationWaitTime;
    [SerializeField] private GameObject avatarHeadRegion;
    //Declaring Events for Arm Animation of the Avatar
    public static event Action OnObjectTrackedInRightDir;
    public static event Action OnObjectTrackedInLeftDir;
    public bool waitForEyeContact = true;
    public bool waitForResponse = false;
    string currentGazedObject;
    string prevGazedObject;
    [SerializeField] float avatarFaceGazeCountdownTimer;
    [SerializeField] float targetObjRegisterCountdownTimer;
    string endPointDirection;
    void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Current Trial is: " + ExperimentSettings.currentTrialCount);
        //Assigning parameter variables with settings values from Input Menu
        timeToMoveHeadTarget = ExperimentSettings.cueDeliverySpeed;
        subjectResponseWaitTime = ExperimentSettings.responseRegistrationFixationDuration;
        faceFixationWaitTime = ExperimentSettings.faceFixationTime;
        // Setting Counter values to User Settings
        targetObjRegisterCountdownTimer = subjectResponseWaitTime;
        avatarFaceGazeCountdownTimer = faceFixationWaitTime;
        //Find all the endTarget tagged objects into an array and select one randomly
        GameObject[] endTargets;
        endTargets = GameObject.FindGameObjectsWithTag("endTarget");
        //Randomly select a endTarget
        int randomTargetIndex = UnityEngine.Random.Range(0, endTargets.Length);
        targetHeadEndPoint = endTargets[randomTargetIndex];
        //Find the direction component attached from the script
        endPointDirection = targetHeadEndPoint.GetComponent<TargetEndPointProperty>().direction.ToString();
        //Fixates the headTarget position to the reference GameObject of initialHeadTargetPosition
        headTarget.transform.position = initialHeadTargetPosition.transform.position;
        //After this setup will wait till the eye contact has been established 
    }

    // Update is called once per frame
    void Update()
    {
        //Recording the user's eyeContact response
        if (waitForEyeContact == true)//waits for the eyeContact flag to turn on
        {
            if (GazeCursor.currentGazedObject != null)//check that the user has started gazing, and that the user is wearing the headset and actually gazing
            {
                if ((GazeCursor.currentGazedObject == avatarHeadRegion.name) && avatarFaceGazeCountdownTimer > 0)//checks if the currently gazed object is the avatarHeadRegion and the timer counter has not lapsed
                {
                    avatarFaceGazeCountdownTimer -= Time.deltaTime;//reduce the timer if fixation is on the avatar's face region
                }
                else if (avatarFaceGazeCountdownTimer <= 0)//check if countdown has been set to zero then the faceFixation has been done for the required duration
                {
                    waitForEyeContact = false;//turn off eye contact response flag
                    //After Eye Contact has been established will start the Coroutine
                    //Coroutine to move the headTarget so as to move the avatar head and the Arm Animation
                    StartCoroutine(MoveTargetBall(endPointDirection));
                }
                else if (GazeCursor.currentGazedObject != avatarHeadRegion.name)//check is fixation is on some other object 
                {
                    avatarFaceGazeCountdownTimer = faceFixationWaitTime;//reset the faceFixation timer if changed the gazed object
                }
            }
            else if (GazeCursor.currentGazedObject == null)//Condition if the user is not staring at a particular collider
            {
                avatarFaceGazeCountdownTimer = faceFixationWaitTime;//reset the faceFixation timer if changed the gazed object
            }
        }
        //Recording the user's gaze response 
        if (waitForResponse == true && waitForEyeContact == false) //waits for registering response flag to turn on and the eyeContact flag to be turned off
        {
            //Deterimines if any GameObject is being gazed upon, if yes then prints the name of the GameObject
            if (GazeCursor.currentGazedObject != null) //check that the user has started gazing, and that the user is wearing the headset and actually gazing
            {
                currentGazedObject = GazeCursor.currentGazedObject;
                if ((currentGazedObject == prevGazedObject || prevGazedObject == null) && targetObjRegisterCountdownTimer > 0)//Counter starts to decrease only if the fixation is on the same obj (i.e current == prev object) or if the prev object is null(i.e the user has focused on a single object and not change the gaze)
                {
                    targetObjRegisterCountdownTimer -= Time.deltaTime;
                }
                else if (targetObjRegisterCountdownTimer <= 0)//If counter is in negative then the user has focused on a particular object for the required duration continously and we accept the gaze object as the final user response
                {
                    //N.B: HELP REQUIRED !!!
                    Debug.Log("User Response is: " + currentGazedObject);
                    waitForResponse = false;//turn off wait for response flag
                    //Increment the curent project counter
                    ExperimentSettings.currentTrialCount += 1;
                    //Check if any remaining trials 
                    if (ExperimentSettings.currentTrialCount > ExperimentSettings.trialCount)
                    {
                        //Load the exit scene, with some features of the Interim Menu
                    }
                    else if (ExperimentSettings.trialCount >= ExperimentSettings.currentTrialCount)
                    {
                        //First reload the Interim Menu which will again reload the trial
                        //For dev purpose only loading the trial Scene again
                        SceneManager.LoadScene(1);//Loading the Experiment Scene Again
                    }
                }
                else  //If the currentGazedObject has changed or not equal to prev GameObj. N.B: Will not work if the user is 
                {
                    targetObjRegisterCountdownTimer = subjectResponseWaitTime; //If lost attention from the Gazed Object set back the timer to the value 
                }
                prevGazedObject = currentGazedObject;//Each frame registers the gazed object and maintains log of the previously gazed object
            }
            else if (GazeCursor.currentGazedObject == null)//Condition if the user is not staring at a particular collider
            {
                targetObjRegisterCountdownTimer = subjectResponseWaitTime; //If lost attention from the Gazed Object set back the timer to the value 
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

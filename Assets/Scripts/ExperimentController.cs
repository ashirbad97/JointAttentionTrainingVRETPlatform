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
    [SerializeField] bool isCueHierarchy; //N.B: Take Value from Settings
    [SerializeField] bool isKeepPointing;//N.B: Take Value from Settings
    //View the current countdown
    [SerializeField] float avatarFaceGazeCountdownTimer;
    [SerializeField] float targetObjRegisterCountdownTimer;
    //Declaring Events for Arm Animation of the Avatar
    public static event Action<float, string> OnObjectTrackedInRightDir;
    public static event Action<float, string> OnObjectTrackedInLeftDir;
    public bool waitForEyeContact = true;
    public static bool waitForResponse = false;
    string currentGazedObject;
    string prevGazedObject;
    string endPointDirection;
    FoveInterface fove;
    GameObject[] endTargets;
    GameObject[] targetObjects;
    TrialValue trialData = new TrialValue();
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Current Trial is: " + ExperimentSettings.currentTrialCount);
        // Adding current trial count into trialData
        trialData.trial_currentTrialNo = ExperimentSettings.currentTrialCount;
        // Adding target objects into trialData
        trialData.trial_spawnedObjects = GameObject.FindGameObjectsWithTag("targetObject");
        // Assigning parameter variables with settings values from Input Menu
        timeToMoveHeadTarget = ExperimentSettings.cueDeliveryDuration;
        subjectResponseWaitTime = ExperimentSettings.responseRegistrationFixationDuration;
        faceFixationWaitTime = ExperimentSettings.faceFixationDuration;
        // Setting Counter values to User Settings
        targetObjRegisterCountdownTimer = subjectResponseWaitTime;
        avatarFaceGazeCountdownTimer = faceFixationWaitTime;
        //Find all the endTarget tagged objects into an array and select one randomly
        endTargets = GameObject.FindGameObjectsWithTag("endTarget");
        //Randomly select a endTarget
        int randomTargetIndex = UnityEngine.Random.Range(0, endTargets.Length);
        targetHeadEndPoint = endTargets[randomTargetIndex];
        //Find the direction component attached from the script
        endPointDirection = targetHeadEndPoint.GetComponent<TargetEndPointProperty>().direction.ToString();
        // Adding the selected trialObject into trialData

        //Fixates the headTarget position to the reference GameObject of initialHeadTargetPosition
        headTarget.transform.position = initialHeadTargetPosition.transform.position;
        isCueHierarchy = true; //N.B: Remove this as this will ultimately come from the settings
        //After this setup will wait till the eye contact has been established 
        //Registration of FoveProperties
        FoveManager.RegisterCapabilities(Fove.ClientCapabilities.UserPresence);
        FoveManager.RegisterCapabilities(Fove.ClientCapabilities.GazeDepth);
        isKeepPointing = true;//Flag to recognize if pointing has to be done or not
        fove = FindObjectOfType<FoveInterface>();//Finding the existing fove object in the scene
    }

    // Update is called once per frame
    void Update()
    {
        //Print the raw gaze values from the fove object
        // Debug.Log("Gaze Depth: " + FoveManager.GetCombinedGazeDepth().value);
        // Debug.Log("Eye Position: " + fove.GetCombinedGazeRay().value.GetPoint(FoveManager.GetCombinedGazeDepth().value));
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
                    HeadMovement();

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
                    // N.B: Add conditon to check if user gazed correct target is incorrect
                    if (isCueHierarchy)
                    {
                        targetObjRegisterCountdownTimer = subjectResponseWaitTime;
                        isCueHierarchy = false;
                        HandMovement();
                    }
                    else
                    {
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
    //Function to call the ball/head movement coroutine
    void HeadMovement()
    {
        StartCoroutine(MoveTargetBall(endPointDirection));
    }
    //Triggers the events to call the Hand Movement Animation
    void HandMovement()
    {
        //At this point the ball has moved to it's targetEndPoint
        //At this point we should start checking if the user is able to Gaze at the correct object or not
        //If we need to mve only the Arm before running this we can set the 
        //Manually hard-coding for the Arm Movement animation to run after the Head Movement
        if (endPointDirection == "Right")
        {
            //Checks for the bool value and pass the parameters. It has been configured in AnimatorController to stay or place the finger back
            if (isKeepPointing)
            {
                OnObjectTrackedInRightDir?.Invoke(ExperimentSettings.cueDeliveryDuration, "PointRightStay");
            }
            else
            {
                OnObjectTrackedInRightDir?.Invoke(ExperimentSettings.cueDeliveryDuration, "PointRight");
            }
        }
        else
        {
            if (isKeepPointing)
            {
                OnObjectTrackedInLeftDir?.Invoke(ExperimentSettings.cueDeliveryDuration, "PointLeftStay");
            }
            else
            {
                OnObjectTrackedInLeftDir?.Invoke(ExperimentSettings.cueDeliveryDuration, "PointLeft");
            } //Invoke Event only if subscribed
        }


        //waitForResponse = true;
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

        if (!isCueHierarchy)
        {
            HandMovement();
        }
        else
        {
            waitForResponse = true;
        }
        yield return null;
    }
}

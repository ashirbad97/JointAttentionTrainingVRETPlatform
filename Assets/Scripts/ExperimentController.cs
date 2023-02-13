using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    public bool waitForEyeContact = true;//Crucial as first condition to check if eye contact is required or not
    //Crucial as after cue delivery need to keep checking for the user reaponse registration, made static as also reffered from the ArmTargetPlacement script
    // Since this is static can't be exposed to the Unity Editor
    public static bool waitForResponse = false;
    string currentGazedObject;
    string prevGazedObject;
    string endPointDirection;
    TimeSpan trialTotalDuration;
    DateTime trialStartTime;
    DateTime trialEndTime;
    FoveInterface fove;
    GameObject[] endTargets;
    GameObject[] targetObjects;
    TrialValue trialData = new TrialValue();

    void Start()
    {
        // FoveManager.IsEyeTrackingReady();
        // Set trial settings from the session settings
        isCueHierarchy = ExperimentSettings.is_conditionedCueSequence;
        isKeepPointing = ExperimentSettings.is_keepFingerPointing;
        // Fetches the system time, check if this is in current time format or it varies according to the system time format
        trialStartTime = DateTime.Now;
        // Check if the avatar is already not set and then set the avatar
        // Ensure that this will be set in the first trial and not in each and every trial
        if (ExperimentSettings.avatar != "")
        {
            //Fetching the experiment settings and convert into string
            ExperimentSettings.avatar = GameObject.FindGameObjectWithTag("Avatar").name;
        }
        // Adding the avatar into trialData
        trialData.trial_currentAvatar = ExperimentSettings.avatar;
        // Adding current trial count into trialData
        trialData.trial_currentTrialNo = ExperimentSettings.currentTrialCount;
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
        trialData.trial_targetDirection = endPointDirection;
        Debug.Log("Target Direction is: " + endPointDirection);
        // Adding the selected trialObject into trialData
        targetObjects = GameObject.FindGameObjectsWithTag("targetObject");
        // Iterating through all the spawned GameObjects and find the direction property and compare with the endPointDirection 
        List<string> spawnedObjects = new List<string>();
        foreach (GameObject obj in targetObjects)
        {
            TargetEndPointProperty objName = obj.GetComponent<TargetEndPointProperty>();
            spawnedObjects.Add(objName.name); // Adding all spawned objects to the list in trialData
            if (objName.direction.ToString() == endPointDirection)
            {
                Debug.Log("Target Object is: " + objName.name);
                trialData.trial_targetObject = objName.name;
            }
        }
        // Adding target objects into trialData
        trialData.trial_spawnedObjects = spawnedObjects;
        //Fixates the headTarget position to the reference GameObject of initialHeadTargetPosition
        headTarget.transform.position = initialHeadTargetPosition.transform.position;
        //After this setup will wait till the eye contact has been established 
        //Registration of FoveProperties
        FoveManager.RegisterCapabilities(Fove.ClientCapabilities.UserPresence);
        FoveManager.RegisterCapabilities(Fove.ClientCapabilities.GazeDepth);
        FoveManager.RegisterCapabilities(Fove.ClientCapabilities.EyeTracking);
        fove = FindObjectOfType<FoveInterface>();//Finding the existing fove object in the scene
        Debug.Log("Current Trial is: " + ExperimentSettings.currentTrialCount);
    }

    // Update is called once per frame
    void Update()
    {
        //Print the raw gaze values from the fove object
        // Debug.Log("Gaze Depth: " + FoveManager.GetCombinedGazeDepth().value);
        // Debug.Log("Eye Position: " + fove.GetCombinedGazeRay().value.GetPoint(FoveManager.GetCombinedGazeDepth().value));
        //Recording the user's eyeContact response
        // *********************************N.B: Condition to check if continously maintaining eye contact with the avatar head region*******************************************
        if (waitForEyeContact == true)//waits for the eyeContact flag to turn on
        {
            if (GazeCursor.currentGazedObject != null)//check that the user has started gazing, and that the user is wearing the headset and actually gazing
            {
                if ((GazeCursor.currentGazedObject == avatarHeadRegion.name) && avatarFaceGazeCountdownTimer > 0)//checks if the currently gazed object is the avatarHeadRegion and the timer counter has not lapsed
                {
                    avatarFaceGazeCountdownTimer -= Time.deltaTime;//reduce the timer if fixation is on the avatar's face region
                }
                // ***************N.B: Condition when user has successfully gazed at the avatar continuously*********************************
                else if (avatarFaceGazeCountdownTimer <= 0)//check if countdown has been set to zero then the faceFixation has been done for the required duration
                {
                    DateTime eyeContactEstablishedTime = DateTime.Now;
                    trialData.trial_eyeContactEstablishedTime = eyeContactEstablishedTime.ToString();
                    trialData.trial_totalDurationToEstablishEyeContactSinceTrialStart = eyeContactEstablishedTime.Subtract(trialStartTime).ToString();
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
                    Debug.Log("User Response is: " + currentGazedObject);
                    waitForResponse = false;//turn off wait for response flag
                    // Adding the registered object into the trial data
                    trialData.trial_registeredObject = currentGazedObject;
                    // Condition to check if the gazed object is the correct target object or not
                    if (currentGazedObject.ToString() == trialData.trial_targetObject)
                        trialData.trial_isGazedObjCorrect = true;
                    else
                        trialData.trial_isGazedObjCorrect = false;
                    // N.B: *******************Condition if the cueHierachy is ON and the user gazesAt the incorrect object******************************
                    if (isCueHierarchy && !trialData.trial_isGazedObjCorrect)
                    {
                        targetObjRegisterCountdownTimer = subjectResponseWaitTime;
                        isCueHierarchy = false;
                        trialData.trial_isAdditionalCue = true;
                        trialData.trial_wrongObjAftInitialCue = currentGazedObject.ToString();//verify that it will be the same frame and not change
                        trialData.trial_wrongObjAftrInitialCue_registrationTime = DateTime.Now.ToString();
                        HandMovement();
                    }
                    // N.B: *******************Condition if the cueHierachy is OFF and records the registered object******************************
                    else
                    {
                        // Fetches the system time, check if this is in current time format or it varies according to the system time format
                        trialEndTime = DateTime.Now;
                        trialTotalDuration = trialEndTime.Subtract(trialStartTime);
                        // Adds all the trial time values into trialData
                        trialData.trial_startTime = trialStartTime.ToString();
                        trialData.trial_endTime = trialEndTime.ToString();
                        trialData.trial_totalDuration = trialTotalDuration.ToString();
                        //Check if any remaining trials, condition to check for the last trial in the session
                        if (ExperimentSettings.trialCount >= ExperimentSettings.currentTrialCount) //check for any remaining sessions
                        {
                            Debug.Log("No of trials remaining: " + (ExperimentSettings.trialCount - ExperimentSettings.currentTrialCount));
                            ExperimentSettings.prevTrialData = trialData;
                            SceneManager.LoadScene("Interim_Menu");//Loading the Interim Menu Scene
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

    //Triggers the events to call the Hand Movement Animation
    void HandMovement()
    {
        // Set the hand cue used flag to be true for the trial
        trialData.trial_isHandCueUsed = true;
        if (endPointDirection == "Right")
        {
            Debug.Log("Start Hand Movement to Right");
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
            Debug.Log("Start Hand Movement to Left");
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
    //Function to call the ball/head movement coroutine
    void HeadMovement()
    {
        StartCoroutine(MoveTargetBall(endPointDirection));
    }
    IEnumerator MoveTargetBall(string endPointDirection)
    {
        trialData.trial_headCueDeliveryStart = DateTime.Now.ToString();
        Debug.Log("Head Movement Direction is: " + endPointDirection);
        // End Temporary Fix for Direction Nomenclature Error
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
        trialData.trial_headCueDeliveryEnd = DateTime.Now.ToString();
        // N.B: **************************Check if cueHierarcy is on to deliver the additional hand cue or simply wait to record the user response********************************
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

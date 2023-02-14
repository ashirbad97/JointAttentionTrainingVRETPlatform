using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;
using System;
using TMPro;
using Fove.Unity;

public class MenuController : MonoBehaviour
{
    //Referencing to the input values from the form
    [SerializeField]
    private TMP_InputField uid, trialCount, targetObjectsCount, targetObjectArrayIndices,
        avatar, faceFixationDuration, responseRegistrationFixationDuration, cueDeliveryDuration, experimentMetaData;
    [SerializeField] private Toggle is_enforceEyeContact, is_conditionedCueSequence, is_keepFingerPointing;
    [SerializeField] string experimentSessionDirName;
    [SerializeField] bool enforceCalibration;
    //Temporary Hardcoded Indices
    private int[] hardCodedTargetObjects = { 0, 1, 2, 3, 4, 5, 6, 7 };
    //Main Menu button to start the experiment
    public void playGame()
    {
        try
        {
            // Registration of FOVE capabilities
            FoveManager.RegisterCapabilities(Fove.ClientCapabilities.EyeTracking);
            FoveManager.RegisterCapabilities(Fove.ClientCapabilities.UserPresence);
            //Fetching the experiment settings and convert into int
            ExperimentSettings.uid = int.Parse(uid.text);
            ExperimentSettings.trialCount = int.Parse(trialCount.text);
            ExperimentSettings.targetObjectsCount = int.Parse("2"); //hardcoded value
            //Fetching the experiment settings and convert into float
            ExperimentSettings.faceFixationDuration = float.Parse(faceFixationDuration.text);
            ExperimentSettings.responseRegistrationFixationDuration = float.Parse(responseRegistrationFixationDuration.text);
            ExperimentSettings.cueDeliveryDuration = float.Parse(cueDeliveryDuration.text);
            // Fetching the experiment settings and convert into bool
            // Comparing the expression and storing the bool response
            ExperimentSettings.is_keepFingerPointing = is_keepFingerPointing.isOn;
            ExperimentSettings.is_conditionedCueSequence = is_conditionedCueSequence.isOn;
            ExperimentSettings.is_enforceEyeContact = is_enforceEyeContact.isOn;
            //Fetching the experiment settings and convert into array
            //Actual Value: targetObjectArrayIndices.text.Trim().Split(",") . N.B: Before using this convert the data type in ExperimentSettings to string
            ExperimentSettings.targetObjectArrayIndices = string.Join("", hardCodedTargetObjects); //hardcoded value
            ExperimentSettings.experimentMetaData = experimentMetaData.text;
            ExperimentSettings.avatar = "Ashirbad";
            // Generate the experiment session Dir name and add it to Experiment Settings
            experimentSessionDirName = "JAMaster_Date_" + DateTime.Now.ToLongDateString() + "_TimeUTC_" + DateTime.Now.ToFileTimeUtc() + "_UID_" + ExperimentSettings.uid;
            ExperimentSettings.experimentSessionParentDirName = experimentSessionDirName;
            //Check if user is wearing headset then only load
            if (FoveManager.IsUserPresent().value)
            {
                CalibrateCheckAndStart();
            }
            else
            {
                StartCoroutine(checkUserPresence());
            }
        }
        catch (Exception e)
        {
            Debug.Log("Caught exception during parsing of experiment settings");
            Debug.LogException(e, this);
        }
    }
    // Check if user is wearing HMD or not
    IEnumerator checkUserPresence()
    {
        Debug.Log("Please wear Headset");
        yield return FoveManager.WaitForUser;// Pauses coroutine until till user wears the headset
        Debug.Log("Is user wearing headset ? " + FoveManager.IsUserPresent().value);
        /**
        @dev: For now double checking is not required enforce later if found useful
        // Double check to ensure the user is wearing the headset
         if (!FoveManager.IsUserPresent().value)// Even if due to some reason it skipped in the first time 
             StartCoroutine(checkUserPresence());//check again and rerun the coroutine
        else
        */
        CalibrateCheckAndStart();//Start calibration check 
    }
    // Check if calibration is to be enforced and load the scene
    void CalibrateCheckAndStart()
    {
        // Check for Calibration Option
        if (enforceCalibration)
        {
            /**
            @dev: For now checking if user is already calibrated being disabled, enforce later if found useful
            // N.B: If user is already calibrated no need to enforce it and start experiment , validate this condition properly
            if (FoveManager.IsEyeTrackingReady().value)
            {
                Debug.Log("Already calibrated user, no need to enforce.");
                StartExperiment();
            }
            else // If not enforce calibration
            */
            StartCoroutine(EnforceCalibration());
        }
        else
        {
            StartExperiment();// If calibration not enforced simply start the experiment
        }
    }
    // Coroutine to enforce the calibration if provided in settings and re-run unless the user has been calibrated
    IEnumerator EnforceCalibration()
    {
        Debug.Log("Enforcing Calibration Start");
        FoveManager.StartEyeTrackingCalibration(); //Start the calibration screen
        yield return FoveManager.WaitForEyeTrackingCalibrationEnd;// Pause the coroutine until the calibration of the user has ended
        if (FoveManager.IsEyeTrackingReady().value)//If calibrated after eye tracking enforced load the experiment
        {
            Debug.Log("Calibration Successful");
            StartExperiment();
        }
        else //If failed to calibrate after the enforced calibration has failed restart the calibration
        {
            Debug.Log("Calibration Failed. Start Again.");
            StartCoroutine(EnforceCalibration());
        }
    }
    void StartExperiment()
    {
        // Register the time when the experiment session starts
        ExperimentSettings.experimentSessionStartTime = DateTime.Now.ToString();
        DataDumper.DumpSessionDataExpStart();//Only dump settings if we know that at least the first trial is going to start
        Debug.Log("Experiment Session Started");
        SceneManager.LoadScene("Experiment1_Ethical_Demo");
    }
    // Function to dump the session data into FS and later on DB
    public void quitGame()
    {
        //Close the application
        Application.Quit();
    }
}

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
            FoveManager.RegisterCapabilities(Fove.ClientCapabilities.EyeTracking);
            FoveManager.RegisterCapabilities(Fove.ClientCapabilities.UserPresence);
            Debug.Log("Experiment Session started");
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
            // Check if user is wearing headset then only load
            if (FoveManager.IsUserPresent())
            {
                LoadExperiment();
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
    IEnumerator checkUserPresence()
    {
        Debug.Log("Please wear Headset");
        yield return FoveManager.WaitForUser;
        LoadExperiment();
    }
    private void EnforceCalibration()
    {
        Debug.Log("Enforcing Calibration Start");
        FoveManager.StartEyeTrackingCalibration();
        if (FoveManager.IsEyeTrackingReady())
        {
            //Load the next scene
            SceneManager.LoadScene("Experiment1_Ethical_Demo");
        }
    }
    void LoadExperiment()
    {
        // Check for Calibration Option
        if (enforceCalibration)
        {
            EnforceCalibration();
        }
        else
        {
            // Register the time when the experiment session starts
            ExperimentSettings.experimentSessionStartTime = DateTime.Now.ToString();
            DataDumper.DumpSessionDataExpStart();//Only dump settings if we know that at least the first trial is going to start
            SceneManager.LoadScene("Experiment1_Ethical_Demo");
        }
    }
    // Function to dump the session data into FS and later on DB
    public void quitGame()
    {
        //Close the application
        Application.Quit();
    }
}

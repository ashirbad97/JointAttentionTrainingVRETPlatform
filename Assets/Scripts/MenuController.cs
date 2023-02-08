using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;
using System;
using TMPro;

public class MenuController : MonoBehaviour
{
    //Referencing to the input values from the form
    [SerializeField]
    private TMP_InputField uid, trialCount, targetObjectsCount, targetObjectArrayIndices,
        avatar, faceFixationDuration, responseRegistrationFixationDuration, cueDeliveryDuration, experimentMetaData;
    [SerializeField] private Toggle is_enforceEyeContact, is_conditionedCueSequence, is_keepFingerPointing;
    [SerializeField] string experimentSessionDirName;
    //Temporary Hardcoded Indices
    private int[] hardCodedTargetObjects = { 0, 1, 2, 3, 4, 5, 6, 7 };
    //Main Menu button to start the experiment
    public void playGame()
    {
        try
        {
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
            ExperimentSettings.targetObjectArrayIndices = hardCodedTargetObjects; //hardcoded value
            ExperimentSettings.experimentMetaData = experimentMetaData.text;
            //Debug.Log(ExperimentSettings.uid+ ExperimentSettings.trialCount+ ExperimentSettings.targetObjectsCount+ ExperimentSettings.faceFixationTime.ToString()+ ExperimentSettings.responseRegistrationFixationDuration+ ExperimentSettings.cueDeliverySpeed+ ExperimentSettings.enforceEyeContact+ ExperimentSettings.targetObjectArrayIndices+ ExperimentSettings.avatar+ExperimentSettings.experimentMetaData);
            ExperimentSettings.experimentSessionStartTime = DateTime.Now.ToString();
            // Generate the experiment session Dir name and add it to Experiment Settings
            experimentSessionDirName = DateTime.Now.ToLongDateString() + "_" + ExperimentSettings.uid;
            ExperimentSettings.experimentSessionParentDirName = experimentSessionDirName;
            DumpSessionData();
            //Load the next scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        catch (Exception e)
        {
            Debug.Log("Caught exception during parsing of experiment settings");
            Debug.LogException(e, this);
        }

    }
    // Function to dump the session data into FS and later on DB
    void DumpSessionData()
    {
        if (Directory.Exists(ExperimentSettings.masterDirName + ExperimentSettings.experimentSessionParentDirName))
        {
            ExperimentSettings.experimentSessionSettingsFileName = ExperimentSettings.masterDirName + ExperimentSettings.experimentSessionParentDirName + "/" + "SessionData" + ".txt";
            File.AppendAllTextAsync(ExperimentSettings.experimentSessionSettingsFileName, "UID: " + ExperimentSettings.uid);
            File.AppendAllTextAsync(ExperimentSettings.experimentSessionSettingsFileName, "Trial Count: " + ExperimentSettings.trialCount);
            File.AppendAllTextAsync(ExperimentSettings.experimentSessionSettingsFileName, "No. of Target Objects: " + ExperimentSettings.targetObjectsCount);
            File.AppendAllTextAsync(ExperimentSettings.experimentSessionSettingsFileName, "Face Fixation Duration: " + ExperimentSettings.faceFixationDuration);
            File.AppendAllTextAsync(ExperimentSettings.experimentSessionSettingsFileName, "Response Registration Fixation Duration: " + ExperimentSettings.responseRegistrationFixationDuration);
            File.AppendAllTextAsync(ExperimentSettings.experimentSessionSettingsFileName, "Cue Delivery Duration: " + ExperimentSettings.cueDeliveryDuration);
            File.AppendAllTextAsync(ExperimentSettings.experimentSessionSettingsFileName, "EnforceEyeContact: " + ExperimentSettings.is_enforceEyeContact);
            File.AppendAllTextAsync(ExperimentSettings.experimentSessionSettingsFileName, "ConditionedCueSequence: " + ExperimentSettings.is_conditionedCueSequence);
            File.AppendAllTextAsync(ExperimentSettings.experimentSessionSettingsFileName, "KeepFingerPointing: " + ExperimentSettings.is_keepFingerPointing);
            File.AppendAllTextAsync(ExperimentSettings.experimentSessionSettingsFileName, "TargetObjArrayIndices: " + ExperimentSettings.targetObjectArrayIndices);
            File.AppendAllTextAsync(ExperimentSettings.experimentSessionSettingsFileName, "Avatar: " + ExperimentSettings.avatar);
            File.AppendAllTextAsync(ExperimentSettings.experimentSessionSettingsFileName, "Experiment Metadata: " + ExperimentSettings.experimentMetaData);
            File.AppendAllTextAsync(ExperimentSettings.experimentSessionSettingsFileName, "Experiment Session Total Duration: " + ExperimentSettings.experimentSessionTotalDuration);
            File.AppendAllTextAsync(ExperimentSettings.experimentSessionSettingsFileName, "Experiment Session Start Time: " + ExperimentSettings.experimentSessionStartTime);
            File.AppendAllTextAsync(ExperimentSettings.experimentSessionSettingsFileName, "Experiment Session End Time: " + ExperimentSettings.experimentSessionEndTime);
            File.AppendAllTextAsync(ExperimentSettings.experimentSessionSettingsFileName, "Experiment Session Master Directory Name: " + ExperimentSettings.masterDirName);
            File.AppendAllTextAsync(ExperimentSettings.experimentSessionSettingsFileName, "Experiment Session Parent Directory Name: " + ExperimentSettings.experimentSessionParentDirName);
            File.AppendAllTextAsync(ExperimentSettings.experimentSessionSettingsFileName, "Experiment Session Settings File Name: " + ExperimentSettings.experimentSessionSettingsFileName);
            File.AppendAllTextAsync(ExperimentSettings.experimentSessionSettingsFileName, "Last Conducted Session No: " + ExperimentSettings.currentTrialCount);
        }
    }
    public void quitGame()
    {
        //Close the application
        Application.Quit();
    }
}

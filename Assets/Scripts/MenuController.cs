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
            //Debug.Log(ExperimentSettings.uid+ ExperimentSettings.trialCount+ ExperimentSettings.targetObjectsCount+ ExperimentSettings.faceFixationTime.ToString()+ ExperimentSettings.responseRegistrationFixationDuration+ ExperimentSettings.cueDeliverySpeed+ ExperimentSettings.enforceEyeContact+ ExperimentSettings.targetObjectArrayIndices+ ExperimentSettings.avatar+ExperimentSettings.experimentMetaData);
            ExperimentSettings.experimentSessionStartTime = DateTime.Now.ToString();
            // Generate the experiment session Dir name and add it to Experiment Settings
            experimentSessionDirName = "JAMaster_Date_" + DateTime.Now.ToLongDateString() + "_TimeUTC_" + DateTime.Now.ToFileTimeUtc() + "_UID_" + ExperimentSettings.uid;
            ExperimentSettings.experimentSessionParentDirName = experimentSessionDirName;
            DataDumper.DumpSessionDataExpStart();
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
    public void quitGame()
    {
        //Close the application
        Application.Quit();
    }
}

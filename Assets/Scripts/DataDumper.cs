using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;
using System;
using TMPro;
public class DataDumper
{
    public static void DumpSessionDataExpStart()
    {
        try
        {
            Debug.Log("Start dumping session settings data into FS at experiment start.");
            // Check if Dir does not exist create a new one
            if (!Directory.Exists(ExperimentSettings.masterDirName + ExperimentSettings.experimentSessionParentDirName))
            {
                Debug.Log("Directory not found will create one. For Dump Session Settings Data");
                Directory.CreateDirectory(ExperimentSettings.masterDirName + ExperimentSettings.experimentSessionParentDirName);
            }
            else if (Directory.Exists(ExperimentSettings.masterDirName + ExperimentSettings.experimentSessionParentDirName))
            {
                Debug.Log("Directory found. For Dump Session Settings Data");
                ExperimentSettings.experimentSessionSettingsFileName = ExperimentSettings.masterDirName + ExperimentSettings.experimentSessionParentDirName + "/" + "SessionData" + ".txt";
                Debug.Log("Experiment Session Settings File Name: " + ExperimentSettings.experimentSessionSettingsFileName);
                Debug.Log("Data Dump Starting. For Dump Session Settings Data");
                File.AppendAllText(ExperimentSettings.experimentSessionSettingsFileName, "Joint Attention Training Platform v0.0.1 Session Settings Dump \n");
                File.AppendAllText(ExperimentSettings.experimentSessionSettingsFileName, "\n User Details: \n");
                File.AppendAllText(ExperimentSettings.experimentSessionSettingsFileName, "\n UID: " + ExperimentSettings.uid);
                File.AppendAllText(ExperimentSettings.experimentSessionSettingsFileName, "\n Total Trials: " + ExperimentSettings.trialCount);
                File.AppendAllText(ExperimentSettings.experimentSessionSettingsFileName, "\n Experiment Session Settings Details: \n");
                File.AppendAllText(ExperimentSettings.experimentSessionSettingsFileName, "\n No. of Target Objects: " + ExperimentSettings.targetObjectsCount);
                File.AppendAllText(ExperimentSettings.experimentSessionSettingsFileName, "\n Face Fixation Duration: " + ExperimentSettings.faceFixationDuration);
                File.AppendAllText(ExperimentSettings.experimentSessionSettingsFileName, "\n Response Registration Fixation Duration: " + ExperimentSettings.responseRegistrationFixationDuration);
                File.AppendAllText(ExperimentSettings.experimentSessionSettingsFileName, "\n Cue Delivery Duration: " + ExperimentSettings.cueDeliveryDuration);
                File.AppendAllText(ExperimentSettings.experimentSessionSettingsFileName, "\n EnforceEyeContact: " + ExperimentSettings.is_enforceEyeContact);
                File.AppendAllText(ExperimentSettings.experimentSessionSettingsFileName, "\n ConditionedCueSequence: " + ExperimentSettings.is_conditionedCueSequence);
                File.AppendAllText(ExperimentSettings.experimentSessionSettingsFileName, "\n KeepFingerPointing: " + ExperimentSettings.is_keepFingerPointing);
                File.AppendAllText(ExperimentSettings.experimentSessionSettingsFileName, "\n TargetObjArrayIndices: " + ExperimentSettings.targetObjectArrayIndices);
                File.AppendAllText(ExperimentSettings.experimentSessionSettingsFileName, "\n Avatar: " + ExperimentSettings.avatar);
                File.AppendAllText(ExperimentSettings.experimentSessionSettingsFileName, "\n Experiment Metadata: " + ExperimentSettings.experimentMetaData);
                File.AppendAllText(ExperimentSettings.experimentSessionSettingsFileName, "\n Experiment Session Start Time: " + ExperimentSettings.experimentSessionStartTime);
                File.AppendAllText(ExperimentSettings.experimentSessionSettingsFileName, "\n Experiment Session Master Directory Name: " + ExperimentSettings.masterDirName);
                File.AppendAllText(ExperimentSettings.experimentSessionSettingsFileName, "\n Experiment Session Parent Directory Name: " + ExperimentSettings.experimentSessionParentDirName);
                File.AppendAllText(ExperimentSettings.experimentSessionSettingsFileName, "\n Experiment Session Settings File Name: " + ExperimentSettings.experimentSessionSettingsFileName);
                Debug.Log("Data Dump Complete. For Dump Session Settings Data");
            }
        }
        catch (Exception e)
        {
            Debug.Log("Caught exception during dumping of experiment session settings data");
            Debug.LogException(e);
        }
    }
    public static void DumpSessionDataExpEnd()
    {
        try
        {
            Debug.Log("Start dumping session settings data into FS at experiment end.");
            // Check if Dir does not exist create a new one
            if (!Directory.Exists(ExperimentSettings.masterDirName + ExperimentSettings.experimentSessionParentDirName))
            {
                Debug.Log("Directory not found will create one. For Dump Session Settings Data");
                Directory.CreateDirectory(ExperimentSettings.masterDirName + ExperimentSettings.experimentSessionParentDirName);
            }
            if (Directory.Exists(ExperimentSettings.masterDirName + ExperimentSettings.experimentSessionParentDirName))
            {
                File.AppendAllText(ExperimentSettings.experimentSessionSettingsFileName, "\n Experiment Session Settings Details Ending: ");
                File.AppendAllText(ExperimentSettings.experimentSessionSettingsFileName, "\n Last Conducted Session No: " + ExperimentSettings.currentTrialCount);
                File.AppendAllText(ExperimentSettings.experimentSessionSettingsFileName, "\n Experiment Session End Time: " + ExperimentSettings.experimentSessionEndTime);
                File.AppendAllText(ExperimentSettings.experimentSessionSettingsFileName, "\n Experiment Session Total Duration: " + ExperimentSettings.experimentSessionTotalDuration);
                Debug.Log("Data Dump Complete. For Dump Session Settings Data at end");
            }
        }
        catch (Exception e)
        {
            Debug.Log("Caught exception during dumping of experiment session settings data at the end");
            Debug.LogException(e);
        }
    }
    // Function to dump the trial data into FS and later on DB
    public static void DumpTrialData(TrialValue dumpTrialData)
    {
        try
        {
            Debug.Log("Starting to dump trial data of " + dumpTrialData.trial_currentTrialNo);
            // Check if Dir does not exist create a new one
            if (!Directory.Exists(ExperimentSettings.masterDirName + ExperimentSettings.experimentSessionParentDirName))
            {
                Directory.CreateDirectory(ExperimentSettings.masterDirName + ExperimentSettings.experimentSessionParentDirName);
            }
            else
            {
                dumpTrialData.trial_trialFileName = Path.Combine(ExperimentSettings.masterDirName, ExperimentSettings.experimentSessionParentDirName + "/" + "Trial" + "_" + ExperimentSettings.currentTrialCount.ToString() + ".json");
                Debug.Log("Dumping trial data details into: " + dumpTrialData.trial_trialFileName);
                File.AppendAllText(dumpTrialData.trial_trialFileName, JsonUtility.ToJson(dumpTrialData));
            }
        }
        catch (Exception e)
        {
            Debug.Log("Caught exception during dumping of experiment session settings data at the end");
            Debug.LogException(e);
        }

    }
    // Function to dump hand delivery cue end time
    public static void DumpHandCueEndTime(string handCue)
    {
        string handCueDumpFileName = Path.Combine(ExperimentSettings.masterDirName, ExperimentSettings.experimentSessionParentDirName + "/handCueEndTime.txt");
        string handCueEndTime = DateTime.Now.ToString();
        File.AppendAllText(handCueDumpFileName, ("Hand Cue: " + handCue + " completed at: " + handCueEndTime + "\n"));

    }
}



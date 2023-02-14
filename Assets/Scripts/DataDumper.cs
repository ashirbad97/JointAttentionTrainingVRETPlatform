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
    static void writeToFileSessionDataExpStart()
    {
        ExperimentSettings.experimentSessionSettingsFileName = ExperimentSettings.masterDirName + ExperimentSettings.experimentSessionParentDirName + "/" + "SessionData" + ".txt";
        Debug.Log("Experiment Session Settings File Name: " + ExperimentSettings.experimentSessionSettingsFileName);
        Debug.Log("Data Dump Starting. For Dump Session Settings Data Start");
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
    // N.B: Important section to check for the presence of a dir, the dir will not be present at beginning so this function will create one
    public static void DumpSessionDataExpStart()
    {
        try
        {
            Debug.Log("Start dumping session settings data into FS at experiment start.");
            // N.B: Check if Dir does not exist create a new one. This check is redundant as this function is expected to create a dir.
            if (!Directory.Exists(ExperimentSettings.masterDirName + ExperimentSettings.experimentSessionParentDirName))
            {
                Debug.Log("Directory not found for current session will create one. For Dump Session Settings Data");
                // N.B: IMPORTANT SHOULD CREATER A DIR
                Directory.CreateDirectory(ExperimentSettings.masterDirName + ExperimentSettings.experimentSessionParentDirName);
                Debug.Log("Created a new directory for the current session");
                writeToFileSessionDataExpStart();
            }
            else if (Directory.Exists(ExperimentSettings.masterDirName + ExperimentSettings.experimentSessionParentDirName))
            {
                Debug.Log("Directory found. For Dump Session Settings Data");
                writeToFileSessionDataExpStart();
            }
        }
        catch (Exception e)
        {
            Debug.Log("Caught exception during dumping of experiment session settings data");
            Debug.Log("Fatal Error. Ensure the dir has been created");
            Debug.LogException(e);
            // This will cause the application to crash
            System.Diagnostics.Debugger.Launch();
        }
    }
    static void writeToFileSessionDataExpEnd()
    {
        Debug.Log("Data Dump Starting. For Dump Session Settings Data End");
        File.AppendAllText(ExperimentSettings.experimentSessionSettingsFileName, "\n Experiment Session Settings Details Ending: ");
        File.AppendAllText(ExperimentSettings.experimentSessionSettingsFileName, "\n Last Conducted Session No: " + ExperimentSettings.currentTrialCount);
        File.AppendAllText(ExperimentSettings.experimentSessionSettingsFileName, "\n Experiment Session End Time: " + ExperimentSettings.experimentSessionEndTime);
        File.AppendAllText(ExperimentSettings.experimentSessionSettingsFileName, "\n Experiment Session Total Duration: " + ExperimentSettings.experimentSessionTotalDuration);
        Debug.Log("Data Dump Complete. For Dump Session Settings Data at end");
    }
    public static void DumpSessionDataExpEnd()
    {
        try
        {
            Debug.Log("Start dumping session settings for current session data into FS at experiment end.");
            // Check if Dir does not exist create a new one
            if (!Directory.Exists(ExperimentSettings.masterDirName + ExperimentSettings.experimentSessionParentDirName))
            {
                Debug.Log("Directory not found for current session will create one. For Dump Session Settings Data");
                Directory.CreateDirectory(ExperimentSettings.masterDirName + ExperimentSettings.experimentSessionParentDirName);
                Debug.Log("ALERT !!. Why this dir not created earlier. Created a new directory for the current session");
                writeToFileSessionDataExpEnd();
            }
            else if (Directory.Exists(ExperimentSettings.masterDirName + ExperimentSettings.experimentSessionParentDirName))
            {
                Debug.Log("Directory found. For Dump Session Settings Data");
                writeToFileSessionDataExpEnd();
            }
        }
        catch (Exception e)
        {
            Debug.Log("Caught exception during dumping of experiment session settings data at the end");
            Debug.LogException(e);
        }
    }
    static void writeToFileTrialData(TrialValue dumpTrialData)
    {
        dumpTrialData.trial_trialFileName = Path.Combine(ExperimentSettings.masterDirName, ExperimentSettings.experimentSessionParentDirName + "/" + "Trial" + "_" + ExperimentSettings.currentTrialCount.ToString() + ".json");
        Debug.Log("Dumping trial data details into: " + dumpTrialData.trial_trialFileName);
        File.AppendAllText(dumpTrialData.trial_trialFileName, JsonUtility.ToJson(dumpTrialData));
        Debug.Log("Completed dumping of trial data");
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
                Debug.Log("ALERT !!. Why this dir not created earlier. Created a new directory for the current trial");
                Directory.CreateDirectory(ExperimentSettings.masterDirName + ExperimentSettings.experimentSessionParentDirName);
                writeToFileTrialData(dumpTrialData);
            }
            else
            {
                writeToFileTrialData(dumpTrialData);
            }
        }
        catch (Exception e)
        {
            Debug.Log("Caught exception during dumping of experiment session settings data at the end");
            Debug.LogException(e);
        }

    }
    // Function to dump hand delivery cue end time
    // Here we don't to dir check as we assume it should already  have bee created at the session start itself
    public static void DumpHandCueEndTime(string handCue)
    {
        string handCueDumpFileName = Path.Combine(ExperimentSettings.masterDirName, ExperimentSettings.experimentSessionParentDirName + "/handCueEndTime.txt");
        string handCueEndTime = DateTime.Now.ToString();
        File.AppendAllText(handCueDumpFileName, ("Hand Cue: " + handCue + " completed at: " + handCueEndTime + "\n"));
    }
}



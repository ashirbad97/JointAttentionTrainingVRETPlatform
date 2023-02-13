using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// [System.Serializable]
public class ExperimentSettings
{
    //Static Parameters constant throughout the experiment sessions
    public static int uid, trialCount, targetObjectsCount;
    public static float faceFixationDuration = 10f, responseRegistrationFixationDuration = 10f, cueDeliveryDuration = 10f; //To prevent the cue delivery right away when the scene is only run
    public static bool is_enforceEyeContact, is_conditionedCueSequence, is_keepFingerPointing;
    public static string targetObjectArrayIndices;
    public static string avatar;
    public static string experimentMetaData;
    public static int currentTrialCount = 1;
    public static string experimentSessionTotalDuration;
    public static string experimentSessionStartTime;
    public static string experimentSessionEndTime;
    public static string masterDirName = "D:/JAExpTrialOutput/";
    public static string experimentSessionParentDirName;
    public static string experimentSessionSettingsFileName;
    public static TrialValue prevTrialData;// Will be updated after each trial is over to be displayed in the Interim Menus
}

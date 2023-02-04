using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// [System.Serializable]
public class ExperimentSettings
{
    //Static Parameters constant throughout the experiment sessions
    public static int uid, trialCount, targetObjectsCount;
    public static float faceFixationDuration = 10f, responseRegistrationFixationDuration = 10f, cueDeliveryDuration = 10f; //To prevent the cue delivery right away when the scene is only run
    public static bool enforceEyeContact;
    public static int[] targetObjectArrayIndices;
    public static string avatar;
    public static string experimentMetaData;
    public static int currentTrialCount = 1;
}

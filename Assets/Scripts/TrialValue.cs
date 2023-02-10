using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class TrialValue
{
    //Parameters unique for each trial
    public int trial_currentTrialNo;
    public List<string> trial_spawnedObjects;
    public string trial_targetDirection;
    public string trial_targetObject;
    public string trial_registeredObject;
    public float trial_durationBtwCueDeliveryAndResponseRegistration;
    public bool trial_isCorrectObject;
    public string trial_currentAvatar;
    public string trial_startTime;
    public string trial_endTime;
    public bool trial_isCustomTrial;
    public bool trial_isGazedObjCorrect;
    public bool trial_isHandCueUsed;
    public float trial_totalDuration;
    public string trial_trialFileName;

}
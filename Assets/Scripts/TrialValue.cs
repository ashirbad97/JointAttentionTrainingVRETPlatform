using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[System.Serializable]
public class TrialValue
{
    //Parameters unique for each trial
    public int trial_currentTrialNo;
    public List<string> trial_spawnedObjects;
    public string trial_targetDirection;
    public string trial_targetObject;
    public string trial_registeredObject;
    public bool trial_isGazedObjCorrect;
    public bool trial_isHandCueUsed;
    public string trial_totalDuration;
    public string trial_startTime;
    public string trial_endTime;
    public float trial_durationBtwCueDeliveryAndResponseRegistration;
    public string trial_currentAvatar;
    public bool trial_isCustomTrial;
    public string trial_trialFileName;
    public string trial_feedback;
    public bool trial_isAdditionalCue;
    public string trial_wrongObjAftInitialCue;
    public string trial_wrongObjAftrInitialCue_registrationTime;
    public string trial_headCueDeliveryStart;
    public string trial_headCueDeliveryEnd;
    public string trial_eyeContactEstablishedTime;
    public string trial_totalDurationToEstablishEyeContactSinceTrialStart;
}
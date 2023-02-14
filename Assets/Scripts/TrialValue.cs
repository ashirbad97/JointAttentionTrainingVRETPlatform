using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[System.Serializable]
public class TrialValue
{
    //Parameters unique for each trial
    public int trial_currentTrialNo; // Current Trial No
    public List<string> trial_spawnedObjects;// Names of spawned gameObjs
    public string trial_targetDirection;// Direction towards which the avatar is pointing
    public string trial_targetObject;// Object towards which the avatar is pointing
    public string trial_registeredObject;// Object which the user has gazed
    public bool trial_isGazedObjCorrect;// If user has gazed correct object or not
    public bool trial_isHandCueUsed;
    public string trial_totalDuration;// Total duration of the trial (from start of trial till the time the user has registered an object)
    public string trial_startTime;// Time stamp of trial starttime (use DateTime.Now)
    public string trial_endTime;// Timestamp of trial end time (when the user has registered an object)
    public float trial_durationBtwCueDeliveryAndResponseRegistration;// Duration when the cue delivery started till the user response of an object
    public string trial_currentAvatar;// Avatar name (name the avatar as "Ashirbad")
    public bool trial_isCustomTrial;
    public string trial_trialFileName;// The .json filename in which all the trial data is being stored (this data)
    public string trial_feedback;
    public bool trial_isAdditionalCue;
    public string trial_wrongObjAftInitialCue;
    public string trial_wrongObjAftrInitialCue_registrationTime;
    public string trial_headCueDeliveryStart;
    public string trial_headCueDeliveryEnd;
    public string trial_eyeContactEstablishedTime;// Time stamp when user has established eye contact with avatar head region
    public string trial_totalDurationToEstablishEyeContactSinceTrialStart;// Total duration since the start of trial till finally establishing eye contact with avatar
}
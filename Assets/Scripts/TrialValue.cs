using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class TrialValue
{
    //Parameters unique for each trial
    public int trial_currentTrialNo;
    public GameObject[] trial_spawnedObjects;
    public int trial_targetObject;
    public int trial_registeredObject;
    public float trial_durationBtwCueDeliveryAndResponseRegistration;
    public bool trial_isCorrectObject;
    public string trial_currentAvatar;
    public string trial_startTime;
    public string trial_endTime;
    public bool trial_isCustomTrial;
}